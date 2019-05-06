using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Monster;
using StarWars5e.Models.Species;
using StarWars5e.Models.Utils;
using Attribute = StarWars5e.Models.Enums.Attribute;

namespace StarWars5e.Parser.Parsers
{
    public class ExpandedContentSpeciesProcessor : BaseProcessor<Species>
    {
        private static readonly List<string> ValidAttributeHints = new List<string>
        {
            Attribute.Charisma.ToString(), Attribute.Constitution.ToString(), Attribute.Dexterity.ToString(),
            Attribute.Intelligence.ToString(), Attribute.Strength.ToString(), Attribute.Wisdom.ToString(), "choice"
        };

        public override Task<List<Species>> FindBlocks(List<string> lines)
        {
            var species = new List<Species>();

            for (var i = 0; i < lines.Count; i++)
            {
                if (!lines[i].StartsWith("> ## ") && !lines[i].StartsWith(">## ")) continue;
                
                var speciesEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("> ## ") || f.StartsWith(">## "));
                var speciesLines = lines.Skip(i).CleanListOfStrings().ToList();
                if (speciesEndIndex != -1)
                {
                    speciesLines = lines.Skip(i).Take(speciesEndIndex - i).CleanListOfStrings().ToList();
                }

                species.Add(ParseSpecies(speciesLines, ContentType.ExpandedContent));
            }

            return Task.FromResult(species);
        }

