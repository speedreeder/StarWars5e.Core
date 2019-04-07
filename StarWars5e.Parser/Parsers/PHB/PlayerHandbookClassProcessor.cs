using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StarWars5e.Models.Class;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Parsers.PHB
{
    public class PlayerHandbookClassProcessor : BaseProcessor<Class>
    {
        private static readonly List<string> BerserkerApproaches = new List<string>{ "Augmented Approach", "Marauder Approach", "Warchief Approach" };
        private static readonly List<string> ConsularTraditions = new List<string> { "Way of Balance", "Way of Lightning", "Way of the Sage" };
        private static readonly List<string> EngineerDisciplines = new List<string> { "Armormech Engineering", "Armstech Engineering", "Astrotech Engineering" };
        private static readonly List<string> FighterSpecialties = new List<string> { "Assault Specialist", "Shield Specialist", "Tactical Specialist" };
        private static readonly List<string> GuardianForms = new List<string> { "Form I: Shii-Cho", "Form II: Makashi", "Form III: Soresu" };
        private static readonly List<string> MonkOrders = new List<string> { "Echani Order", "Nightsister Order", "Ter�s K�si Order" };
        private static readonly List<string> OperativePractices = new List<string> { "Gunslinger Practice", "Lethality Practice", "Saboteur Practice" };
        private static readonly List<string> ScholarPursuits = new List<string> { "Physician Pursuit", "Politician Pursuit", "Tactician Pursuit" };
        private static readonly List<string> ScoutTechniques = new List<string> { "Deadeye Technique", "Hunter Technique", "Stalker Technique" };
        private static readonly List<string> SentinelPaths = new List<string> { "Path of Aggression", "Path of Focus", "Path of Shadows" };
        public override Task<List<Class>> FindBlocks(List<string> lines)
        {
            var classes = new List<Class>();
            lines = lines.CleanListOfStrings().ToList();
            var classTableStart = lines.FindIndex(f => f.Equals("##### Classes"));
            var classTableEnd = lines.FindIndex(classTableStart, f => f.Equals(string.Empty));
            var classTableLines = lines.Skip(classTableStart + 3).Take(classTableEnd - (classTableStart + 3)).ToList();

            var classNames = classTableLines.Select(s => s.Split('|')[1].Trim()).ToList();
            classNames.Sort();

            foreach (var classTableLine in classTableLines)
            {
                var classTableLineSplit = classTableLine.Split('|');

                var starWarsClass = new Class();
                starWarsClass.Name = classTableLineSplit[1].Trim();
                starWarsClass.Summary = classTableLineSplit[2].Trim();
                starWarsClass.HitDiceDieTypeEnum = (DiceType)int.Parse(Regex.Match(classTableLineSplit[3], @"\d+").Value);
                starWarsClass.PrimaryAbility = classTableLineSplit[4].Trim();
                starWarsClass.SavingThrows = classTableLineSplit[5].Split('&').Select(s => s.Trim()).ToList();

                var classLinesStart = lines.FindIndex(f => f.StartsWith($"## {starWarsClass.Name}"));
                var nextClassName = classNames.ElementAtOrDefault(classNames.IndexOf(starWarsClass.Name) + 1);
                var classLines = lines.Skip(classLinesStart).ToList();
                if (nextClassName != null)
                {
                    var classLinesEnd = lines.FindIndex(classLinesStart, f => f.Equals($"## {nextClassName}"));
                    classLines = lines.Skip(classLinesStart).Take(classLinesEnd - classLinesStart).ToList();
                }
                
                classes.Add(ParseClass(classLines, starWarsClass, ContentType.Base));
            }

            return Task.FromResult(classes);
        }

        public static Class ParseClass(List<string> classLines, Class starWarsClass, ContentType contentType)
        {
            try
            {
                starWarsClass.ContentTypeEnum = contentType;
                starWarsClass.PartitionKey = contentType.ToString();
                starWarsClass.RowKey = starWarsClass.Name;

                var flavorTextEnd = classLines.FindIndex(f =>
                    Regex.IsMatch(f, $@"\#\#\#\s*Creating.*{starWarsClass.Name}"));
                starWarsClass.FlavorText = string.Join("\r\n", classLines.Skip(1).Take(flavorTextEnd));

                var buildStart = classLines.FindIndex(flavorTextEnd, f => Regex.IsMatch(f, @"[#]+\s*Quick\s*Build"));
                starWarsClass.CreatingText =
                    string.Join("\r\n", classLines.Skip(flavorTextEnd + 1).Take(buildStart - (flavorTextEnd + 1)));

                var classTableStart = classLines.FindIndex(f => f.Equals($"##### The {starWarsClass.Name}"));
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

                starWarsClass.HitPointsAtFirstLevel = classLines.Find(f => f.Contains("**Hit Points at 1st Level:**"))
                    .Split("**").ElementAtOrDefault(2)?.Trim();
                starWarsClass.HitPointsAtHigherLevels = classLines.Find(f => f.Contains("**Hit Points at Higher Level"))
                    .Split("**").ElementAtOrDefault(2)?.Trim();
                starWarsClass.ArmorProficiencies = classLines.Find(f => f.Contains("**Armor:**"))
                    .Split("**").ElementAtOrDefault(2)?.Split(',').Select(s => s.Trim()).ToList();
                starWarsClass.WeaponProficiencies = classLines.Find(f => f.Contains("**Weapons:**"))
                    .Split("**").ElementAtOrDefault(2)?.Split(',').Select(s => s.Trim()).ToList();
                starWarsClass.ToolProficiencies = classLines.Find(f => f.Contains("**Tools:**"))
                    .Split("**").ElementAtOrDefault(2)?.Split(',').Select(s => s.Trim()).ToList();
                starWarsClass.SkillChoices = classLines.Find(f => f.Contains("**Skills:**"))
                    .Split("**").ElementAtOrDefault(2)?.Trim();

                var equipmentLinesStart = classLines.FindIndex(f => f.Equals("#### Equipment"));
                var equipmentLinesEnd = classLines.FindIndex(equipmentLinesStart + 1, f => f.StartsWith("#"));
                starWarsClass.EquipmentLines = classLines.Skip(equipmentLinesStart).Take(equipmentLinesEnd - equipmentLinesStart)
                    .Where(s => s.StartsWith('-')).ToList();

                var variantWealthLine =
                    classLines.FindIndex(f => Regex.IsMatch(f, $@"^\|\s*{starWarsClass.Name}\s*\|\s*.*\s*\|\s*$"));
                starWarsClass.StartingWealthVariant = classLines.ElementAtOrDefault(variantWealthLine)?.Split('|')
                    .ElementAtOrDefault(2)?.Trim();

                var classFeatureText = string.Join("\r\n", classLines.Skip(variantWealthLine + 1).ToList());
                starWarsClass.ClassFeatureText = classFeatureText;
                if (classFeatureText.Length > 30000)
                {
                    starWarsClass.ClassFeatureText = new string(classFeatureText.Take(30000).ToArray());
                    starWarsClass.ClassFeatureText2 = new string(classFeatureText.Skip(30000).ToArray());
                }

                var archetypeStartLine = classLines.FindIndex(variantWealthLine, f => f.StartsWith("## "));
                var archetypeFlavorEnd = classLines.FindIndex(archetypeStartLine + 1, f => f.StartsWith("#"));
                starWarsClass.ArchetypeFlavorName = classLines.ElementAtOrDefault(archetypeStartLine)?.Split("## ").ElementAtOrDefault(1);
                starWarsClass.ArchetypeFlavorText = string.Join("\r\n",
                    classLines.Skip(archetypeStartLine + 1).Take(archetypeFlavorEnd - (archetypeStartLine + 1))
                        .Select(s => s.RemoveHtmlWhitespace()).CleanListOfStrings());

                var archetypesLines = classLines.Skip(archetypeFlavorEnd).ToList();
                var archetypes = ParseArchetypes(archetypesLines, starWarsClass.Name, contentType);
                starWarsClass.Archetypes = archetypes;

                return starWarsClass;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing {starWarsClass.Name}", e);
            }
        }

        private static List<Archetype> ParseArchetypes(List<string> classArchetypeLines, string className,
            ContentType contentType)
        {
            var archetypes = new List<Archetype>();
            switch (className)
            {
                case "Berserker":
                    foreach (var archetypeName in BerserkerApproaches)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Consular":
                    foreach (var archetypeName in ConsularTraditions)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Engineer":
                    foreach (var archetypeName in EngineerDisciplines)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Fighter":
                    foreach (var archetypeName in FighterSpecialties)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Guardian":
                    foreach (var archetypeName in GuardianForms)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Monk":
                    foreach (var archetypeName in MonkOrders)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Operative":
                    foreach (var archetypeName in OperativePractices)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Scholar":
                    foreach (var archetypeName in ScholarPursuits)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Scout":
                    foreach (var archetypeName in ScoutTechniques)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Sentinel":
                    foreach (var archetypeName in SentinelPaths)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
            }

            return archetypes;
        }

        private static List<string> GetArchetypeLines(List<string> lines, string archetypeName)
        {
            var archetypeStartLine = lines.FindIndex(f => f.Contains($"## {archetypeName}"));
            var archetypeEndLine = lines.FindIndex(archetypeStartLine + 1,
                f => BerserkerApproaches.Contains($"## {f}"));
            var archetypesLines = lines.Skip(archetypeStartLine).ToList();
            if (archetypeEndLine != -1)
            {
                archetypesLines = lines.Skip(archetypeStartLine)
                    .Take(archetypeEndLine - archetypeStartLine).ToList();
            }

            return archetypesLines;
        }

        public static Archetype ParseArchetype(List<string> archetypeLines, string className, ContentType contentType)
        {
            var name = archetypeLines[0].Split("##").ElementAtOrDefault(1)?.Trim();
            try
            {
                var archetype = new Archetype
                {
                    ContentTypeEnum = contentType,
                    PartitionKey = contentType.ToString(),
                    RowKey = name,
                    Name = name,
                    ClassName = className
                };

                var archetypeTableStart = archetypeLines.FindIndex(f => f.StartsWith("|"));
                if (archetypeTableStart != -1)
                {
                    var archetypeTableEnd = archetypeLines.FindIndex(archetypeTableStart, f => f.Equals(string.Empty));
                    var archetypesTableLines = archetypeLines.Skip(archetypeTableStart).Take(archetypeTableEnd - archetypeTableStart).ToList();

                    var archetypeTableHeaders = archetypesTableLines[0].Split('|').Select(s => s.RemoveHtmlWhitespace().Trim()).ToList();

                    archetype.LeveledTable = new Dictionary<int, Dictionary<string, string>>();
                    foreach (var classTableLine in archetypesTableLines.Skip(2))
                    {
                        archetype.LeveledTableHeadersJson =
                            JsonConvert.SerializeObject(archetypeTableHeaders.Where(s => !s.Equals(string.Empty)).ToList());
                        var levelChange = new Dictionary<string, string>();
                        var archetypeTableLineSplit = classTableLine.Split('|').Select(s => s.RemoveHtmlWhitespace().Trim()).ToList();
                        var level = int.Parse(Regex.Match(archetypeTableLineSplit[1].Trim(), @"\d+").Value);
                        for (var i = 2; i < archetypeTableHeaders.Count; i++)
                        {
                            if (archetypeTableHeaders[i].Equals(string.Empty)) continue;
                            levelChange.Add(archetypeTableHeaders[i], archetypeTableLineSplit[i]);
                        }

                        archetype.LeveledTable.Add(level, levelChange);
                    }

                    var archetypeText = string.Join("\r\n",archetypeLines.Skip(1).Take(archetypeTableStart)
                        .Concat(archetypeLines.Skip(archetypeTableEnd)).ToList().CleanListOfStrings());

                    archetype.Text = archetypeText;
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
                    if (archetypeText.Length > 30000)
                    {
                        archetype.Text = new string(archetypeText.Take(30000).ToArray());
                        archetype.Text2 = new string(archetypeText.Skip(30000).ToArray());
                    }
                }

                return archetype;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing {name}", e);
            }
        }
    }
}
