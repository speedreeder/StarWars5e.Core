using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Parsers.SOTG
{
    public class StarshipChapterRulesProcessor : BaseProcessor<ChapterRules>
    {
        public override Task<List<ChapterRules>> FindBlocks(List<string> lines)
        {
            var chapters = new List<ChapterRules>();

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
            var changelogStartIndex = lines.FindIndex(f => f == "## Changelog");

            var introLines = lines.Skip(chapter0StartIndex).Take(chapter1StartIndex - chapter0StartIndex)
                .CleanListOfStrings().ToList();
            introLines[2] = introLines[2].Insert(0, "T");
            chapters.Add(CreateStarshipChapterRules(
                introLines, 0,
                "Introduction"));

            var chapter1Lines = lines.Skip(chapter1StartIndex).Take(chapter2StartIndex - chapter1StartIndex)
                .CleanListOfStrings().ToList();
            chapter1Lines[2] = chapter1Lines[2].Insert(0, "Y");
            chapters.Add(CreateStarshipChapterRules(
                chapter1Lines, 1,
                "Step-By-Step Starships"));

            var chapter2EndIndex = lines.FindIndex(chapter2StartIndex, f => f == "##### Deployments");
            var chapter2Lines = lines.Skip(chapter2StartIndex).Take(chapter2EndIndex - chapter2StartIndex)
                .CleanListOfStrings().ToList();
            chapter2Lines[2] = chapter2Lines[2].Insert(0, "A");
            chapters.Add(CreateStarshipChapterRules(
                chapter2Lines, 2,
                "Deployments"));

            var chapter3EndIndex = lines.FindIndex(chapter3StartIndex, f => f == "## Tiny Ships");
            var chapter3Lines = lines.Skip(chapter3StartIndex).Take(chapter3EndIndex - chapter3StartIndex)
                .CleanListOfStrings().ToList();
            var variantStart = lines.FindIndex(chapter3StartIndex, f => f == "## Variant: Space Stations");
            var variantLines = lines.Skip(variantStart).Take(chapter4StartIndex - variantStart).CleanListOfStrings().ToList();
            chapter3Lines.AddRange(variantLines);
            chapter3Lines[2] = chapter3Lines[2].Insert(0, "C");
            chapters.Add(CreateStarshipChapterRules(
                chapter3Lines, 3,
                "Starships"));

            var chapter4EndIndex = lines.FindIndex(chapter4StartIndex, f => f == "## Engineering Systems");
            var chapter4Lines = lines.Skip(chapter4StartIndex).Take(chapter4EndIndex - chapter4StartIndex)
                .CleanListOfStrings().ToList();
            chapter4Lines[2] = chapter4Lines[2].Insert(0, "A");
            chapters.Add(CreateStarshipChapterRules(
                chapter4Lines, 4,
                "Modifications"));

            var chapter5Lines = lines.Skip(chapter5StartIndex).Take(chapter6StartIndex - chapter5StartIndex)
                .CleanListOfStrings().ToList();
            chapter5Lines[2] = chapter5Lines[2].Insert(0, "T");
            chapters.Add(CreateStarshipChapterRules(
                chapter5Lines, 6,
                "Equipment"));

            var chapter6EndIndex = lines.FindIndex(lines.FindIndex(chapter6StartIndex, f => f == "## Ventures"), f => f.StartsWith("### "));
            var chapter6Lines = lines.Skip(chapter6StartIndex).Take(chapter6EndIndex - chapter6StartIndex)
                .CleanListOfStrings().ToList();
            chapter6Lines[2] = chapter6Lines[2].Insert(0, "T");
            chapters.Add(CreateStarshipChapterRules(
                chapter6Lines, 6,
                "Customization Options"));

            var chapter7Lines = lines.Skip(chapter7StartIndex).Take(chapter8StartIndex - chapter7StartIndex)
                .CleanListOfStrings().ToList();
            chapter7Lines[2] = chapter7Lines[2].Insert(0, "S");
            chapters.Add(CreateStarshipChapterRules(
                chapter7Lines, 7,
                "Using Ability Scores"));

            var chapter8Lines = lines.Skip(chapter8StartIndex).Take(chapter9StartIndex - chapter8StartIndex)
                .CleanListOfStrings().ToList();
            chapter8Lines[2] = chapter8Lines[2].Insert(0, "D");
            chapters.Add(CreateStarshipChapterRules(
                chapter8Lines, 8,
                "Adventuring"));

            var chapter9Lines = lines.Skip(chapter9StartIndex).Take(chapter10StartIndex - chapter9StartIndex).CleanListOfStrings().ToList();
            chapter9Lines[2] = chapter9Lines[2].Insert(0, "A");
            chapters.Add(CreateStarshipChapterRules(
                chapter9Lines, 9,
                "Combat"));

            var chapter10Lines = lines.Skip(chapter10StartIndex).Take(appendixAStartIndex - chapter10StartIndex).CleanListOfStrings().ToList();
            chapter10Lines[2] = chapter10Lines[2].Insert(0, "C");
            chapters.Add(CreateStarshipChapterRules(
                chapter10Lines, 10,
                "Generating Encounters"));

            var appendixALines = lines.Skip(appendixAStartIndex).Take(changelogStartIndex - appendixAStartIndex).CleanListOfStrings().ToList();
            appendixALines[2] = appendixALines[2].Insert(0, "C");
            chapters.Add(CreateStarshipChapterRules(appendixALines
                , 11,
                "Appendix A: Conditions"));

            var changelogLines = lines.Skip(changelogStartIndex).CleanListOfStrings().ToList();
            chapters.Add(CreateStarshipChapterRules(changelogLines, 99, "Changelog"));

            return Task.FromResult(chapters);
        }

        private static ChapterRules CreateStarshipChapterRules(IEnumerable<string> chapterLines, int chapterNumber,
            string chapterName, string joinString = "\r\n", bool shouldCleanMarkdown = false)
        {
            var chapter = new ChapterRules
            {
                PartitionKey = ContentType.Core.ToString(),
                RowKey = chapterNumber.ToString(),
                ChapterNumber = chapterNumber,
                ChapterName = chapterName,
                ContentMarkdown = shouldCleanMarkdown
                    ? string.Join("\r\n", chapterLines).RemoveMarkdownCharacters()
                    : string.Join("\r\n", chapterLines)
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
