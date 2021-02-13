using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StarWars5e.Models;
using StarWars5e.Models.Class;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Lookup;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Processors.PHB
{
    public class PlayerHandbookClassProcessor : BaseProcessor<Class>
    {
        private readonly List<ClassImageLU> _classImageLus;
        private readonly List<MulticlassProficiencyLU> _multiclassProficiencyLus;
        private readonly List<CasterRatioLU> _casterRatioLus;

        public PlayerHandbookClassProcessor(List<ClassImageLU> classImageLus = null,
            List<CasterRatioLU> casterRatioLus = null, List<MulticlassProficiencyLU> multiclassProficiencyLus = null)
        {
            _classImageLus = classImageLus;
            _multiclassProficiencyLus = multiclassProficiencyLus;
            _casterRatioLus = casterRatioLus;
        }

        public override Task<List<Class>> FindBlocks(List<string> lines)
        {
            var classes = new List<Class>();
            lines = lines.CleanListOfStrings().ToList();
            var classTableStart = lines.FindIndex(f => f.Equals(Localization.PHBClassesTableStart));
            var classTableEnd = lines.FindIndex(classTableStart, f => f.Equals(string.Empty));
            var classTableLines = lines.Skip(classTableStart + 3).Take(classTableEnd - (classTableStart + 3)).ToList();

            var classNames = classTableLines.Select(s => s.Split('|')[1].Trim()).ToList();
            classNames.Sort();

            foreach (var classTableLine in classTableLines)
            {
                var classTableLineSplit = classTableLine.Split('|');

                var starWarsClass = new Class
                {
                    Name = classTableLineSplit[1].Trim(),
                    Summary = classTableLineSplit[2].Trim(),
                    HitDiceDieTypeEnum = (DiceType)int.Parse(Regex.Match(classTableLineSplit[3], @"\d+").Value),
                    PrimaryAbility = classTableLineSplit[4].Trim(),
                    SavingThrows = classTableLineSplit[5].Split('&').Select(s => s.Trim()).ToList()
                };

                var classLinesStart = lines.FindIndex(f => f.StartsWith($"## {starWarsClass.Name}"));
                var nextClassName = classNames.ElementAtOrDefault(classNames.IndexOf(starWarsClass.Name) + 1);
                var classLines = lines.Skip(classLinesStart).ToList();
                if (nextClassName != null)
                {
                    var classLinesEnd = lines.FindIndex(classLinesStart, f => f.Equals($"## {nextClassName}"));
                    classLines = lines.Skip(classLinesStart).Take(classLinesEnd - classLinesStart).ToList();
                }

                if (_classImageLus != null)
                {
                    foreach (var classImageLu in _classImageLus.Where(c => c.Class == starWarsClass.Name))
                    {
                        starWarsClass.ImageUrls.Add(classImageLu.URL);
                    }
                }

                if (_multiclassProficiencyLus != null)
                {
                    foreach (var multiclassProficiencyLu in _multiclassProficiencyLus.Where(m => m.Class == starWarsClass.Name))
                    {
                        starWarsClass.MultiClassProficiencies.Add(multiclassProficiencyLu.Proficiency);
                    }
                }

                var casterRatio = _casterRatioLus?.SingleOrDefault(c => c.Name == starWarsClass.Name);
                if (casterRatio != null)
                {
                    starWarsClass.CasterRatio = casterRatio.Ratio;
                    starWarsClass.CasterTypeEnum = casterRatio.CasterTypeEnum;
                }

                classes.Add(ParseClass(classLines.CleanListOfStrings().ToList(), starWarsClass, ContentType.Core));
            }

            return Task.FromResult(classes);
        }

        private Class ParseClass(List<string> classLines, Class starWarsClass, ContentType contentType)
        {
            try
            {
                starWarsClass.ContentTypeEnum = contentType;
                starWarsClass.PartitionKey = contentType.ToString();
                starWarsClass.RowKey = starWarsClass.Name;

                var flavorTextEnd = classLines.FindIndex(f =>
                    Regex.IsMatch(f, $@"\#\#\#\s*{Localization.Creating}.*{starWarsClass.Name}"));
                starWarsClass.FlavorText = string.Join("\r\n", classLines.Skip(1).Take(flavorTextEnd - 1));

                var buildStart = classLines.FindIndex(flavorTextEnd, f => Regex.IsMatch(f, Localization.PHBClassBuildStartPattern));
                starWarsClass.CreatingText =
                    string.Join("\r\n", classLines.Skip(flavorTextEnd + 1).Take(buildStart - (flavorTextEnd + 1)));

                var classTableStart = classLines.FindIndex(f => f.Equals($"##### {Localization.The} {starWarsClass.Name}"));
                var classTableEnd = classLines.FindIndex(classTableStart, f => f.Equals(string.Empty));
                var classTableLines = classLines.Skip(classTableStart).Take(classTableEnd - classTableStart).ToList();

                var classTableHeaders = classTableLines[1].Split('|').Select(s => s.RemoveHtmlWhitespace().Trim()).ToList();

                starWarsClass.QuickBuildText =
                    string.Join("\r\n", classLines.Skip(buildStart + 1).Take(classTableStart - (buildStart + 1)));

                starWarsClass.LevelChanges = new Dictionary<int, Dictionary<string, string>>();
                foreach (var classTableLine in classTableLines.Skip(3))
                {
                    starWarsClass.LevelChangeHeadersJson =
                        JsonConvert.SerializeObject(classTableHeaders.Where(s => !s.Equals(string.Empty)).ToList());

                    var levelChange = new Dictionary<string, string>();
                    var classTableLineSplit = classTableLine.Split('|').Select(s => s.RemoveHtmlWhitespace().Trim()).ToList();
                    var level = int.Parse(Regex.Match(classTableLineSplit[1].Trim(), @"\d+").Value);

                    for (var i = 0; i < classTableHeaders.Count; i++)
                    {
                        if(classTableHeaders[i].Equals(string.Empty)) continue;
                        levelChange.Add(classTableHeaders[i], classTableLineSplit[i]);
                    }

                    starWarsClass.LevelChanges.Add(level, levelChange);
                }

                starWarsClass.HitPointsAtFirstLevel = classLines.Find(f => f.Contains(Localization.PHBClassHitPointsAtFirstLevel))
                    .Split("**").ElementAtOrDefault(2)?.Trim();
                starWarsClass.HitPointsAtHigherLevels = classLines.Find(f => f.Contains(Localization.PHBClassHitPointsAtHigherLevel))
                    .Split("**").ElementAtOrDefault(2)?.Trim();

                if (starWarsClass.HitPointsAtFirstLevel != null)
                {
                    starWarsClass.HitPointsAtFirstLevelNumber =
                        int.Parse(Regex.Match(starWarsClass.HitPointsAtFirstLevel, @"\d+").Value);
                }

                if (starWarsClass.HitPointsAtHigherLevels != null)
                {
                    starWarsClass.HitPointsAtHigherLevelsNumber =
                        int.Parse(Regex.Match(starWarsClass.HitPointsAtHigherLevels.Split('(')[1], @"\d+").Value);
                }

                starWarsClass.ArmorProficiencies = classLines.Find(f => f.Contains($"**{Localization.Armor}:**"))
                    .Split("**").ElementAtOrDefault(2)?.Split(',').Select(s => s.Trim()).ToList();


                starWarsClass.WeaponProficiencies = classLines.Find(f => f.Contains($"**{Localization.Weapons}:**"))
                    .Split("**").ElementAtOrDefault(2)?.Split(',').Select(s => s.Trim()).ToList();


                starWarsClass.ToolProficiencies = classLines.Find(f => f.Contains($"**{Localization.Tools}:**"))
                    .Split("**").ElementAtOrDefault(2)?.Split(new [] {",", Localization.and}, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();

                if (starWarsClass.ToolProficiencies != null)
                {
                    starWarsClass.ToolProficienciesList = starWarsClass.ToolProficiencies.Select(t =>
                    {
                        t = t.Replace($"{Localization.PHBClassToolProficiencyChoice} ", "");
                        return t.RemoveWords(new [] {"or"});
                    }).ToList();
                }

                starWarsClass.SkillChoices = classLines.Find(f => f.Contains($"**{Localization.Skills}:**"))
                    .Split("**").ElementAtOrDefault(2)?.Trim();

                if (starWarsClass.SkillChoices != null)
                {
                    if (starWarsClass.SkillChoices.Contains(Localization.ChooseAny, StringComparison.InvariantCultureIgnoreCase))
                    {
                        starWarsClass.SkillChoicesList = new List<string>
                        {
                            Localization.Any
                        };
                    }
                    else
                    {
                        starWarsClass.SkillChoicesList = starWarsClass.SkillChoices.Split($"{Localization.from} ")[1]
                            .Split(",")
                            .Select(c =>
                            {
                                c = c.RemoveWords(new[] { Localization.and });
                                return c.Trim();
                            })
                            .ToList();
                    }

                    starWarsClass.NumSkillChoices = Regex.Match(starWarsClass.SkillChoices,
                            @$"{Localization.one}|{Localization.two}|{Localization.three}|{Localization.four}|{Localization.five}|{Localization.six}|{Localization.seven}|{Localization.eight}|{Localization.nine}")
                        .Value.ToInteger();
                }

                var equipmentLinesStart = classLines.FindIndex(f => f.Equals($"#### {Localization.Equipment}"));
                var equipmentLinesEnd = classLines.FindIndex(equipmentLinesStart + 1, f => f.StartsWith("#"));
                starWarsClass.EquipmentLines = classLines.Skip(equipmentLinesStart).Take(equipmentLinesEnd - equipmentLinesStart)
                    .Where(s => s.StartsWith('-')).ToList();

                var variantWealthLine =
                    classLines.FindIndex(f => Regex.IsMatch(f, $@"^\|\s*{starWarsClass.Name}\s*\|\s*.*\s*\|\s*$"));
                starWarsClass.StartingWealthVariant = classLines.ElementAtOrDefault(variantWealthLine)?.Split('|')
                    .ElementAtOrDefault(2)?.Trim();

                var archetypeStartLine = classLines.FindIndex(variantWealthLine, f => f.StartsWith("## "));
                var classFeatureText = string.Join("\r\n", classLines.Skip(variantWealthLine + 1).Take(archetypeStartLine - (variantWealthLine + 1)).ToList());

                starWarsClass.Features = ParseFeatures(classLines.Skip(variantWealthLine + 1).Take(archetypeStartLine - (variantWealthLine + 1)).ToList(), starWarsClass.Name, FeatureSource.Class, ContentType.Core);
                starWarsClass.ClassFeatureText = classFeatureText;
                if (classFeatureText.Length > 30000)
                {
                    starWarsClass.ClassFeatureText = new string(classFeatureText.Take(30000).ToArray());
                    starWarsClass.ClassFeatureText2 = new string(classFeatureText.Skip(30000).ToArray());
                }

                var archetypeFlavorEnd = classLines.FindIndex(archetypeStartLine + 1, f => f.StartsWith("#"));
                starWarsClass.ArchetypeFlavorName = classLines.ElementAtOrDefault(archetypeStartLine)?.Split("## ").ElementAtOrDefault(1);
                starWarsClass.ArchetypeFlavorText = string.Join("\r\n",
                    classLines.Skip(archetypeStartLine + 1).Take(archetypeFlavorEnd - (archetypeStartLine + 1))
                        .Select(s => s.RemoveHtmlWhitespace()).CleanListOfStrings());

                var archetypesLines = classLines.Skip(archetypeFlavorEnd).ToList();
                var archetypes = ParseArchetypes(archetypesLines, starWarsClass, contentType);
                starWarsClass.Archetypes = archetypes;

                return starWarsClass;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing {starWarsClass.Name}", e);
            }
        }

        private List<Archetype> ParseArchetypes(List<string> classArchetypeLines, Class starWarsClass,
            ContentType contentType)
        {
            var archetypes = new List<Archetype>();

            var archetypeStartingLines = classArchetypeLines.FindAll(c => c.StartsWith("## "));

            if (starWarsClass.Name == Localization.Engineer)
            {
                archetypeStartingLines =
                    classArchetypeLines.FindAll(c => c.StartsWith("## ") && c.Contains(Localization.Engineering));
            }
            else if (starWarsClass.Name == Localization.Scholar)
            {
                archetypeStartingLines =
                    classArchetypeLines.FindAll(c => c.StartsWith("## ") && c.Contains(Localization.Pursuit));
            }

            for (var i = 0; i < archetypeStartingLines.Count; i++)
            {
                var nextArchetype = string.Empty;

                if (i != archetypeStartingLines.Count - 1)
                {
                    nextArchetype = archetypeStartingLines[i + 1];
                }
                var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeStartingLines[i], nextArchetype);

                archetypes.Add(ParseArchetype(archetypeLines, starWarsClass, contentType));
            }

            return archetypes;
        }

        private List<string> GetArchetypeLines(List<string> classArchetypeLines, string archetypeStartLine, string nextArchetype)
        {
            var archetypeStartLineIndex = classArchetypeLines.FindIndex(f => f.Contains(archetypeStartLine));

            var archetypeEndLine = -1;
            if (!string.IsNullOrWhiteSpace(nextArchetype))
            {
                archetypeEndLine = classArchetypeLines.FindIndex(archetypeStartLineIndex + 1, f => f == nextArchetype);
            }

            var archetypesLines = classArchetypeLines.Skip(archetypeStartLineIndex).ToList();
            if (archetypeEndLine != -1)
            {
                archetypesLines = classArchetypeLines.Skip(archetypeStartLineIndex)
                    .Take(archetypeEndLine - archetypeStartLineIndex).ToList();
            }

            return archetypesLines;
        }

        public Archetype ParseArchetype(List<string> archetypeLines, Class starWarsClass, ContentType contentType)
        {
            var name = archetypeLines[0].Split("##").ElementAtOrDefault(1)?.Trim();
            try
            {
                var archetype = new Archetype
                {
                    ContentTypeEnum = contentType,
                    PartitionKey = contentType.ToString(),
                    RowKey = name.Replace("/", string.Empty).Replace(@"\", string.Empty),
                    Name = name,
                    ClassName = starWarsClass.Name
                };

                var archetypeTableStart = archetypeLines.FindIndex(f =>
                    f.StartsWith("|") && f.Contains("level", StringComparison.InvariantCultureIgnoreCase));
                if (archetypeTableStart != -1)
                {
                    var archetypeTableEnd = archetypeLines.FindIndex(archetypeTableStart, f => f.Equals(string.Empty));
                    var archetypesTableLines = archetypeLines.Skip(archetypeTableStart).Take(archetypeTableEnd - archetypeTableStart).ToList();

                    var archetypeTableHeaders = archetypesTableLines[0].Split('|')
                        .Select(s => s.RemoveHtmlWhitespace().Trim()).CleanListOfStrings().ToList()
                        .Where(s => !s.Equals(string.Empty)).ToList();

                    if (archetypeTableHeaders[0].Contains("Level", StringComparison.InvariantCultureIgnoreCase))
                    {
                        archetype.LeveledTable = new Dictionary<int, List<KeyValuePair<string, string>>>();
                        foreach (var classTableLine in archetypesTableLines.Skip(2))
                        {
                            archetype.LeveledTableHeadersJson =
                                JsonConvert.SerializeObject(archetypeTableHeaders);
                            var levelChange = new List<KeyValuePair<string, string>>();
                            var archetypeTableLineSplit = classTableLine.Split('|').Select(s => s.RemoveHtmlWhitespace().Trim()).Where(s => !s.Equals(string.Empty)).ToList();
                            var level = int.Parse(Regex.Match(archetypeTableLineSplit[0].Trim(), @"\d+").Value);
                            for (var i = 1; i < archetypeTableHeaders.Count; i++)
                            {
                                if (archetypeTableHeaders[i].Equals(string.Empty)) continue;
                                levelChange.Add(new KeyValuePair<string, string>(archetypeTableHeaders[i], archetypeTableLineSplit[i]));
                            }

                            archetype.LeveledTable.Add(level, levelChange);
                        }

                        var archetypeText = string.Join("\r\n", archetypeLines.Skip(1).Take(archetypeTableStart - 1)
                            .Concat(archetypeLines.Skip(archetypeTableEnd)).ToList().CleanListOfStrings());

                        archetype.Text = archetypeText;
                        archetype.Features = ParseFeatures(archetypeLines.Skip(1).CleanListOfStrings().ToList(),
                            archetype.Name, FeatureSource.Archetype, contentType);
                        if (archetypeText.Length > 30000)
                        {
                            archetype.Text = new string(archetypeText.Skip(1).Take(30000).ToArray());
                            archetype.Text2 = new string(archetypeText.Skip(30000).ToArray());
                        }
                    }
                    else
                    {
                        var archetypeText = string.Join("\r\n", archetypeLines.Skip(1).CleanListOfStrings());
                        archetype.Text = archetypeText;
                        archetype.Features = ParseFeatures(archetypeLines.Skip(1).CleanListOfStrings().ToList(),
                            archetype.Name, FeatureSource.Archetype, contentType);
                        if (archetypeText.Length > 30000)
                        {
                            archetype.Text = new string(archetypeText.Take(30000).ToArray());
                            archetype.Text2 = new string(archetypeText.Skip(30000).ToArray());
                        }
                    }
                }
                else
                {
                    var archetypeText = string.Join("\r\n", archetypeLines.Skip(1).CleanListOfStrings());
                    archetype.Text = archetypeText;
                    archetype.Features = ParseFeatures(archetypeLines.Skip(1).CleanListOfStrings().ToList(),
                        archetype.Name, FeatureSource.Archetype, contentType);
                    if (archetypeText.Length > 30000)
                    {
                        archetype.Text = new string(archetypeText.Take(30000).ToArray());
                        archetype.Text2 = new string(archetypeText.Skip(30000).ToArray());
                    }
                }

                archetype.CasterRatio = 0;
                archetype.CasterTypeEnum = PowerType.None;
                archetype.ClassCasterTypeEnum = starWarsClass.CasterTypeEnum;

                var casterRatio = _casterRatioLus.SingleOrDefault(c => c.Name == name);
                if (casterRatio != null)
                {
                    archetype.CasterRatio = casterRatio.Ratio;
                    archetype.CasterTypeEnum = casterRatio.CasterTypeEnum;
                }

                return archetype;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing {name}", e);
            }
        }

        public List<Feature> ParseFeatures(List<string> featureTextLines, string sourceName, FeatureSource featureSource, ContentType contentType)
        {
            var features = new List<Feature>();

            var featureNameLineIndexes = featureTextLines.FindAllIndexOf(f => f.StartsWith("### "));

            for (var i = 0; i < featureNameLineIndexes.Count - 1; i++)
            {
                var currentFeatureNameLineIndex = featureNameLineIndexes[i];
                var nextFeatureNameLineIndex = featureNameLineIndexes.ElementAtOrDefault(i + 1);
                var featureLines = nextFeatureNameLineIndex == 0
                    ? featureTextLines.Skip(currentFeatureNameLineIndex).RemoveEmptyLines().ToList()
                    : featureTextLines.Skip(currentFeatureNameLineIndex)
                        .Take(nextFeatureNameLineIndex - currentFeatureNameLineIndex).RemoveEmptyLines().ToList();

                var name = featureLines[0].Split("# ")[1].Trim();
                

                var levels = GetFeatureLevels(featureLines.Skip(1).CleanListOfStrings().ToList());

                foreach (var level in levels)
                {
                    var feature = new Feature
                    {
                        Name = name,
                        SourceEnum = featureSource,
                        Text = string.Join("\r\n", featureLines.Skip(1).CleanListOfStrings()),
                        PartitionKey = contentType.ToString(),
                        SourceName = sourceName,
                        Level = level,
                        RowKey = $"{featureSource}-{sourceName}-{name}-{level}".Replace("/", string.Empty)
                            .Replace(@"\", string.Empty)
                    };

                    features.Add(feature);
                }
            }

            return features;
        }

        private List<int> GetFeatureLevels(List<string> featureTextLines)
        {
            var featureLevels = new List<Tuple<string, int>>
            {
                Localization.FirstNum, Localization.SecondNum, Localization.ThirdNum,
                Localization.FourthNum, Localization.FifthNum, Localization.SixthNum,
                Localization.SeventhNum, Localization.EighthNum, Localization.NinthNum,
                Localization.TenthNum, Localization.EleventhNum, Localization.TwelfthNum,
                Localization.ThirteenthNum, Localization.FourteenthNum, Localization.FifteenthNum,
                Localization.SixteenthNum, Localization.SeventeenthNum, Localization.EighteenthNum,
                Localization.NineteenthNum, Localization.TwentiethNum
            };

            var levels = new List<int>();

            var levelsTextSplit = featureTextLines[0].Split(',');

            featureLevels.Reverse();

            foreach (var levelSplit in levelsTextSplit)
            {
                var levelLine = featureLevels.FirstOrDefault(f => levelSplit.Contains(f.Item1));

                if (levelLine != null)
                {
                    levels.Add(levelLine.Item2);
                }
            }

            return levels;

            //if (featureTextLines.Any())
            //{
            //    List<Tuple<string, int>> exists;
            //    var i = 0;
            //    do
            //    {
            //        exists = featureLevels.Where(f => featureTextLines[i].Contains(f.Item1)).ToList();
            //        i++;
            //    } while (i <= featureTextLines.Count - 1 && !exists.Any());

            //    if (exists.Any())
            //    {
            //        return exists[0].Item2;
            //    }
            //}
        }
    }
}
