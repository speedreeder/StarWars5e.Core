using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Lookup;
using StarWars5e.Models.Monster;
using StarWars5e.Models.Species;
using StarWars5e.Models.Utils;
using StarWars5e.Parser.Localization;
using Attribute = StarWars5e.Models.Enums.Attribute;
// ReSharper disable StringLiteralTypo

namespace StarWars5e.Parser.Processors
{
    public class ExpandedContentSpeciesProcessor : BaseProcessor<Species>
    {
        private readonly List<SpeciesImageUrlLU> _speciesImageUrlLus;

        public ExpandedContentSpeciesProcessor(ILocalization localization, List<SpeciesImageUrlLU> speciesImageUrlLus)
        {
            _speciesImageUrlLus = speciesImageUrlLus;
            Localization = localization;
        }

        public override Task<List<Species>> FindBlocks(List<string> lines)
        {
            var species = new List<Species>();

            for (var i = 0; i < lines.Count; i++)
            {
                if (!Regex.IsMatch(lines[i], @"^>\s*##\s+")) continue;

                //var speciesEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("> ## ") || f.StartsWith(">## ") || f.start);
                var speciesEndIndex = lines.FindIndex(i + 1, f => Regex.IsMatch(f, @"^>\s*##\s+"));
                var speciesLines = lines.Skip(i).CleanListOfStrings().ToList();
                if (speciesEndIndex != -1)
                {
                    speciesLines = lines.Skip(i).Take(speciesEndIndex - i).CleanListOfStrings().ToList();
                }

                species.Add(ParseSpecies(speciesLines, ContentType.ExpandedContent));
            }

            return Task.FromResult(species);
        }

