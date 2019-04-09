using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Parsers.PHB
{
    public class PlayerHandbookChapterRulesProcessor : BaseProcessor<ChapterRules>
    {
        public override Task<List<ChapterRules>> FindBlocks(List<string> lines)
        {
            var chapters = new List<ChapterRules>();

            var chapter0StartIndex = lines.FindIndex(f => f == "# Introduction");
            var chapter1StartIndex = lines.FindIndex(f => f == "# Chapter 1: Step-By-Step Characters");
            var chapter2StartIndex = lines.FindIndex(f => f == "# Chapter 2: Species");
            var chapter3StartIndex = lines.FindIndex(f => f == "# Chapter 3: Classes");
            var chapter4StartIndex = lines.FindIndex(f => f == "# Chapter 4: Personality and Backgrounds");
            var chapter5StartIndex = lines.FindIndex(f => f == "# Chapter 5: Equipment");
            var chapter6StartIndex = lines.FindIndex(f => f == "# Chapter 6: Customization Options");
            var chapter7StartIndex = lines.FindIndex(f => f == "# Chapter 7: Using Ability Scores");
            var chapter8StartIndex = lines.FindIndex(f => f == "# Chapter 8: Adventuring");
            var chapter9StartIndex = lines.FindIndex(f => f == "# Chapter 9: Combat");
            var chapter10StartIndex = lines.FindIndex(f => f == "# Chapter 10: Force- and Tech-casting");
            var chapter11StartIndex = lines.FindIndex(f => f == "# Chapter 11: Force Powers");
            var chapter12StartIndex = lines.FindIndex(f => f == "# Chapter 12: Tech Powers");
            var appendixAStartIndex = lines.FindIndex(f => f == "# Appendix A: Conditions");
            var appendixBStartIndex = lines.FindIndex(f => f == "# Appendix B: Recommended Variant Rules");

            chapters.Add(CreateChapterRules(
                lines.Skip(chapter0StartIndex).Take(chapter1StartIndex - chapter0StartIndex).CleanListOfStrings(), 0,
                "Introduction"));
            chapters.Add(CreateChapterRules(
                lines.Skip(chapter1StartIndex).Take(chapter2StartIndex - chapter1StartIndex).CleanListOfStrings(), 1,
                "Step-By-Step Characters"));

            var chapter2EndIndex = lines.FindIndex(chapter2StartIndex, f => f.StartsWith("___"));
            chapters.Add(CreateChapterRules(
                lines.Skip(chapter2StartIndex).Take(chapter2EndIndex - chapter2StartIndex).CleanListOfStrings(), 2,
                "Species"));

            var chapter3EndIndex = lines.FindIndex(chapter3StartIndex, f => f.StartsWith("##### Classes"));
            chapters.Add(CreateChapterRules(
                lines.Skip(chapter3StartIndex).Take(chapter3EndIndex - chapter3StartIndex).CleanListOfStrings(), 3,
                "Classes"));

            var chapter4EndIndex = lines.FindIndex(chapter4StartIndex, f => f.StartsWith("## Agent"));
            chapters.Add(CreateChapterRules(
                lines.Skip(chapter4StartIndex).Take(chapter4EndIndex - chapter4StartIndex).CleanListOfStrings(), 4,
                "Personality and Backgrounds"));

            chapters.Add(CreateChapterRules(
                lines.Skip(chapter5StartIndex).Take(chapter6StartIndex - chapter5StartIndex).CleanListOfStrings(), 5,
                "Equipment"));

            var chapter6EndIndex = lines.FindIndex(chapter6StartIndex, f => f.StartsWith("## Feats"));
            chapters.Add(CreateChapterRules(
                lines.Skip(chapter6StartIndex).Take(chapter6EndIndex - chapter6StartIndex).CleanListOfStrings(), 6,
                "Classes"));

            chapters.Add(CreateChapterRules(
                lines.Skip(chapter7StartIndex).Take(chapter8StartIndex - chapter7StartIndex).CleanListOfStrings(), 7,
                "Using Ability Scores"));

            chapters.Add(CreateChapterRules(
                lines.Skip(chapter8StartIndex).Take(chapter9StartIndex - chapter8StartIndex).CleanListOfStrings(), 8,
                "Adventuring"));

            chapters.Add(CreateChapterRules(
                lines.Skip(chapter9StartIndex).Take(chapter10StartIndex - chapter9StartIndex).CleanListOfStrings(), 9,
                "Combat"));

            chapters.Add(CreateChapterRules(
                lines.Skip(chapter10StartIndex).Take(chapter11StartIndex - chapter10StartIndex).CleanListOfStrings(), 10,
                "Force- and Tech-casting"));

            chapters.Add(CreateChapterRules(
                lines.Skip(appendixAStartIndex).Take(appendixBStartIndex - appendixAStartIndex).CleanListOfStrings(), 13,
                "Appendix A: Conditions"));

            chapters.Add(CreateChapterRules(
                lines.Skip(appendixBStartIndex).CleanListOfStrings(), 14,
                "Appendix B: Recommended Variant Rules"));

            return Task.FromResult(chapters);
        }

        private static ChapterRules CreateChapterRules(IEnumerable<string> chapterLines, int chapterNumber, string chapterName)
        {
            var chapter = new ChapterRules
            {
                PartitionKey = ContentType.Base.ToString(),
                ContentTypeEnum = ContentType.Base,
                RowKey = chapterNumber.ToString(),
                ChapterNumber = chapterNumber,
                ChapterName = chapterName,
                ContentMarkdown = string.Join("\r\n", chapterLines)
            };
            return chapter;
        }
    }
}
