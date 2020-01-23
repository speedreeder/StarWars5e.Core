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
// ReSharper disable StringLiteralTypo

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

            MapImageUrls(species);

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

                var sizeTrait = species.Traits.SingleOrDefault(t => t.Name == "Size");
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
                    t.Name.Contains("Ability Score Increase", StringComparison.InvariantCultureIgnoreCase));
                if (attributeIncreaseTrait != null)
                {
                    species.AbilitiesIncreased = new List<List<AbilityIncrease>>();
                    var alternateSplit = attributeIncreaseTrait.Description.Split(".");
                    foreach (var alternate in alternateSplit.Take(alternateSplit.Length - 1))
                    {
                        var abilitiesSplit = alternate.Split(new []{ ", ", "and" }, StringSplitOptions.None);
                        var abilityIncreases = new List<AbilityIncrease>();
                        foreach (var abilitySplit in abilitiesSplit)
                        {
                            if(!ValidAttributeHints.Any(v => abilitySplit.Contains(v))) continue;
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

                            if (abilitySplit.Contains("choice", StringComparison.InvariantCultureIgnoreCase))
                            {
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

                                abilityIncrease.Abilities.Add($"Any {otherAmount}");

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

                if (name == "Half-Human")
                {
                    var tableLines = speciesLines.Where(c => Regex.IsMatch(c, @"^>\s*\|[A-Za-z]+")).ToList();

                    foreach (var tableLine in tableLines)
                    {
                        var specie = tableLine.Split('|')[1].Trim();
                        var traits = tableLine.Split('|')[2].Trim();

                        species.HalfHumanTableEntries.Add(specie, traits);
                    }
                }

                return species;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing {name}", e);
            }
        }

        public static void MapImageUrls(IEnumerable<Species> species)
        {
            foreach (var specie in species)
            {
                switch (specie.Name)
                {
                    case "Aing-Tii":
                        specie.ImageUrls.Add("https://starwars5e.blob.core.windows.net/site-images/species/species_aing-tii.png");
                        break;
                    case "Aqualish":
                        specie.ImageUrls.Add("https://starwars5ecentralcentral.blob.core.windows.net/site-images/species/species_aqualish.png");
                        break;
                    case "Ardennian":
                        specie.ImageUrls.Add("https://starwars5ecentralcentral.blob.core.windows.net/site-images/species/species_ardennian.png");
                        break;
                    case "Balosar":
                        specie.ImageUrls.Add("https://starwars5ecentralcentral.blob.core.windows.net/site-images/species/species_balosar.png");
                        break;
                    case "Barabel":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_barabel.png");
                        break;
                    case "Besalisk":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_besalisk.png");
                        break;
                    case "Bith":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_bith.png");
                        break;
                    case "Bothan":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_bothan.png");
                        break;
                    case "Cathar":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_cathar.png");
                        break;
                    case "Cerean":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_cerean.png");
                        break;
                    case "Chadra-Fan":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_chadrafan.png");
                        break;
                    case "Chagrian":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_chagrian.png");
                        break;
                    case "Chevin":
                        specie.ImageUrls.Add("https://starwars5ecentralcentral.blob.core.windows.net/site-images/species/species_chevin.png");
                        break;
                    case "Chiss":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_chiss.png");
                        break;
                    case "Codru-Ji":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_codru-ji.png");
                        break;
                    case "Devaronian":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_devaronian.png");
                        break;
                    case "Draethos":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_draethos.png");
                        break;
                    case "Droid, Class I":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_droidclass01.png");
                        break;
                    case "Droid, Class IV":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_droidclass04.png");
                        break;
                    case "Droid, Class III":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_droidclas03_01.png");
                        break;
                    case "Droid, Class II":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_droidclass02_01.png");
                        break;
                    case "Droid, Class V":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_droidclass05.png");
                        break;
                    case "Dug":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_dug.png");
                        break;
                    case "Duros":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_duros.png");
                        break;
                    case "Echani":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_echani.png");
                        break;
                    case "Ewok":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_ewok.png");
                        break;
                    case "Falleen":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_falleen.png");
                        break;
                    case "Felucian":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_felucian.png");
                        break;
                    case "Gamorrean":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_gamorrean.png");
                        break;
                    case "Gand":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_gand.png");
                        break;
                    case "Geonosian":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_geonosian.png");
                        break;
                    case "Givin":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_givin.png");
                        break;
                    case "Gotal":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_gotal.png");
                        break;
                    case "Gran":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_gran.png");
                        break;
                    case "Gungan":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_gungan.jpg");
                        break;
                    case "Harch":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_harch.png");
                        break;
                    case "Herglic":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_herglic.png");
                        break;
                    case "Human":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_human.png");
                        break;
                    case "Iktotchi":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_iktotchi.png");
                        break;
                    case "Ithorian":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_ithorian.png");
                        break;
                    case "Jawa":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_jawa.png");
                        break;
                    case "Kaleesh":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_kaleesh.png");
                        break;
                    case "Kaminoan":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_kaminoan.png");
                        break;
                    case "Karkarodon":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_karkarodon.png");
                        break;
                    case "Kel Dor":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_keldor.png");
                        break;
                    case "Killik":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_killik.png");
                        break;
                    case "Klatooinian":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_klatooinian.png");
                        break;
                    case "Kubaz":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_kubaz.png");
                        break;
                    case "Kushiban":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_kushiban.png");
                        break;
                    case "Kyuzo":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_kyuzo.png");
                        break;
                    case "Lannik":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_lannik.png");
                        break;
                    case "Lasat":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_lassat.jpg");
                        break;
                    case "Miraluka":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_miraluka.png");
                        break;
                    case "Mirialan":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_mirialan.png");
                        break;
                    case "Mon Calamari":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_moncalamari.png");
                        break;
                    case "Mustafarian":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_mustafarian.png");
                        break;
                    case "Muun":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_muun.png");
                        break;
                    case "Nautolan":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_nautolan.jpg");
                        break;
                    case "Neimoidian":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_neimoidian.jpg");
                        break;
                    case "Noghri":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_noghri.png");
                        break;
                    case "Ortolan":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_ortolan.png");
                        break;
                    case "Pau'an":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_pau\'an.png");
                        break;
                    case "Quarren":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_quarren.png");
                        break;
                    case "Rattataki":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_rattataki.png");
                        break;
                    case "Rishii":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_rishii.png");
                        break;
                    case "Rodian":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_rodian.png");
                        break;
                    case "Selkath":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_selkath.png");
                        break;
                    case "Shistavanen":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_shistavanen.png");
                        break;
                    case "Sith Pureblood":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_sith.png");
                        break;
                    case "Squib":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_squib.png");
                        break;
                    case "Ssi-Ruu":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_ssi-ruu.jpg");
                        break;
                    case "Sullustan":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_sullustan.png");
                        break;
                    case "Talz":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_talz.png");
                        break;
                    case "Thisspiasian":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_thisspiasian.png");
                        break;
                    case "Togorian":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_togorian.png");
                        break;
                    case "Togruta":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_togruta.png");
                        break;
                    case "Toydarian":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_toydarian.png");
                        break;
                    case "Trandoshan":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_trandoshan (2).png");
                        break;
                    case "Tusken":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_tusken.png");
                        break;
                    case "Twi'lek":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_twilek.png");
                        break;
                    case "Ugnaught":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_ugnaught.jpg");
                        break;
                    case "Umbaran":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_umbaran.png");
                        break;
                    case "Verpine":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_verpine.png");
                        break;
                    case "Voss":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_voss.png");
                        break;
                    case "Vurk":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_vurk.png");
                        break;
                    case "Weequay":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_weequay.png");
                        break;
                    case "Wookiee":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_wookiee.png");
                        break;
                    case "Yevetha":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_yevetha.png");
                        break;
                    case "Zabrak":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_zabrak.png");
                        break;
                    case "Zeltron":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_zeltron.png");
                        break;
                    case "Zygerrian":
                        specie.ImageUrls.Add("https://starwars5ecentral.blob.core.windows.net/site-images/species/species_zygerrian.png");
                        break;
                }
            }
        }
    }
}