        public static Species ParseSpecies(List<string> speciesLines, ContentType contentType)
        {
            var name = speciesLines.Find(f => f.StartsWith("> ## ") || f.StartsWith(">## ")).Split("## ")[1].Trim().RemoveMarkdownCharacters();
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
                var traitsStart = speciesLines.FindIndex(f => f.StartsWith("### ") && f.Contains("Traits"));
                species.FlavorText = string.Join("\r\n",
                    speciesLines.Skip(descriptionStart).Take(traitsStart - descriptionStart).CleanListOfStrings().ToList());

                var traitLines = speciesLines.Skip(traitsStart).ToList().Where(t => t.StartsWith("***") || t.StartsWith("**"));

                species.ColorScheme = speciesLines.Find(f => f.Contains("***Color Scheme***"))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();
                species.SkinColorOptions = speciesLines.Find(f => f.Contains("***Skin Color***"))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();
                species.HairColorOptions = speciesLines.Find(f => f.Contains("***Hair Color***"))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();
                species.EyeColorOptions = speciesLines.Find(f => f.Contains("***Eye Color***"))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();
                species.Distinctions = speciesLines.Find(f => f.Contains("***Distinctions***"))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();
                species.HeightAverage = speciesLines.Find(f => f.Contains("***Height***"))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();
                species.HeightRollMod = speciesLines.Find(f => f.Contains("***Height***"))?.Split('|')
                    .Reverse().Skip(1).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                species.WeightAverage = speciesLines.Find(f => f.Contains("***Weight***"))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();
                species.WeightRollMod = speciesLines.Find(f => f.Contains("***Weight***"))?.Split('|')
                    .Reverse().Skip(1).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                species.Homeworld = speciesLines.Find(f => f.Contains("***Homeworld***"))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();              
                species.Manufacturer = speciesLines.Find(f => f.Contains("***Manufacturer***"))?.Split('|')
                    .Skip(2).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();

                species.Language = speciesLines.Find(f => f.Contains("***Language***"))?.Split('|')
                    .Skip(2).FirstOrDefault(f => !f.IsJustHtmlOrOtherWhitespace())?.Trim().RemoveHtmlWhitespace();
                if (species.Language == null)
                {
                    species.Language = speciesLines.Find(f => f.Contains("***Primary Language***"))?.Split('|')
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

                var attributeIncreaseTrait = species.Traits.SingleOrDefault(t =>
                    t.Name.Contains("Ability Score Increase", StringComparison.InvariantCultureIgnoreCase));
                if (attributeIncreaseTrait != null)
                {
                    species.AbilitiesIncreased = new List<List<AbilityIncrease>>();
                    var alternateSplit = attributeIncreaseTrait.Description.Split(".");
                    foreach (var alternate in alternateSplit.Take(alternateSplit.Length - 1))
                    {
                        var abilitiesSplit = alternate.Split(",");
                        var abilityIncreases = new List<AbilityIncrease>();
                        foreach (var abilitySplit in abilitiesSplit)
                        {
                            if(!ValidAttributeHints.Any(v => abilitySplit.Contains(v))) continue;

                            if (abilitySplit.Contains(Attribute.Strength.ToString()))
                            {
                                var abilityIncrease = new AbilityIncrease();
                                abilityIncrease.Ability = Attribute.Strength.ToString();

                                var abilityIndex = abilitySplit.IndexOf(Attribute.Strength.ToString(),
                                    StringComparison.InvariantCultureIgnoreCase);

                                var amountMatches = Regex.Matches(abilitySplit, @"\d+");
                                var amountMatch = amountMatches.FirstOrDefault(a => a.Index > abilityIndex);
                                abilityIncrease.Amount = amountMatch != null ? int.Parse(amountMatch.Value) : 0;
                                abilityIncreases.Add(abilityIncrease);
                            }

                            if (abilitySplit.Contains(Attribute.Dexterity.ToString()))
                            {
                                var abilityIncrease = new AbilityIncrease();
                                abilityIncrease.Ability = Attribute.Dexterity.ToString();

                                var abilityIndex = abilitySplit.IndexOf(Attribute.Dexterity.ToString(),
                                    StringComparison.InvariantCultureIgnoreCase);

                                var amountMatches = Regex.Matches(abilitySplit, @"\d+");
                                var amountMatch = amountMatches.FirstOrDefault(a => a.Index > abilityIndex);
                                abilityIncrease.Amount = amountMatch != null ? int.Parse(amountMatch.Value) : 0;
                                abilityIncreases.Add(abilityIncrease);
                            }

                            if (abilitySplit.Contains(Attribute.Constitution.ToString()))
                            {
                                var abilityIncrease = new AbilityIncrease();
                                abilityIncrease.Ability = Attribute.Constitution.ToString();

                                var abilityIndex = abilitySplit.IndexOf(Attribute.Constitution.ToString(),
                                    StringComparison.InvariantCultureIgnoreCase);

                                var amountMatches = Regex.Matches(abilitySplit, @"\d+");
                                var amountMatch = amountMatches.FirstOrDefault(a => a.Index > abilityIndex);
                                abilityIncrease.Amount = amountMatch != null ? int.Parse(amountMatch.Value) : 0;
                                abilityIncreases.Add(abilityIncrease);
                            }

                            if (abilitySplit.Contains(Attribute.Intelligence.ToString()))
                            {
                                var abilityIncrease = new AbilityIncrease();
                                abilityIncrease.Ability = Attribute.Intelligence.ToString();

                                var abilityIndex = abilitySplit.IndexOf(Attribute.Intelligence.ToString(),
                                    StringComparison.InvariantCultureIgnoreCase);

                                var amountMatches = Regex.Matches(abilitySplit, @"\d+");
                                var amountMatch = amountMatches.FirstOrDefault(a => a.Index > abilityIndex);
                                abilityIncrease.Amount = amountMatch != null ? int.Parse(amountMatch.Value) : 0;
                                abilityIncreases.Add(abilityIncrease);
                            }

                            if (abilitySplit.Contains(Attribute.Wisdom.ToString()))
                            {
                                var abilityIncrease = new AbilityIncrease();
                                abilityIncrease.Ability = Attribute.Wisdom.ToString();

                                var abilityIndex = abilitySplit.IndexOf(Attribute.Wisdom.ToString(),
                                    StringComparison.InvariantCultureIgnoreCase);

                                var amountMatches = Regex.Matches(abilitySplit, @"\d+");
                                var amountMatch = amountMatches.FirstOrDefault(a => a.Index > abilityIndex);
                                abilityIncrease.Amount = amountMatch != null ? int.Parse(amountMatch.Value) : 0;
                                abilityIncreases.Add(abilityIncrease);
                            }

                            if (abilitySplit.Contains(Attribute.Charisma.ToString()))
                            {
                                var abilityIncrease = new AbilityIncrease();
                                abilityIncrease.Ability = Attribute.Charisma.ToString();

                                var abilityIndex = abilitySplit.IndexOf(Attribute.Charisma.ToString(),
                                    StringComparison.InvariantCultureIgnoreCase);

                                var amountMatches = Regex.Matches(abilitySplit, @"\d+");
                                var amountMatch = amountMatches.FirstOrDefault(a => a.Index > abilityIndex);
                                abilityIncrease.Amount = amountMatch != null ? int.Parse(amountMatch.Value) : 0;
                                abilityIncreases.Add(abilityIncrease);
                            }

                            if (abilitySplit.Contains("choice", StringComparison.InvariantCultureIgnoreCase))
                            {
                                var abilityIncrease = new AbilityIncrease();
                                var otherAmount = "";
                                if (abilitySplit.Contains("one", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    otherAmount = "one";
                                }

                                if (abilitySplit.Contains("two", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    otherAmount = "two";
                                }

                                if (abilitySplit.Contains("three", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    otherAmount = "three";
                                }

                                if (abilitySplit.Contains("four", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    otherAmount = "four";
                                }

                                if (abilitySplit.Contains("five", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    otherAmount = "five";
                                }

                                if (abilitySplit.Contains("six", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    otherAmount = "six";
                                }

                                abilityIncrease.Ability = $"Any other {otherAmount}";

                                var abilityIndex = abilitySplit.IndexOf(Attribute.Wisdom.ToString(),
                                    StringComparison.InvariantCultureIgnoreCase);

                                var amountMatches = Regex.Matches(abilitySplit, @"\d+");
                                var amountMatch = amountMatches.FirstOrDefault(a => a.Index > abilityIndex);
                                abilityIncrease.Amount = amountMatch != null ? int.Parse(amountMatch.Value) : 0;
                                abilityIncreases.Add(abilityIncrease);
                            }

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

                return species;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing {name}", e);
            }
        }
    }
}