        public Species ParseSpecies(List<string> speciesLines, ContentType contentType)
        {
            var name = speciesLines.Find(f => Regex.IsMatch(f, @"^>\s*##\s+")).Split("## ")[1].Trim().RemoveMarkdownCharacters();
            try
            {
                var species = new Species
                {
                    ContentTypeEnum = contentType,
                    Name = name,
                    RowKey = name,
                    PartitionKey = contentType.ToString()
                };

                var descriptionStart = speciesLines.FindIndex(f => f.StartsWith("### "));
                var traitsStart = speciesLines.FindIndex(f => f.StartsWith("### ") && f.Contains(Localization.Traits));
                species.FlavorText = string.Join("\r\n",
                    speciesLines.Skip(descriptionStart).Take(traitsStart - descriptionStart).CleanListOfStrings().ToList());

                var traitLines = speciesLines.Skip(traitsStart).ToList().Where(t => t.StartsWith("***") || t.StartsWith("**"));

                species.ColorScheme = speciesLines.Find(f => f.Contains(Localization.ECSpeciesColorScheme))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();
                species.SkinColorOptions = speciesLines.Find(f => f.Contains(Localization.ECSpeciesSkinColor))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();
                species.HairColorOptions = speciesLines.Find(f => f.Contains(Localization.ECSpeciesHairColor))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();
                species.EyeColorOptions = speciesLines.Find(f => f.Contains(Localization.ECSpeciesEyeColor))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();
                species.Distinctions = speciesLines.Find(f => f.Contains(Localization.ECSpeciesDistinctions))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();
                species.HeightAverage = speciesLines.Find(f => f.Contains(Localization.ECSpeciesHeight))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();
                species.HeightRollMod = speciesLines.Find(f => f.Contains(Localization.ECSpeciesHeight))?.Split('|')
                    .Reverse().Skip(1).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                species.WeightAverage = speciesLines.Find(f => f.Contains(Localization.ECSpeciesWeight))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();
                species.WeightRollMod = speciesLines.Find(f => f.Contains(Localization.ECSpeciesWeight))?.Split('|')
                    .Reverse().Skip(1).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                species.Homeworld = speciesLines.Find(f => f.Contains(Localization.ECSpeciesHomeworld))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();              
                species.Manufacturer = speciesLines.Find(f => f.Contains(Localization.ECSpeciesManufacturer))?.Split('|')
                    .Skip(2).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();

                species.Language = speciesLines.Find(f => f.Contains(Localization.ECSpeciesLanguage))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();
                if (species.Language == null)
                {
                    species.Language = speciesLines.Find(f => f.Contains(Localization.ECSpeciesPrimaryLanguage))?.Split('|')
                        .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();
                }

                species.Traits = new List<Trait>();
                foreach (var traitLine in traitLines)
                {
                    var trait = new Trait();
                    var asterisks = Regex.Match(traitLine, @"\*+");

                    if (!asterisks.Success) continue;

                    trait.Name = traitLine.Split(asterisks.Value)[1].Trim().Replace(".", string.Empty);
                    trait.Description = traitLine.Split(asterisks.Value)[2].Trim().RemoveHtmlWhitespace();
                    species.Traits.Add(trait);
                }

                foreach (var speciesTrait in species.Traits)
                {
                    var feature = new Feature
                    {
                        Name = speciesTrait.Name,
                        Text = speciesTrait.Description,
                        RowKey = $"{FeatureSource.Species}-{name}-{speciesTrait.Name}",
                        SourceName = name,
                        SourceEnum = FeatureSource.Species,
                        PartitionKey = contentType.ToString()
                    };
                    species.Features.Add(feature);
                }

                var sizeTrait = species.Traits.SingleOrDefault(t => t.Name == Localization.Size);
                if (sizeTrait != null)
                {
                    if (sizeTrait.Description.Contains(MonsterSize.Gargantuan.ToString()))
                    {
                        species.Size = MonsterSize.Gargantuan.ToString();
                    }

                    if (sizeTrait.Description.Contains(MonsterSize.Huge.ToString()))
                    {
                        species.Size = MonsterSize.Huge.ToString();
                    }

                    if (sizeTrait.Description.Contains(MonsterSize.Large.ToString()))
                    {
                        species.Size = MonsterSize.Large.ToString();
                    }

                    if (sizeTrait.Description.Contains(MonsterSize.Medium.ToString()))
                    {
                        species.Size = MonsterSize.Medium.ToString();
                    }

                    if (sizeTrait.Description.Contains(MonsterSize.Small.ToString()))
                    {
                        species.Size = MonsterSize.Small.ToString();
                    }

                    if (sizeTrait.Description.Contains(MonsterSize.Tiny.ToString()))
                    {
                        species.Size = MonsterSize.Tiny.ToString();
                    }
                }

                var attributeIncreaseTrait = species.Traits.SingleOrDefault(t =>
                    t.Name.Contains(Localization.ECSpeciesAbilityScoreIncrease, StringComparison.InvariantCultureIgnoreCase));
                if (attributeIncreaseTrait != null)
                {
                    species.AbilitiesIncreased = new List<List<AbilityIncrease>>();
                    var alternateSplit = attributeIncreaseTrait.Description.Split(".");
                    foreach (var alternate in alternateSplit.Take(alternateSplit.Length - 1))
                    {
                        var abilitiesSplit = alternate.Split(new []{ ", ", Localization.and }, StringSplitOptions.None);
                        var abilityIncreases = new List<AbilityIncrease>();
                        foreach (var abilitySplit in abilitiesSplit)
                        {
                            if(!Localization.ECValidAttributeHints.Any(v => abilitySplit.Contains(v))) continue;
                            var abilityIncrease = new AbilityIncrease();

                            if (abilitySplit.Contains(Attribute.Strength.ToString()))
                            {
                                abilityIncrease.Abilities.Add(Attribute.Strength.ToString()); 

                                var abilityIndex = abilitySplit.IndexOf(Attribute.Strength.ToString(),
                                    StringComparison.InvariantCultureIgnoreCase);

                                var amountMatches = Regex.Matches(abilitySplit, @"\d+");
                                var amountMatch = amountMatches.FirstOrDefault(a => a.Index > abilityIndex);
                                abilityIncrease.Amount = amountMatch != null ? int.Parse(amountMatch.Value) : 0;
                            }

                            if (abilitySplit.Contains(Attribute.Dexterity.ToString()))
                            {
                                abilityIncrease.Abilities.Add(Attribute.Dexterity.ToString());

                                var abilityIndex = abilitySplit.IndexOf(Attribute.Dexterity.ToString(),
                                    StringComparison.InvariantCultureIgnoreCase);

                                var amountMatches = Regex.Matches(abilitySplit, @"\d+");
                                var amountMatch = amountMatches.FirstOrDefault(a => a.Index > abilityIndex);
                                abilityIncrease.Amount = amountMatch != null ? int.Parse(amountMatch.Value) : 0;
                            }

                            if (abilitySplit.Contains(Attribute.Constitution.ToString()))
                            {
                                abilityIncrease.Abilities.Add(Attribute.Constitution.ToString());

                                var abilityIndex = abilitySplit.IndexOf(Attribute.Constitution.ToString(),
                                    StringComparison.InvariantCultureIgnoreCase);

                                var amountMatches = Regex.Matches(abilitySplit, @"\d+");
                                var amountMatch = amountMatches.FirstOrDefault(a => a.Index > abilityIndex);
                                abilityIncrease.Amount = amountMatch != null ? int.Parse(amountMatch.Value) : 0;
                            }

                            if (abilitySplit.Contains(Attribute.Intelligence.ToString()))
                            {
                                abilityIncrease.Abilities.Add(Attribute.Intelligence.ToString());

                                var abilityIndex = abilitySplit.IndexOf(Attribute.Intelligence.ToString(),
                                    StringComparison.InvariantCultureIgnoreCase);

                                var amountMatches = Regex.Matches(abilitySplit, @"\d+");
                                var amountMatch = amountMatches.FirstOrDefault(a => a.Index > abilityIndex);
                                abilityIncrease.Amount = amountMatch != null ? int.Parse(amountMatch.Value) : 0;
                            }

                            if (abilitySplit.Contains(Attribute.Wisdom.ToString()))
                            {
                                abilityIncrease.Abilities.Add(Attribute.Wisdom.ToString());

                                var abilityIndex = abilitySplit.IndexOf(Attribute.Wisdom.ToString(),
                                    StringComparison.InvariantCultureIgnoreCase);

                                var amountMatches = Regex.Matches(abilitySplit, @"\d+");
                                var amountMatch = amountMatches.FirstOrDefault(a => a.Index > abilityIndex);
                                abilityIncrease.Amount = amountMatch != null ? int.Parse(amountMatch.Value) : 0;
                            }

                            if (abilitySplit.Contains(Attribute.Charisma.ToString()))
                            {
                                abilityIncrease.Abilities.Add(Attribute.Charisma.ToString());

                                var abilityIndex = abilitySplit.IndexOf(Attribute.Charisma.ToString(),
                                    StringComparison.InvariantCultureIgnoreCase);

                                var amountMatches = Regex.Matches(abilitySplit, @"\d+");
                                var amountMatch = amountMatches.FirstOrDefault(a => a.Index > abilityIndex);
                                abilityIncrease.Amount = amountMatch != null ? int.Parse(amountMatch.Value) : 0;
                            }

                            if (abilitySplit.Contains(Localization.choice, StringComparison.InvariantCultureIgnoreCase))
                            {
                                var otherAmount = "";
                                if (abilitySplit.Contains(Localization.one, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    otherAmount = Localization.one;
                                }

                                if (abilitySplit.Contains(Localization.two, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    otherAmount = Localization.two;
                                }

                                if (abilitySplit.Contains(Localization.three, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    otherAmount = Localization.three;
                                }

                                if (abilitySplit.Contains(Localization.four, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    otherAmount = Localization.four;
                                }

                                if (abilitySplit.Contains(Localization.five, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    otherAmount = Localization.five;
                                }

                                if (abilitySplit.Contains(Localization.six, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    otherAmount = Localization.six;
                                }

                                abilityIncrease.Abilities.Add($"{Localization.Any} {otherAmount}");

                                var abilityIndex = abilitySplit.IndexOf(Attribute.Wisdom.ToString(),
                                    StringComparison.InvariantCultureIgnoreCase);

                                var amountMatches = Regex.Matches(abilitySplit, @"\d+");
                                var amountMatch = amountMatches.FirstOrDefault(a => a.Index > abilityIndex);
                                abilityIncrease.Amount = amountMatch != null ? int.Parse(amountMatch.Value) : 0;
                            }

                            abilityIncreases.Add(abilityIncrease);

                            var abilityIncreasesWithoutAmounts =
                                abilityIncreases.Where(a => !a.Amount.HasValue || a.Amount.Value == 0).ToList();
                            var defaultAmount = abilityIncreases.LastOrDefault(a => a.Amount.HasValue && a.Amount.Value != 0)?.Amount;

                            foreach (var abilityIncreasesWithoutAmount in abilityIncreasesWithoutAmounts)
                            {
                                abilityIncreasesWithoutAmount.Amount = defaultAmount ?? 0;
                            }
                        }

                        species.AbilitiesIncreased.Add(abilityIncreases);
                    }
                }

                if (name.Equals(Localization.ECSpeciesHalfHuman, StringComparison.InvariantCultureIgnoreCase))
                {
                    var tableLines = speciesLines.Where(c => Regex.IsMatch(c, @"^>\s*\|[A-Za-z]+")).ToList();

                    foreach (var tableLine in tableLines)
                    {
                        var specie = tableLine.Split('|')[1].Trim();
                        var traits = tableLine.Split('|')[2].Trim();

                        species.HalfHumanTableEntries.Add(specie, traits);
                    }
                }

                species.ImageUrls = _speciesImageUrlLus.Where(s => s.Specie.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Select(s => s.Url).ToList();

                return species;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing {name}", e);
            }
        }
    }
}
