using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Processors.SOTG
{
    public class StarshipChapterRulesProcessor : BaseProcessor<ChapterRules>
    {
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;

        public StarshipChapterRulesProcessor(GlobalSearchTermRepository globalSearchTermRepository)
        {
            _globalSearchTermRepository = globalSearchTermRepository;
        }

        public override Task<List<ChapterRules>> FindBlocks(List<string> lines)
        {
            var chapters = new List<ChapterRules>();

            var chapter0StartIndex = lines.FindIndex(f => f == Localization.SOTGChapter0StartLine);
            var chapter1StartIndex = lines.FindIndex(f => f == Localization.SOTGChapter1StartLine);
            var chapter2StartIndex = lines.FindIndex(f => f == Localization.SOTGChapter2StartLine);
            var chapter3StartIndex = lines.FindIndex(f => f == Localization.SOTGChapter3StartLine);
            var chapter4StartIndex = lines.FindIndex(f => f == Localization.SOTGChapter4StartLine);
            var chapter5StartIndex = lines.FindIndex(f => f == Localization.SOTGChapter5StartLine);
            var chapter6StartIndex = lines.FindIndex(f => f == Localization.SOTGChapter6StartLine);
            var chapter7StartIndex = lines.FindIndex(f => f == Localization.SOTGChapter7StartLine);
            var chapter8StartIndex = lines.FindIndex(f => f == Localization.SOTGChapter8StartLine);
            var chapter9StartIndex = lines.FindIndex(f => f == Localization.SOTGChapter9StartLine);
            var chapter10StartIndex = lines.FindIndex(f => f == Localization.SOTGChapter10StartLine);
            var appendixAStartIndex = lines.FindIndex(f => f == Localization.SOTGAppendixAStartLine);
            var changelogStartIndex = lines.FindIndex(f => f == Localization.SOTGChangelogStartLine);

            var introLines = lines.Skip(chapter0StartIndex).Take(chapter1StartIndex - chapter0StartIndex)
                .CleanListOfStrings().ToList();
            introLines[2] = introLines[2].Insert(0, "T");
            chapters.Add(CreateStarshipChapterRules(introLines, 0, Localization.SOTGChapter0Title, SectionNames.SOTGChapterZeroSections,
                ""));

            var chapter1Lines = lines.Skip(chapter1StartIndex).Take(chapter2StartIndex - chapter1StartIndex)
                .CleanListOfStrings().ToList();
            chapter1Lines[2] = chapter1Lines[2].Insert(0, "Y");
            chapters.Add(CreateStarshipChapterRules(chapter1Lines, 1, Localization.SOTGChapter1Title,
                SectionNames.SOTGChapterOneSections, "stepByStep"));

            var chapter2EndIndex = lines.FindIndex(chapter2StartIndex, f => f == Localization.SOTGDeploymentsStartLine);
            var chapter2Lines = lines.Skip(chapter2StartIndex).Take(chapter2EndIndex - chapter2StartIndex)
                .CleanListOfStrings().ToList();
            chapter2Lines[2] = chapter2Lines[2].Insert(0, "A");
            chapters.Add(CreateStarshipChapterRules(chapter2Lines, 2, Localization.SOTGChapter2Title, SectionNames.PHBChapterTwoSections,
                "deployments"));

            var chapter3EndIndex = lines.FindIndex(chapter3StartIndex, f => f == Localization.SOTGShipSizeStartLine);
            var chapter3Lines = lines.Skip(chapter3StartIndex).Take(chapter3EndIndex - chapter3StartIndex)
                .CleanListOfStrings().ToList();
            var variantStart = lines.FindIndex(chapter3StartIndex, f => f == "## Variant: Space Stations");
            var variantLines = lines.Skip(variantStart).Take(chapter4StartIndex - variantStart).CleanListOfStrings().ToList();
            chapter3Lines.AddRange(variantLines);
            chapter3Lines[2] = chapter3Lines[2].Insert(0, "C");
            chapters.Add(CreateStarshipChapterRules(chapter3Lines, 3, Localization.SOTGChapter3Title,
                SectionNames.SOTGChapterThreeSections, "starshipSizes"));

            var chapter4EndIndex = lines.FindIndex(chapter4StartIndex, f => f == Localization.SOTGModificationsStart);
            var chapter4Lines = lines.Skip(chapter4StartIndex).Take(chapter4EndIndex - chapter4StartIndex)
                .CleanListOfStrings().ToList();
            chapter4Lines[2] = chapter4Lines[2].Insert(0, "A");
            chapters.Add(CreateStarshipChapterRules(chapter4Lines, 4, Localization.SOTGChapter4Title,
                SectionNames.SOTGChapterFourSections, "modifications"));

            var chapter5Lines = lines.Skip(chapter5StartIndex).Take(chapter6StartIndex - chapter5StartIndex)
                .CleanListOfStrings().ToList();
            chapter5Lines[2] = chapter5Lines[2].Insert(0, "T");
            chapters.Add(CreateStarshipChapterRules(chapter5Lines, 5, Localization.SOTGChapter5Title, SectionNames.SOTGChapterFiveSections,
                "equipment"));

            var chapter6EndIndex = lines.FindIndex(lines.FindIndex(chapter6StartIndex, f => f == Localization.SOTGVenturesStart),
                f => f.StartsWith("### "));
            var chapter6Lines = lines.Skip(chapter6StartIndex).Take(chapter6EndIndex - chapter6StartIndex)
                .CleanListOfStrings().ToList();
            chapter6Lines[2] = chapter6Lines[2].Insert(0, "T");
            chapters.Add(CreateStarshipChapterRules(chapter6Lines, 6, Localization.SOTGChapter6Title,
                SectionNames.SOTGChapterSixSections, "customization"));

            var chapter7Lines = lines.Skip(chapter7StartIndex).Take(chapter8StartIndex - chapter7StartIndex)
                .CleanListOfStrings().ToList();
            chapter7Lines[2] = chapter7Lines[2].Insert(0, "S");
            chapters.Add(CreateStarshipChapterRules(chapter7Lines, 7, Localization.SOTGChapter7Title,
                SectionNames.SOTGChapterSevenSections, "abilityScores"));

            var chapter8Lines = lines.Skip(chapter8StartIndex).Take(chapter9StartIndex - chapter8StartIndex)
                .CleanListOfStrings().ToList();
            chapter8Lines[2] = chapter8Lines[2].Insert(0, "D");
            chapters.Add(CreateStarshipChapterRules(chapter8Lines, 8, Localization.SOTGChapter8Title,
                SectionNames.SOTGChapterEightSections, "adventuring"));

            var chapter9Lines = lines.Skip(chapter9StartIndex).Take(chapter10StartIndex - chapter9StartIndex).CleanListOfStrings().ToList();
            chapter9Lines[2] = chapter9Lines[2].Insert(0, "A");
            chapters.Add(CreateStarshipChapterRules(chapter9Lines, 9, Localization.SOTGChapter9Title, SectionNames.SOTGChapterNineSections,
                "combat"));

            var chapter10Lines = lines.Skip(chapter10StartIndex).Take(appendixAStartIndex - chapter10StartIndex).CleanListOfStrings().ToList();
            chapter10Lines[2] = chapter10Lines[2].Insert(0, "C");
            chapters.Add(CreateStarshipChapterRules(chapter10Lines, 10, Localization.SOTGChapter10Title,
                SectionNames.SOTGChapterTenSections, "generatingEncounters"));

            var appendixALines = lines.Skip(appendixAStartIndex).Take(changelogStartIndex - appendixAStartIndex).CleanListOfStrings().ToList();
            appendixALines[2] = appendixALines[2].Insert(0, "C");
            chapters.Add(CreateStarshipChapterRules(appendixALines, 11, Localization.SOTGAppendixATitle,
                SectionNames.SOTGAppendixAConditionsSections, "conditions"));

            var changelogLines = lines.Skip(changelogStartIndex).CleanListOfStrings().ToList();
            chapters.Add(CreateStarshipChapterRules(changelogLines, 99, Localization.SOTGChangelogTitle));

            foreach (var sotgChapterName in SectionNames.SOTGChapterNames)
            {
                var searchTerm = _globalSearchTermRepository.CreateSearchTerm(sotgChapterName.name,
                    sotgChapterName.globalSearchTermType, ContentType.Core, sotgChapterName.pathOverride);
                _globalSearchTermRepository.SearchTerms.Add(searchTerm);
            }

            return Task.FromResult(chapters);
        }

        private ChapterRules CreateStarshipChapterRules(IEnumerable<string> chapterLines, int chapterNumber,
            string chapterName,
            IReadOnlyCollection<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> searchTerms = null,
            string path = null)
        {
            if (searchTerms != null)
            {
                foreach (var globalSearchTermType in searchTerms)
                {
                    var searchTerm = _globalSearchTermRepository.CreateSectionSearchTermFromName(globalSearchTermType.name,
                        globalSearchTermType.globalSearchTermType, ContentType.Core, chapterName, $"/rules/sotg/{path}",
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
