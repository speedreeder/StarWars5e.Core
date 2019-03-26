using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Starship;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Parsers
{
    public class StarshipChapterRulesProcessor : StarshipBaseProcessor<StarshipChapterRules>
    {
        public override Task<List<StarshipChapterRules>> FindBlocks(List<string> lines)
        {
            var chapters = new List<StarshipChapterRules>();

            var chapter0StartIndex = lines.FindIndex(f => f == "# Introduction");
            var chapter1StartIndex = lines.FindIndex(f => f == "# Chapter 1: Step-By-Step Starships");
            var chapter2StartIndex = lines.FindIndex(f => f == "# Chapter 2: Deployments");
            var chapter3StartIndex = lines.FindIndex(f => f == "# Chapter 3: Starships");
            var chapter4StartIndex = lines.FindIndex(f => f == "# Chapter 4: Modifications");
            var chapter5StartIndex = lines.FindIndex(f => f == "# Chapter 5: Equipment");
            var chapter6StartIndex = lines.FindIndex(f => f == "# Chapter 6: Customization Options");
            var chapter7StartIndex = lines.FindIndex(f => f == "# Chapter 7: Using Ability Scores");
            var chapter8StartIndex = lines.FindIndex(f => f == "# Chapter 8: Adventuring");
            var chapter9StartIndex = lines.FindIndex(f => f == "# Chapter 9: Combat");
            var chapter10StartIndex = lines.FindIndex(f => f == "# Chapter 10: Generating Encounters");
            var appendixAStartIndex = lines.FindIndex(f => f == "# Appendix A: Conditions");

            chapters.Add(CreateStarshipChapterRules(
                lines.Skip(chapter0StartIndex).Take(chapter1StartIndex - chapter0StartIndex).CleanListOfStrings(), 0,
                "Introduction"));
            chapters.Add(CreateStarshipChapterRules(
                lines.Skip(chapter1StartIndex).Take(chapter2StartIndex - chapter1StartIndex).CleanListOfStrings(), 1,
                "Step-By-Step Starships"));

            var chapter2EndIndex = lines.FindIndex(chapter2StartIndex, f => f == "## Coordinator");
            chapters.Add(CreateStarshipChapterRules(
                lines.Skip(chapter2StartIndex).Take(chapter2EndIndex - chapter2StartIndex).CleanListOfStrings(), 2,
                "Deployments"));

            var chapter3EndIndex = lines.FindIndex(chapter3StartIndex, f => f == "## Tiny Ships");
            chapters.Add(CreateStarshipChapterRules(
                lines.Skip(chapter3StartIndex).Take(chapter3EndIndex - chapter3StartIndex).CleanListOfStrings(), 3,
                "Starships"));

            var chapter4EndIndex = lines.FindIndex(chapter4StartIndex, f => f == "## Engineering Systems");
            chapters.Add(CreateStarshipChapterRules(
                lines.Skip(chapter4StartIndex).Take(chapter4EndIndex - chapter4StartIndex).CleanListOfStrings(), 4,
                "Modifications"));

            var chapter5RulesLines =
                lines.Skip(chapter5StartIndex).Take(chapter6StartIndex - chapter5StartIndex).ToList();
            chapters.Add(CreateStarshipChapterRules(GetChapter5Rules(chapter5RulesLines).CleanListOfStrings(), 5, "Equipment"));

            var chapter6EndIndex = lines.FindIndex(lines.FindIndex(chapter6StartIndex, f => f == "## Ventures"), f => f.StartsWith("### "));
            chapters.Add(CreateStarshipChapterRules(
                lines.Skip(chapter6StartIndex).Take(chapter6EndIndex - chapter6StartIndex).CleanListOfStrings(), 6,
                "Customization Options"));

            chapters.Add(CreateStarshipChapterRules(
                lines.Skip(chapter7StartIndex).Take(chapter8StartIndex - chapter7StartIndex).CleanListOfStrings(), 7,
                "Using Ability Scores"));
            chapters.Add(CreateStarshipChapterRules(
                lines.Skip(chapter8StartIndex).Take(chapter9StartIndex - chapter8StartIndex).CleanListOfStrings(), 8,
                "Adventuring"));
            chapters.Add(CreateStarshipChapterRules(
                lines.Skip(chapter9StartIndex).Take(appendixAStartIndex - chapter9StartIndex).CleanListOfStrings(), 9,
                "Combat"));
            chapters.Add(CreateStarshipChapterRules(
                lines.Skip(appendixAStartIndex).Take(lines.Count - appendixAStartIndex).CleanListOfStrings(), 11,
                "Appendix A: Conditions"));

            return Task.FromResult(chapters);
        }

        private static StarshipChapterRules CreateStarshipChapterRules(IEnumerable<string> chapterLines, int chapterNumber, string chapterName)
        {
            var chapter = new StarshipChapterRules
            {
                PartitionKey = ContentType.Base.ToString(),
                RowKey = chapterNumber.ToString(),
                ChapterNumber = chapterNumber,
                ChapterName = chapterName,
                ContentMarkdown = string.Join("\r\n", chapterLines)
            };
            return chapter;
        }

        private static IEnumerable<string> GetChapter5Rules(List<string> chapter5Lines)
        {
            var chapter5RulesLines = new List<string>();

            var armorTableStart = chapter5Lines.FindIndex(f => f.StartsWith("|Name|Cost|Armor Class"));
            chapter5RulesLines.AddRange(chapter5Lines.Take(armorTableStart));

            var weaponRulesStart = chapter5Lines.FindIndex(f => f == "## Weapons");
            var weaponRulesStop = chapter5Lines.FindIndex(f => f == "##### Ship Weapons (Small)");

            chapter5RulesLines.AddRange(chapter5Lines.Skip(weaponRulesStart).Take(weaponRulesStop - weaponRulesStart));

            var ammunitionRulesStart = chapter5Lines.FindIndex(f => f.StartsWith("## Ammunition"));
            var ammunitionRulesStop = chapter5Lines.FindIndex(f => f == "##### Ammunition (Small)");

            chapter5RulesLines.AddRange(chapter5Lines.Skip(ammunitionRulesStart).Take(ammunitionRulesStop - ammunitionRulesStart));

            var hyperdriveRulesStart = chapter5Lines.FindIndex(f => f == "## Hyperdrive");
            var hyperdriveRulesStop = chapter5Lines.FindIndex(f => f == "##### Hyperdrives");

            chapter5RulesLines.AddRange(chapter5Lines.Skip(hyperdriveRulesStart).Take(hyperdriveRulesStop - hyperdriveRulesStart));

            var navcomputerRulesStart = chapter5Lines.FindIndex(f => f == "## Navcomputer");
            var navcomputerRulesStop = chapter5Lines.FindIndex(f => f == "##### Navcomputer");

            chapter5RulesLines.AddRange(chapter5Lines.Skip(navcomputerRulesStart).Take(navcomputerRulesStop - navcomputerRulesStart));

            var otherRulesStart = chapter5Lines.FindIndex(f => f == "## Docking");

            chapter5RulesLines.AddRange(chapter5Lines.Skip(otherRulesStart).Take(chapter5Lines.Count - otherRulesStart));

            return chapter5RulesLines;
        }
    }
}
