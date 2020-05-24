using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Processors.PHB
{
    public class PlayerHandbookChapterRulesProcessor : BaseProcessor<ChapterRules>
    {
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;

        public PlayerHandbookChapterRulesProcessor(GlobalSearchTermRepository globalSearchTermRepository)
        {
            _globalSearchTermRepository = globalSearchTermRepository;
        }
        public override Task<List<ChapterRules>> FindBlocks(List<string> lines)
        {
            var chapters = new List<ChapterRules>();

            var prefaceStartIndex = lines.FindIndex(f => f == Localization.PHBPrefaceStartLine);
            var whatsDifferentStartIndex = lines.FindIndex(f => f == Localization.PHBWhatsDifferentStartLine);
            var chapter0StartIndex = lines.FindIndex(f => f == Localization.PHBIntroductionStartLine);
            var chapter1StartIndex = lines.FindIndex(f => f == Localization.PHBChapter1StartLine);
            var chapter2StartIndex = lines.FindIndex(f => f == Localization.PHBChapter2StartLine);
            var chapter3StartIndex = lines.FindIndex(f => f == Localization.PHBChapter3StartLine);
            var chapter4StartIndex = lines.FindIndex(f => f == Localization.PHBChapter4StartLine);
            var chapter5StartIndex = lines.FindIndex(f => f == Localization.PHBChapter5StartLine);
            var chapter6StartIndex = lines.FindIndex(f => f == Localization.PHBChapter6StartLine);
            var chapter7StartIndex = lines.FindIndex(f => f == Localization.PHBChapter7StartLine);
            var chapter8StartIndex = lines.FindIndex(f => f == Localization.PHBChapter8StartLine);
            var chapter9StartIndex = lines.FindIndex(f => f == Localization.PHBChapter9StartLine);
            var chapter10StartIndex = lines.FindIndex(f => f == Localization.PHBChapter10StartLine);
            var chapter11StartIndex = lines.FindIndex(f => f == Localization.PHBChapter11StartLine);
            var appendixAStartIndex = lines.FindIndex(f => f == Localization.PHBAppendixAStartLine);
            var appendixBStartIndex = lines.FindIndex(f => f == Localization.PHBAppendixBStartLine);
            var changelogStartIndex = lines.FindIndex(f => f == Localization.PHBChangelogStartLine);

            var prefaceLines = lines.Skip(prefaceStartIndex).Take(whatsDifferentStartIndex - prefaceStartIndex)
                .CleanListOfStrings().ToList();
            chapters.Add(CreateChapterRules(prefaceLines, -2, Localization.PHBPrefaceTitle));

            var whatsDifferentLines = lines.Skip(whatsDifferentStartIndex).Take(chapter0StartIndex - whatsDifferentStartIndex)
                .CleanListOfStrings().ToList();
            chapters.Add(CreateChapterRules(whatsDifferentLines, -1, Localization.PHBWhatsDifferentTitle));

            var introLines = lines.Skip(chapter0StartIndex).Take(chapter1StartIndex - chapter0StartIndex)
                .CleanListOfStrings().ToList();
            introLines[2] = introLines[2].Insert(0, "T");
            chapters.Add(CreateChapterRules(introLines, 0, Localization.PHBIntroductionTitle));

            var chapter1Lines = lines.Skip(chapter1StartIndex).Take(chapter2StartIndex - chapter1StartIndex)
                .CleanListOfStrings().ToList();
            chapter1Lines[2] = chapter1Lines[2].Insert(0, "Y");
            chapters.Add(CreateChapterRules(chapter1Lines, 1, Localization.PHBChapter1Title,
                SectionNames.PHBChapterOneSections, "stepByStep"));

            var chapter2EndIndex = lines.FindIndex(chapter2StartIndex, f => f.StartsWith("___"));
            var chapter2Lines = lines.Skip(chapter2StartIndex).Take(chapter2EndIndex - chapter2StartIndex)
                .CleanListOfStrings().ToList();
            chapter2Lines[2] = chapter2Lines[2].Insert(0, "A ");
            chapters.Add(CreateChapterRules(chapter2Lines, 2, Localization.PHBChapter2Title, SectionNames.PHBChapterTwoSections,
                "species"));

            var chapter3EndIndex = lines.FindIndex(chapter3StartIndex, f => f.StartsWith(Localization.PHBClassesStartLine));
            var chapter3Lines = lines.Skip(chapter3StartIndex).Take(chapter3EndIndex - chapter3StartIndex)
                .CleanListOfStrings().ToList();
            chapter3Lines[2] = chapter3Lines[2].Insert(0, "A");
            chapters.Add(CreateChapterRules(chapter3Lines, 3, Localization.PHBChapter3Title, SectionNames.PHBChapterThreeSections,
                "classes"));

            var chapter4EndIndex = lines.FindIndex(chapter4StartIndex, f => f.StartsWith(Localization.PHBBackgroundsStartLine));
            var chapter4Lines = lines.Skip(chapter4StartIndex).Take(chapter4EndIndex - chapter4StartIndex)
                .CleanListOfStrings().ToList();
            chapter4Lines[2] = chapter4Lines[2].Insert(0, "C");
            chapters.Add(CreateChapterRules(chapter4Lines, 4, Localization.PHBChapter4Title,
                SectionNames.PHBChapterFourSections, "backgrounds"));

            var chapter5Lines = lines.Skip(chapter5StartIndex).Take(chapter6StartIndex - chapter5StartIndex)
                .CleanListOfStrings().ToList();
            chapter5Lines[2] = chapter5Lines[2].Insert(0, "T");
            chapters.Add(CreateChapterRules(chapter5Lines, 5, Localization.PHBChapter5Title, SectionNames.PHBChapterFiveSections,
                "equipment"));

            var chapter6EndIndex = lines.FindIndex(chapter6StartIndex, f => f.StartsWith(Localization.PHBFeatsStartLine));
            var chapter6Lines = lines.Skip(chapter6StartIndex).Take(chapter6EndIndex - chapter6StartIndex)
                .CleanListOfStrings().ToList();
            chapter6Lines[2] = chapter6Lines[2].Insert(0, "T");
            chapters.Add(CreateChapterRules(chapter6Lines, 6, Localization.PHBChapter6Title,
                SectionNames.PHBChapterSixSections, "customization"));

            var chapter7Lines = lines.Skip(chapter7StartIndex).Take(chapter8StartIndex - chapter7StartIndex)
                .CleanListOfStrings().ToList();
            chapter7Lines[2] = chapter7Lines[2].Insert(0, "S");
            chapters.Add(CreateChapterRules(chapter7Lines, 7, Localization.PHBChapter7Title,
                SectionNames.PHBChapterSevenSections, "abilityScores"));

            var chapter8Lines = lines.Skip(chapter8StartIndex).Take(chapter9StartIndex - chapter8StartIndex)
                .CleanListOfStrings().ToList();
            chapter8Lines[2] = chapter8Lines[2].Insert(0, "D");
            chapters.Add(CreateChapterRules(chapter8Lines, 8, Localization.PHBChapter8Title, SectionNames.PHBChapterEightSections,
                "adventuring"));

            var chapter9Lines = lines.Skip(chapter9StartIndex).Take(chapter10StartIndex - chapter9StartIndex)
                .CleanListOfStrings().ToList();
            chapter9Lines[2] = chapter9Lines[2].Insert(0, "T");
            chapters.Add(CreateChapterRules(chapter9Lines, 9, Localization.PHBChapter9Title, SectionNames.PHBChapterNineSections, "combat"));

            var chapter10Lines = lines.Skip(chapter10StartIndex).Take(chapter11StartIndex - chapter10StartIndex)
                .CleanListOfStrings().ToList();
            chapter10Lines[2] = chapter10Lines[2].Insert(0, "M");
            chapters.Add(CreateChapterRules(chapter10Lines, 10, Localization.PHBChapter10Title,
                SectionNames.PHBChapterTenSections, "casting"));

            var appendixALines = lines.Skip(appendixAStartIndex).Take(appendixBStartIndex - appendixAStartIndex)
                .CleanListOfStrings().ToList();
            appendixALines[2] = appendixALines[2].Insert(0, "C");
            chapters.Add(CreateChapterRules(appendixALines, 13, Localization.PHBAppendixATitle,
                SectionNames.PHBAppendixAConditionsSections, "conditions"));

            var appendixBLines = lines.Skip(appendixBStartIndex).Take(changelogStartIndex - appendixBStartIndex).CleanListOfStrings().ToList();
            appendixBLines[2] = appendixBLines[2].Insert(0, "H");
            chapters.Add(CreateChapterRules(appendixBLines, 14, Localization.PHBAppendixBTitle,
                SectionNames.PHBAppendixBVariantRulesSections, "variantRules"));

            var changelogLines = lines.Skip(changelogStartIndex).CleanListOfStrings().ToList();
            chapters.Add(CreateChapterRules(changelogLines, 99, Localization.PHBChangelogTitle));

            foreach (var phbChapterName in SectionNames.PHBChapterNames)
            {
                var searchTerm = _globalSearchTermRepository.CreateSearchTerm(phbChapterName.name,
                    phbChapterName.globalSearchTermType, ContentType.Core, phbChapterName.pathOverride);
                _globalSearchTermRepository.SearchTerms.Add(searchTerm);
            }

            return Task.FromResult(chapters);
        }

        private ChapterRules CreateChapterRules(IEnumerable<string> chapterLines, int chapterNumber, string chapterName,
            IReadOnlyCollection<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> searchTerms = null, string path = null)
        {
            if(searchTerms != null)
            {
                foreach (var globalSearchTermType in searchTerms)
                {
                    var searchTerm = _globalSearchTermRepository.CreateSectionSearchTermFromName(globalSearchTermType.name,
                        globalSearchTermType.globalSearchTermType, ContentType.Core, chapterName, $"/rules/phb/{path}", globalSearchTermType.pathOverride);
                    _globalSearchTermRepository.SearchTerms.Add(searchTerm);
                }
            }

            var chapter = new ChapterRules
            {
                PartitionKey = ContentType.Core.ToString(),
                ContentTypeEnum = ContentType.Core,
                RowKey = chapterNumber.ToString(),
                ChapterNumber = chapterNumber,
                ChapterName = chapterName,
                ContentMarkdown = string.Join("\r\n", chapterLines)
            };
            return chapter;
        }
    }
}
