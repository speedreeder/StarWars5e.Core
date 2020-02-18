using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Parsers.WH
{
    public class WretchedHivesChapterRulesProcessor : BaseProcessor<ChapterRules>
    {
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;

        public WretchedHivesChapterRulesProcessor(GlobalSearchTermRepository globalSearchTermRepository)
        {
            _globalSearchTermRepository = globalSearchTermRepository;
        }

        public override Task<List<ChapterRules>> FindBlocks(List<string> lines)
        {
            var chapters = new List<ChapterRules>();

            var chapter0StartIndex = lines.FindIndex(f => f == "# Introduction");
            var chapter1StartIndex = lines.FindIndex(f => f == "# Chapter 1: Step-By-Step Factions");
            var chapter2StartIndex = lines.FindIndex(f => f == "# Chapter 2: Entertainment and Downtime");
            var chapter3StartIndex = lines.FindIndex(f => f == "# Chapter 3: Factions and Membership");
            var chapter4StartIndex = lines.FindIndex(f => f == "# Chapter 4: Using Ability Scores");
            var chapter5StartIndex = lines.FindIndex(f => f == "# Chapter 5: Equipment");
            var chapter6StartIndex = lines.FindIndex(f => f == "# Chapter 6: Customization Options");
            var chapter7StartIndex = lines.FindIndex(f => f == "# Chapter 7: Enhanced Items");
            var chapter8StartIndex = lines.FindIndex(f => f == "# Chapter 8: Tool Proficiencies");
            var appendixAStartIndex = lines.FindIndex(f => f == "# Appendix A: Enhanced Items");
            var changelogStartIndex = lines.FindIndex(f => f == "# Changelog");

            var chapter0Lines = lines.Skip(chapter0StartIndex).Take(chapter1StartIndex - chapter0StartIndex)
                .CleanListOfStrings().ToList();
            chapter0Lines[2] = chapter0Lines[2].Insert(0, "T");
            chapters.Add(CreateWretchedHivesChapterRules(chapter0Lines, 1, "Introduction", SectionNames.WHChapterZeroSections,
                "introduction"));

            var chapter1Lines = lines.Skip(chapter1StartIndex).Take(chapter2StartIndex - chapter1StartIndex)
                .CleanListOfStrings().ToList();
            chapter1Lines[2] = chapter1Lines[2].Insert(0, "M");
            chapters.Add(CreateWretchedHivesChapterRules(chapter1Lines, 1, "Step-By-Step Factions", SectionNames.WHChapterOneSections,
                "stepByStep"));

            var chapter2Lines = lines.Skip(chapter2StartIndex).Take(chapter3StartIndex - chapter2StartIndex)
                .CleanListOfStrings().ToList();
            chapter2Lines[2] = chapter2Lines[2].Insert(0, "A");
            chapters.Add(CreateWretchedHivesChapterRules(chapter2Lines, 2, "Entertainment and Downtime", SectionNames.WHChapterTwoSections,
                "downtime"));

            var chapter3Lines = lines.Skip(chapter3StartIndex).Take(chapter5StartIndex - chapter3StartIndex)
                .CleanListOfStrings().ToList();
            chapter3Lines[2] = chapter3Lines[2].Insert(0, "A");
            chapters.Add(CreateWretchedHivesChapterRules(chapter3Lines, 3, "Factions and Membership", SectionNames.WHChapterThreeSections,
                "factionsAndMembership"));

            var chapter4Lines = lines.Skip(chapter4StartIndex).Take(chapter5StartIndex - chapter4StartIndex)
                .CleanListOfStrings().ToList();
            chapter4Lines[2] = chapter4Lines[2].Insert(0, "S");
            chapters.Add(CreateWretchedHivesChapterRules(chapter4Lines, 3, "Using Ability Scores", SectionNames.WHChapterFourSections,
                "abilityScores"));

            var chapter5Lines = lines.Skip(chapter5StartIndex).Take(chapter6StartIndex - chapter5StartIndex)
                .CleanListOfStrings().ToList();
            chapter5Lines[2] = chapter5Lines[2].Insert(0, "T");
            chapters.Add(CreateWretchedHivesChapterRules(chapter5Lines, 5, "Equipment", SectionNames.WHChapterFiveSections,
                "equipment"));

            var chapter6Lines = lines.Skip(chapter6StartIndex).Take(chapter7StartIndex - chapter6StartIndex)
                .CleanListOfStrings().ToList();
            chapter6Lines[2] = chapter6Lines[2].Insert(0, "T");
            chapters.Add(CreateWretchedHivesChapterRules(chapter6Lines, 6, "Customization Options", SectionNames.WHChapterSixSections,
                "customizationOptions"));

            var chapter7Lines = lines.Skip(chapter7StartIndex).Take(chapter8StartIndex - chapter7StartIndex)
                .CleanListOfStrings().ToList();
            chapter7Lines[2] = chapter7Lines[2].Insert(0, "T");
            chapters.Add(CreateWretchedHivesChapterRules(chapter7Lines, 7, "Enhanced Items", SectionNames.WHChapterSevenSections,
                "enhancedItems"));

            var chapter8Lines = lines.Skip(chapter8StartIndex).Take(appendixAStartIndex - chapter8StartIndex)
                .CleanListOfStrings().ToList();
            chapter8Lines[2] = chapter8Lines[2].Insert(0, "T");
            chapters.Add(CreateWretchedHivesChapterRules(chapter8Lines, 8, "Tool Proficiencies", SectionNames.WHChapterEightSections,
                "toolProficiencies"));

            var changelogLines = lines.Skip(changelogStartIndex).CleanListOfStrings().ToList();
            chapters.Add(CreateWretchedHivesChapterRules(changelogLines, 99, "Changelog"));

            foreach (var whChapterName in SectionNames.WHChapterNames)
            {
                var searchTerm = _globalSearchTermRepository.CreateSearchTerm(whChapterName.name,
                    whChapterName.globalSearchTermType, ContentType.Core, whChapterName.pathOverride);
                _globalSearchTermRepository.SearchTerms.Add(searchTerm);
            }

            return Task.FromResult(chapters);
        }

        private ChapterRules CreateWretchedHivesChapterRules(IEnumerable<string> chapterLines, int chapterNumber,
            string chapterName,
            IReadOnlyCollection<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> searchTerms = null,
            string path = null)
        {
            if (searchTerms != null)
            {
                foreach (var globalSearchTermType in searchTerms)
                {
                    var searchTerm = _globalSearchTermRepository.CreateSectionSearchTermFromName(globalSearchTermType.name,
                        globalSearchTermType.globalSearchTermType, ContentType.Core, chapterName, $"/rules/hives/{path}",
                        globalSearchTermType.pathOverride);
                    _globalSearchTermRepository.SearchTerms.Add(searchTerm);
                }
            }

            var chapter = new ChapterRules
            {
                PartitionKey = ContentType.Core.ToString(),
                RowKey = chapterNumber.ToString(),
                ChapterNumber = chapterNumber,
                ChapterName = chapterName,
                ContentMarkdown = string.Join("\r\n", chapterLines)
            };
            return chapter;
        }
    }
}
