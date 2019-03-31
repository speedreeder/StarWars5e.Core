using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models.Background;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Parsers
{
    public class BackgroundProcessor : BaseProcessor<Background>
    {
        public override Task<List<Background>> FindBlocks(List<string> lines)
        {
            var species = new List<Background>();

            for (var i = 0; i < lines.Count; i++)
            {
                if (!lines[i].StartsWith("## ") && !lines[i].StartsWith("## ") || lines[i].Contains("Changelog")) continue;

                var backgroundEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("## ") || f.StartsWith("## "));
                var backgroundLines = lines.Skip(i).ToList();
                if (backgroundEndIndex != -1)
                {
                    backgroundLines = lines.Skip(i).Take(backgroundEndIndex - i).CleanListOfStrings().ToList();
                }

                species.Add(ParseBackground(backgroundLines));
            }

            return Task.FromResult(species);
        }

        private static Background ParseBackground(List<string> backgroundLines)
        {
            var name = backgroundLines.Find(f => f.StartsWith("## ") || f.StartsWith("## ")).Split("## ")[1].Trim().RemoveMarkdownCharacters();
            try
            {
                var background = new Background
                {
                    ContentTypeEnum = ContentType.ExpandedContent,
                    Name = name,
                    RowKey = name,
                    PartitionKey = ContentType.ExpandedContent.ToString()
                };

                var nameIndex = backgroundLines.FindIndex(f => f.StartsWith("## ") || f.StartsWith("## "));

                background.FlavorText = string.Join("\r\n",
                    backgroundLines.Skip(nameIndex + 1)
                        .Take(backgroundLines.FindIndex(nameIndex, f => f.StartsWith('_')) - (nameIndex + 1)));

                background.SkillProficiencies = backgroundLines.Find(f => f.Contains("**Skill Proficiencies"))?.Split("**")
                    .Skip(2).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                background.ToolProficiencies = backgroundLines.Find(f => f.Contains("**Tool Proficiencies"))?.Split("**")
                    .Skip(2).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                background.Languages = backgroundLines.Find(f => f.Contains("**Languages"))?.Split("**")
                    .Skip(2).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();
                background.Equipment = backgroundLines.Find(f => f.Contains("**Equipment"))?.Split("**")
                    .Skip(2).FirstOrDefault()?.Trim().RemoveHtmlWhitespace();

                var flavorNameIndex = backgroundLines.FindIndex(f => f.StartsWith("#### ") && !f.Contains("Suggested Characteristics"));
                if (flavorNameIndex != -1)
                {
                    var checkLine = backgroundLines.FindIndex(flavorNameIndex + 1, f => f.StartsWith("###"));

                    background.FlavorName = backgroundLines[flavorNameIndex];
                    background.FlavorDescription = string.Join("\r\n", backgroundLines.Skip(flavorNameIndex + 1).Take(checkLine - (flavorNameIndex + 1)));

                    var flavorTableLinesStart = backgroundLines.FindIndex(f => f.Contains("|"));
                    if (flavorTableLinesStart < checkLine)
                    {
                        var flavorTableDieType = Regex.Match(backgroundLines[flavorTableLinesStart], @"\d+").Value;
                        var flavorTableLinesEnd = backgroundLines.FindIndex(flavorTableLinesStart, f => f.StartsWith($"|{flavorTableDieType}"));
                        var flavorTableLines = backgroundLines.Skip(flavorTableLinesStart).Take(flavorTableLinesEnd - flavorTableLinesStart + 1)
                            .Where(f => Regex.IsMatch(f, @"^\|\d"));
                        background.FlavorOptions = flavorTableLines
                            .Select(f => (int.Parse(Regex.Match(f, @"\d").Value), f.Split('|')[2].Trim())).ToList();
                    }
                }

                var suggestedCharacteristicsStart =
                    backgroundLines.FindIndex(f => f.Contains("#### Suggested Characteristics"));
                background.SuggestedCharacteristics = backgroundLines[suggestedCharacteristicsStart + 1];

                var featureLineStart = backgroundLines.FindIndex(f => f.StartsWith("### Feature"));
                var featureLineEnd = backgroundLines.FindIndex(featureLineStart + 1, f => f.StartsWith("### "));

                background.FeatureName = backgroundLines[featureLineStart].Split(':')[1].Trim();
                background.FeatureText = string.Join("\r\n",
                    backgroundLines.Skip(featureLineStart + 1).Take(featureLineEnd - (featureLineStart + 1)));

                var featTableLinesStart = backgroundLines.FindIndex(f => f.Contains("|Feat"));
                var featTableDieType = Regex.Match(backgroundLines[featTableLinesStart], @"\d+").Value;
                var featTableLinesEnd = backgroundLines.FindIndex(featTableLinesStart, f => Regex.IsMatch(f, @"\|\s*" + $"{featTableDieType}" + @"\s*\|"));
                var featTableLines = backgroundLines.Skip(featureLineStart).Take(featTableLinesEnd - featureLineStart + 1)
                    .Where(f => Regex.IsMatch(f, @"^\|\d"));
                background.FeatOptions = featTableLines
                    .Select(f => new BackgroundOption
                    {
                        Name = f.Split('|')[2].Trim(),
                        Roll = int.Parse(Regex.Match(f, @"\d").Value)
                    }).ToList();

                var personalityTraitTableLinesStart = backgroundLines.FindIndex(f => Regex.IsMatch(f, @"\|.*Personality.*Trait.*\|"));
                var personalityTraitTableDieType = Regex.Match(backgroundLines[personalityTraitTableLinesStart], @"\d+").Value;
                var personalityTraitTableLinesEnd = backgroundLines.FindIndex(personalityTraitTableLinesStart, f => Regex.IsMatch(f, @"\|\s*" + $"{personalityTraitTableDieType}" + @"\s*\|"));
                var personalityTraitTableLines = backgroundLines.Skip(personalityTraitTableLinesStart).Take(personalityTraitTableLinesEnd - personalityTraitTableLinesStart + 1)
                    .Where(f => Regex.IsMatch(f, @"^\|\s*\d"));
                background.PersonalityTraitOptions = personalityTraitTableLines
                    .Select(f => new BackgroundOption
                    {
                        Name = f.Split('|')[2].Trim(),
                        Roll = int.Parse(Regex.Match(f, @"\d").Value)
                    }).ToList();

                var idealTableLinesStart = backgroundLines.FindIndex(f => Regex.IsMatch(f, @"\|.*Ideal.*\|"));
                var idealTableDieType = Regex.Match(backgroundLines[idealTableLinesStart], @"\d+").Value;
                var idealTableLinesEnd = backgroundLines.FindIndex(idealTableLinesStart, f => Regex.IsMatch(f, @"\|\s*" + $"{idealTableDieType}" + @"\s*\|"));
                var idealTableLines = backgroundLines.Skip(idealTableLinesStart).Take(idealTableLinesEnd - idealTableLinesStart + 1)
                    .Where(f => Regex.IsMatch(f, @"^\|\s*\d"));
                background.IdealOptions = idealTableLines
                    .Select(f => new BackgroundOption
                    {
                        Name = f.Split('|')[1].Trim(),
                        Roll = int.Parse(Regex.Match(f, @"\d").Value),
                        Description = f.Split("**")[2].TrimStart('.', ',').TrimEnd('|').Trim()
                    }).ToList();

                var bondTableLinesStart = backgroundLines.FindIndex(f => Regex.IsMatch(f, @"\|.*Bond.*\|"));
                var bondTableDieType = Regex.Match(backgroundLines[bondTableLinesStart], @"\d+").Value;
                var bondTableLinesEnd = backgroundLines.FindIndex(bondTableLinesStart, f => Regex.IsMatch(f, @"\|\s*" + $"{bondTableDieType}" + @"\s*\|"));
                var bondTableLines = backgroundLines.Skip(bondTableLinesStart).Take(bondTableLinesEnd - bondTableLinesStart + 1)
                    .Where(f => Regex.IsMatch(f, @"^\|\s*\d"));
                background.BondOptions = bondTableLines
                    .Select(f => new BackgroundOption
                    {
                        Name = f.Split('|')[2].Trim(),
                        Roll = int.Parse(Regex.Match(f, @"\d").Value)
                    }).ToList();

                var flawTableLinesStart = backgroundLines.FindIndex(f => Regex.IsMatch(f, @"\|.*Flaw.*\|"));
                var flawTableDieType = Regex.Match(backgroundLines[flawTableLinesStart], @"\d+").Value;
                var flawTableLinesEnd = backgroundLines.FindIndex(flawTableLinesStart, f => Regex.IsMatch(f, @"\|\s*" + $"{flawTableDieType}" + @"\s*\|"));
                var flawTableLines = backgroundLines.Skip(flawTableLinesStart).Take(flawTableLinesEnd - flawTableLinesStart + 1)
                    .Where(f => Regex.IsMatch(f, @"^\|\s*\d"));
                background.FlawOptions = flawTableLines
                    .Select(f => new BackgroundOption
                    {
                        Name = f.Split('|')[2].Trim(),
                        Roll = int.Parse(Regex.Match(f, @"\d").Value)
                    }).ToList();

                return background;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing {name}", e);
            }

        }
    }
}
