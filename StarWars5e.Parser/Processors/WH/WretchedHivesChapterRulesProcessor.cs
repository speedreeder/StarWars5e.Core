using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Processors.WH
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

            var chapter0StartIndex = lines.FindIndex(f => f == Localization.WHChapter0StartLine);
            var chapter1StartIndex = lines.FindIndex(f => f == Localization.WHChapter1StartLine);
            var chapter2StartIndex = lines.FindIndex(f => f == Localization.WHChapter2StartLine);
            var chapter3StartIndex = lines.FindIndex(f => f == Localization.WHChapter3StartLine);
            var chapter4StartIndex = lines.FindIndex(f => f == Localization.WHChapter4StartLine);
            var chapter5StartIndex = lines.FindIndex(f => f == Localization.WHChapter5StartLine);
            var chapter6StartIndex = lines.FindIndex(f => f == Localization.WHChapter6StartLine);
            var chapter7StartIndex = lines.FindIndex(f => f == Localization.WHChapter7StartLine);
            var chapter8StartIndex = lines.FindIndex(f => f == Localization.WHChapter8StartLine);
            var appendixAStartIndex = lines.FindIndex(f => f == Localization.WHAppendixAStartLine);
            var changelogStartIndex = lines.FindIndex(f => f == Localization.WHChangelogStartLine);

            var chapter0Lines = lines.Skip(chapter0StartIndex).Take(chapter1StartIndex - chapter0StartIndex)
                .CleanListOfStrings().ToList();
            chapter0Lines[2] = chapter0Lines[2].Insert(0, "T");
            chapters.Add(CreateWretchedHivesChapterRules(chapter0Lines, 1, Localization.WHChapter0Title, SectionNames.WHChapterZeroSections,
                "introduction"));

            var chapter1Lines = lines.Skip(chapter1StartIndex).Take(chapter2StartIndex - chapter1StartIndex)
                .CleanListOfStrings().ToList();
            chapter1Lines[2] = chapter1Lines[2].Insert(0, "M");
            chapters.Add(CreateWretchedHivesChapterRules(chapter1Lines, 1, Localization.WHChapter1Title, SectionNames.WHChapterOneSections,
                "stepByStep"));

            var chapter2Lines = lines.Skip(chapter2StartIndex).Take(chapter3StartIndex - chapter2StartIndex)
                .CleanListOfStrings().ToList();
            chapter2Lines[2] = chapter2Lines[2].Insert(0, "A");
            chapters.Add(CreateWretchedHivesChapterRules(chapter2Lines, 2, Localization.WHChapter2Title, SectionNames.WHChapterTwoSections,
                "downtime"));

            var chapter3Lines = lines.Skip(chapter3StartIndex).Take(chapter5StartIndex - chapter3StartIndex)
                .CleanListOfStrings().ToList();
            chapter3Lines[2] = chapter3Lines[2].Insert(0, "A");
            chapters.Add(CreateWretchedHivesChapterRules(chapter3Lines, 3, Localization.WHChapter3Title, SectionNames.WHChapterThreeSections,
                "factionsAndMembership"));

            var chapter4Lines = lines.Skip(chapter4StartIndex).Take(chapter5StartIndex - chapter4StartIndex)
                .CleanListOfStrings().ToList();
            chapter4Lines[2] = chapter4Lines[2].Insert(0, "S");
            chapters.Add(CreateWretchedHivesChapterRules(chapter4Lines, 4, Localization.WHChapter4Title, SectionNames.WHChapterFourSections,
                "abilityScores"));

            var chapter5Lines = lines.Skip(chapter5StartIndex).Take(chapter6StartIndex - chapter5StartIndex)
                .CleanListOfStrings().ToList();
            chapter5Lines[2] = chapter5Lines[2].Insert(0, "T");
            chapters.Add(CreateWretchedHivesChapterRules(chapter5Lines, 5, Localization.WHChapter5Title, SectionNames.WHChapterFiveSections,
                "equipment"));

            var chapter6Lines = lines.Skip(chapter6StartIndex).Take(chapter7StartIndex - chapter6StartIndex)
                .CleanListOfStrings().ToList();
            chapter6Lines[2] = chapter6Lines[2].Insert(0, "T");
            chapters.Add(CreateWretchedHivesChapterRules(chapter6Lines, 6, Localization.WHChapter6Title, SectionNames.WHChapterSixSections,
                "customizationOptions"));

            var chapter7Lines = lines.Skip(chapter7StartIndex).Take(chapter8StartIndex - chapter7StartIndex)
                .CleanListOfStrings().ToList();
            chapter7Lines[2] = chapter7Lines[2].Insert(0, "T");
            chapters.Add(CreateWretchedHivesChapterRules(chapter7Lines, 7, Localization.WHChapter7Title, SectionNames.WHChapterSevenSections,
                "enhancedItems"));

            var chapter8Lines = lines.Skip(chapter8StartIndex).Take(appendixAStartIndex - chapter8StartIndex)
                .CleanListOfStrings().ToList();
            chapter8Lines[2] = chapter8Lines[2].Insert(0, "T");
            chapters.Add(CreateWretchedHivesChapterRules(chapter8Lines, 8, Localization.WHChapter8Title, SectionNames.WHChapterEightSections,
                "toolProficiencies"));

            var changelogLines = lines.Skip(changelogStartIndex).CleanListOfStrings().ToList();
            chapters.Add(CreateWretchedHivesChapterRules(changelogLines, 99, Localization.WHChangelogTitle));

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
                        globalSearchTermType.globalSearchTermType, ContentType.Core, chapterName, $"/rules/wh/{path}",
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
