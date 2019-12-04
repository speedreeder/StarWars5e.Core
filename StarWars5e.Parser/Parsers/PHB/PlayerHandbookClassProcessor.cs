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

        private static readonly List<(string ArchetypeName, double CasterRatio, PowerType CasterType)>
            ArchetypeCasterMap = new List<(string ArchetypeName, double CasterRatio, PowerType CasterType)>
            {
                ("Marauder Approach", .3333333333333333, PowerType.Force),
                ("Adept Specialist", .3333333333333333, PowerType.Force),
                ("Aing-Tii Order", .3333333333333333, PowerType.Force),
                ("Beguiler Practice", .3333333333333333, PowerType.Force)
            };

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
                
                classes.Add(ParseClass(classLines, starWarsClass, ContentType.Core));
            }

            MapImageUrls(classes);
            MapCasterRatioAndType(classes);
            MapMultiClassProficiencies(classes);

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
                starWarsClass.FlavorText = string.Join("\r\n", classLines.Skip(1).Take(flavorTextEnd - 1));

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
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName, BerserkerApproaches);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Consular":
                    foreach (var archetypeName in ConsularTraditions)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName, ConsularTraditions);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Engineer":
                    foreach (var archetypeName in EngineerDisciplines)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName, EngineerDisciplines);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Fighter":
                    foreach (var archetypeName in FighterSpecialties)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName, FighterSpecialties);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Guardian":
                    foreach (var archetypeName in GuardianForms)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName, GuardianForms);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Monk":
                    foreach (var archetypeName in MonkOrders)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName, MonkOrders);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Operative":
                    foreach (var archetypeName in OperativePractices)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName, OperativePractices);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Scholar":
                    foreach (var archetypeName in ScholarPursuits)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName, ScholarPursuits);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Scout":
                    foreach (var archetypeName in ScoutTechniques)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName, ScoutTechniques);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
                case "Sentinel":
                    foreach (var archetypeName in SentinelPaths)
                    {
                        var archetypeLines = GetArchetypeLines(classArchetypeLines, archetypeName, SentinelPaths);

                        archetypes.Add(ParseArchetype(archetypeLines, className, contentType));
                    }
                    break;
            }

            return archetypes;
        }

        private static List<string> GetArchetypeLines(List<string> lines, string archetypeName, IReadOnlyCollection<string> archetypeNames)
        {
            var archetypeStartLine = lines.FindIndex(f => f.Contains($"## {archetypeName}"));
            var archetypeEndLine = lines.FindIndex(archetypeStartLine + 1,
                f => archetypeNames.SingleOrDefault(a => f.StartsWith("## ") && f.Contains(a)) != null);
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

                var casterRatio = ArchetypeCasterMap.FirstOrDefault(c => c.ArchetypeName == name);
                if (casterRatio != default((string ArchetypeName, double CasterRatio, PowerType casterType)))
                {
                    archetype.CasterRatio = casterRatio.CasterRatio;
                    archetype.CasterTypeEnum = casterRatio.CasterType;
                }

                return archetype;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing {name}", e);
            }
        }

        public static void MapImageUrls(IEnumerable<Class> classes)
        {
            foreach (var starWarsClass in classes)
            {
                switch (starWarsClass.Name)
                {
                    case "Berserker":
                        starWarsClass.ImageUrls.Add("https://starwars5e.blob.core.windows.net/site-images/classes/berserker_01.png");
                        break;
                    case "Consular":
                        starWarsClass.ImageUrls.Add("https://starwars5e.blob.core.windows.net/site-images/classes/consular_01.png");
                        starWarsClass.ImageUrls.Add("https://starwars5e.blob.core.windows.net/site-images/classes/consular_02.png");
                        break;
                    case "Engineer":
                        starWarsClass.ImageUrls.Add("https://starwars5e.blob.core.windows.net/site-images/classes/engineer_01.png");
                        break;
                    case "Fighter":
                        starWarsClass.ImageUrls.Add("https://starwars5e.blob.core.windows.net/site-images/classes/fighter_01.png");
                        break;
                    case "Guardian":
                        starWarsClass.ImageUrls.Add("https://starwars5e.blob.core.windows.net/site-images/classes/guardian_01.png");
                        starWarsClass.ImageUrls.Add("https://starwars5e.blob.core.windows.net/site-images/classes/guardian_02.png");
                        break;
                    case "Monk":
                        starWarsClass.ImageUrls.Add("https://starwars5e.blob.core.windows.net/site-images/classes/monk_01.png");
                        starWarsClass.ImageUrls.Add("https://starwars5e.blob.core.windows.net/site-images/classes/monk_02.png");
                        break;
                    case "Operative":
                        starWarsClass.ImageUrls.Add("https://starwars5e.blob.core.windows.net/site-images/classes/operative_01.png");
                        break;
                    case "Scholar":
                        starWarsClass.ImageUrls.Add("https://starwars5e.blob.core.windows.net/site-images/classes/scholar_01.png");
                        starWarsClass.ImageUrls.Add("https://starwars5e.blob.core.windows.net/site-images/classes/scholar_02.png");
                        break;
                    case "Scout":
                        starWarsClass.ImageUrls.Add("https://starwars5e.blob.core.windows.net/site-images/classes/scout_01.png");
                        starWarsClass.ImageUrls.Add("https://starwars5e.blob.core.windows.net/site-images/classes/scout_02.png");
                        break;
                    case "Sentinel":
                        starWarsClass.ImageUrls.Add("https://starwars5e.blob.core.windows.net/site-images/classes/sentinel_01.png");
                        starWarsClass.ImageUrls.Add("https://starwars5e.blob.core.windows.net/site-images/classes/sentinel_02.png");
                        break;
                }
            }
        }

        public static void MapMultiClassProficiencies(IEnumerable<Class> classes)
        {
            foreach (var starWarsClass in classes)
            {
                switch (starWarsClass.Name)
                {
                    case "Berserker":
                        starWarsClass.MultiClassProficiencies.Add("Light armor");
                        starWarsClass.MultiClassProficiencies.Add("all vibroweapons");
                        break;
                    case "Consular":
                        starWarsClass.MultiClassProficiencies.Add("Simple lightweapons");
                        break;
                    case "Engineer":
                        starWarsClass.MultiClassProficiencies.Add("Light armor");
                        break;
                    case "Fighter":
                        starWarsClass.MultiClassProficiencies.Add("Light armor");
                        starWarsClass.MultiClassProficiencies.Add("medium armor");
                        starWarsClass.MultiClassProficiencies.Add("all blasters");
                        starWarsClass.MultiClassProficiencies.Add("all vibroweapons");
                        break;
                    case "Guardian":
                        starWarsClass.MultiClassProficiencies.Add("Light armor");
                        starWarsClass.MultiClassProficiencies.Add("medium armor");
                        starWarsClass.MultiClassProficiencies.Add("all lightweapons");
                        starWarsClass.MultiClassProficiencies.Add("all vibroweapons");
                        break;
                    case "Monk":
                        starWarsClass.MultiClassProficiencies.Add("Simple vibroweapons");
                        starWarsClass.MultiClassProficiencies.Add("techblades");
                        break;
                    case "Operative":
                        starWarsClass.MultiClassProficiencies.Add("Light armor");
                        break;
                    case "Scholar":
                        starWarsClass.MultiClassProficiencies.Add("Light armor");
                        break;
                    case "Scout":
                        starWarsClass.MultiClassProficiencies.Add("Light armor");
                        starWarsClass.MultiClassProficiencies.Add("medium armor");
                        starWarsClass.MultiClassProficiencies.Add("all blasters");
                        starWarsClass.MultiClassProficiencies.Add("all vibroweapons");
                        break;
                    case "Sentinel":
                        starWarsClass.MultiClassProficiencies.Add("Light armor");
                        starWarsClass.MultiClassProficiencies.Add("simple lightweapons");
                        starWarsClass.MultiClassProficiencies.Add("simple vibroweapons");
                        break;
                }
            }
        }

        public static void MapCasterRatioAndType(IEnumerable<Class> classes)
        {
            foreach (var starWarsClass in classes)
            {
                switch (starWarsClass.Name)
                {
                    case "Berserker":
                        break;
                    case "Consular":
                        starWarsClass.CasterRatio = 1;
                        starWarsClass.CasterTypeEnum = PowerType.Force;
                        break;
                    case "Engineer":
                        starWarsClass.CasterRatio = 1;
                        starWarsClass.CasterTypeEnum = PowerType.Tech;
                        break;
                    case "Fighter":
                        break;
                    case "Guardian":
                        starWarsClass.CasterRatio = .5;
                        starWarsClass.CasterTypeEnum = PowerType.Force;
                        break;
                    case "Monk":
                        starWarsClass.CasterRatio = .3333333333333333;
                        starWarsClass.CasterTypeEnum = PowerType.Force;
                        break;
                    case "Operative":
                        break;
                    case "Scholar":
                        break;
                    case "Scout":
                        starWarsClass.CasterRatio = .5;
                        starWarsClass.CasterTypeEnum = PowerType.Tech;
                        break;
                    case "Sentinel":
                        starWarsClass.CasterRatio = .6666666666666666;
                        starWarsClass.CasterTypeEnum = PowerType.Force;
                        break;
                }
            }
        }
    }
}
