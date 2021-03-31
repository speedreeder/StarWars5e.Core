using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            if (Localization.Language == Language.en) introLines[2] = introLines[2].Insert(0, "T");
            chapters.Add(CreateStarshipChapterRulesAndSearchTerms(introLines, 0, Localization.SOTGChapter0Title));

            var chapter1Lines = lines.Skip(chapter1StartIndex).Take(chapter2StartIndex - chapter1StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter1Lines[2] = chapter1Lines[2].Insert(0, "Y");
            chapters.Add(CreateStarshipChapterRulesAndSearchTerms(chapter1Lines, 1, Localization.SOTGChapter1Title,
                "stepByStep"));

            var chapter2EndIndex = lines.FindIndex(chapter2StartIndex, f => f == Localization.SOTGDeploymentsStartLine);
            var chapter2Lines = lines.Skip(chapter2StartIndex).Take(chapter2EndIndex - chapter2StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter2Lines[2] = chapter2Lines[2].Insert(0, "A");
            chapters.Add(CreateStarshipChapterRulesAndSearchTerms(chapter2Lines, 2, Localization.SOTGChapter2Title,
                "deployments"));

            var chapter3EndIndex = lines.FindIndex(chapter3StartIndex, f => f == Localization.SOTGShipSizeStartLine);
            var chapter3Lines = lines.Skip(chapter3StartIndex).Take(chapter3EndIndex - chapter3StartIndex)
                .CleanListOfStrings().ToList();
            var variantStart = lines.FindIndex(chapter3StartIndex, f => f == Localization.SOTGVariantSpaceStations);
            var variantLines = lines.Skip(variantStart).Take(chapter4StartIndex - variantStart).CleanListOfStrings().ToList();
            chapter3Lines.AddRange(variantLines);
            if (Localization.Language == Language.en) chapter3Lines[2] = chapter3Lines[2].Insert(0, "C");
            chapters.Add(CreateStarshipChapterRulesAndSearchTerms(chapter3Lines, 3, Localization.SOTGChapter3Title,
                "starshipSizes"));

            var chapter4EndIndex = lines.FindIndex(chapter4StartIndex, f => f == Localization.SOTGModificationsStart);
            var chapter4Lines = lines.Skip(chapter4StartIndex).Take(chapter4EndIndex - chapter4StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter4Lines[2] = chapter4Lines[2].Insert(0, "A");
            chapters.Add(CreateStarshipChapterRulesAndSearchTerms(chapter4Lines, 4, Localization.SOTGChapter4Title,
                "modifications"));

            var chapter5Lines = lines.Skip(chapter5StartIndex).Take(chapter6StartIndex - chapter5StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter5Lines[2] = chapter5Lines[2].Insert(0, "T");
            chapters.Add(CreateStarshipChapterRulesAndSearchTerms(chapter5Lines, 5, Localization.SOTGChapter5Title,
                "equipment"));

            var chapter6Lines = lines.Skip(chapter6StartIndex).Take(chapter7StartIndex - chapter6StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter6Lines[2] = chapter6Lines[2].Insert(0, "T");
            chapters.Add(CreateStarshipChapterRulesAndSearchTerms(chapter6Lines, 6, Localization.SOTGChapter6Title,
                "customization"));

            var chapter7Lines = lines.Skip(chapter7StartIndex).Take(chapter8StartIndex - chapter7StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter7Lines[2] = chapter7Lines[2].Insert(0, "S");
            chapters.Add(CreateStarshipChapterRulesAndSearchTerms(chapter7Lines, 7, Localization.SOTGChapter7Title,
                "abilityScores"));

            var chapter8Lines = lines.Skip(chapter8StartIndex).Take(chapter9StartIndex - chapter8StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter8Lines[2] = chapter8Lines[2].Insert(0, "D");
            chapters.Add(CreateStarshipChapterRulesAndSearchTerms(chapter8Lines, 8, Localization.SOTGChapter8Title,
               "adventuring"));

            var chapter9Lines = lines.Skip(chapter9StartIndex).Take(chapter10StartIndex - chapter9StartIndex).CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter9Lines[2] = chapter9Lines[2].Insert(0, "A");
            chapters.Add(CreateStarshipChapterRulesAndSearchTerms(chapter9Lines, 9, Localization.SOTGChapter9Title,
                "combat"));

            var chapter10Lines = lines.Skip(chapter10StartIndex).Take(appendixAStartIndex - chapter10StartIndex).CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter10Lines[2] = chapter10Lines[2].Insert(0, "C");
            chapters.Add(CreateStarshipChapterRulesAndSearchTerms(chapter10Lines, 10, Localization.SOTGChapter10Title,
                "generatingEncounters"));

            var appendixALines = lines.Skip(appendixAStartIndex).Take(changelogStartIndex - appendixAStartIndex).CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) appendixALines[2] = appendixALines[2].Insert(0, "C");
            chapters.Add(CreateStarshipChapterRulesAndSearchTerms(appendixALines, 11, Localization.SOTGAppendixATitle,
                "conditions"));

            var changelogLines = lines.Skip(changelogStartIndex).CleanListOfStrings().ToList();
            chapters.Add(CreateStarshipChapterRulesAndSearchTerms(changelogLines, 99, Localization.SOTGChangelogTitle));

            var sotgSections = new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( Localization.StarshipsOfTheGalaxy, GlobalSearchTermType.Book, "/rules/sotg"),
                ( Localization.StarshipsOfTheGalaxyChangelog, GlobalSearchTermType.Changelog, "/rules/sotg/changelog")
            };

            foreach (var sotgSection in sotgSections)
            {
                var searchTerm = _globalSearchTermRepository.CreateSearchTerm(sotgSection.name,
                    sotgSection.globalSearchTermType, ContentType.Core, sotgSection.pathOverride);
                _globalSearchTermRepository.SearchTerms.Add(searchTerm);
            }

            return Task.FromResult(chapters);
        }

        private ChapterRules CreateStarshipChapterRulesAndSearchTerms(List<string> chapterLines, int chapterNumber,
            string chapterName, string path = null)
        {
            List<int> headerLineIndexes;
            var chapterLineIndexesToExclude = new List<int>();

            if (chapterNumber == 11)
            {
                headerLineIndexes = chapterLines.FindAllIndexOf(f => f.StartsWith("#### ") || f.StartsWith("# ")).Except(chapterLineIndexesToExclude)
                    .ToList();

                foreach (var headerLineIndex in headerLineIndexes)
                {
                    var line = chapterLines.ElementAt(headerLineIndex);

                    var searchTermType = GlobalSearchTermType.StarshipCondition;

                    if (line.Equals(chapterLines.First(f => f.StartsWith("# "))))
                    {
                        searchTermType = GlobalSearchTermType.StarshipChapter;
                    }

                    var name = line.RemoveHashtagCharacters().Trim();
                    var repeatIndexes = chapterLines.FindAllIndexOf(f => Regex.IsMatch(f, $@"^#+\s*{name}"));

                    var instance = repeatIndexes.IndexOf(headerLineIndex) + 1;
                    var searchTerm = _globalSearchTermRepository.CreateSectionSearchTermFromName(name,
                        searchTermType, ContentType.Core, chapterName, $"/rules/sotg/{path}",
                        instance > 1 ? $"{name} {instance}" : null);
                    _globalSearchTermRepository.SearchTerms.Add(searchTerm);
                }
            }
            else if (chapterNumber != 99)
            {
                var weaponPropertiesStartIndex = chapterLines.FindIndex(f => f.StartsWith($"### {Localization.WeaponProperties}"));
                if (weaponPropertiesStartIndex > 0)
                {
                    HandleSearchTermForProperty(chapterLines, weaponPropertiesStartIndex, chapterName,
                        path, GlobalSearchTermType.StarshipWeaponProperty, chapterLineIndexesToExclude);
                }

                HandleTables(chapterLines, chapterName, path, chapterLineIndexesToExclude);

                headerLineIndexes = chapterLines
                    .FindAllIndexOf(f => f.StartsWith('#') || f.StartsWith("> #")).Except(chapterLineIndexesToExclude)
                    .ToList();

                foreach (var headerLineIndex in headerLineIndexes)
                {
                    var line = chapterLines.ElementAt(headerLineIndex);

                    var searchTermType = GlobalSearchTermType.StarshipRule;

                    if (line.Equals(chapterLines.First(f => f.StartsWith("# "))))
                    {
                        searchTermType = GlobalSearchTermType.StarshipChapter;
                    }

                    if (line.Contains("variant:", StringComparison.InvariantCultureIgnoreCase))
                    {
                        searchTermType = GlobalSearchTermType.VariantRule;
                    }

                    var name = line.RemoveHashtagCharacters().Trim();
                    var repeatIndexes = chapterLines.FindAllIndexOf(f => Regex.IsMatch(f, $@"^#+\s*{name}"));

                    var instance = repeatIndexes.IndexOf(headerLineIndex) + 1;
                    var searchTerm = _globalSearchTermRepository.CreateSectionSearchTermFromName(name,
                        searchTermType, ContentType.Core, chapterName, $"/rules/sotg/{path}",
                        instance > 1 ? $"{name} {instance}" : null);
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

        private void HandleSearchTermForProperty(List<string> chapterLines, int startingIndex, string chapterName,
            string path, GlobalSearchTermType globalSearchTermType, List<int> chapterLineIndexesToExclude)
        {
            if (startingIndex > 0)
            {
                var endIndex = chapterLines.FindIndex(startingIndex + 1, f => Regex.IsMatch(f, @"^#{1,3}\s+"));

                var indexes = chapterLines
                    .FindAllIndexOf(f => f.StartsWith("####"))
                    .Where(i => i > startingIndex && i < (endIndex > 0 ? endIndex : chapterLines.Count - 1))
                    .Except(chapterLineIndexesToExclude);

                foreach (var index in indexes)
                {
                    var line = chapterLines.ElementAt(index);
                    var repeatIndexes = chapterLines.FindAllIndexOf(f => f == line);
                    var name = line.RemoveHashtagCharacters().Trim();

                    var instance = repeatIndexes.IndexOf(index) + 1;
                    var searchTerm = _globalSearchTermRepository.CreateSectionSearchTermFromName(name,
                        globalSearchTermType, ContentType.Core, chapterName, $"/rules/sotg/{path}",
                        instance > 1 ? $"{name} {instance}" : null);

                    _globalSearchTermRepository.SearchTerms.Add(searchTerm);

                    chapterLineIndexesToExclude.Add(index);
                }
            }
        }

        private void HandleTables(List<string> chapterLines, string chapterName, string path, List<int> chapterLineIndexesToExclude)
        {
            var tableLinesIndexes = chapterLines
                .FindAllIndexOf(f => f.StartsWith("|")).Where(f =>
                    chapterLines[f - 1].StartsWith("#") || string.IsNullOrWhiteSpace(chapterLines[f - 1]))
                .Except(chapterLineIndexesToExclude);

            foreach (var tableLinesIndex in tableLinesIndexes)
            {
                var tableHeaderLineIndex = chapterLines.FindLastIndex(tableLinesIndex, f => f.StartsWith("#"));

                if (tableHeaderLineIndex > 0)
                {
                    var tableHeaderLine = chapterLines[tableHeaderLineIndex];

                    var name = tableHeaderLine.RemoveHashtagCharacters().Trim();
                    var repeatIndexes = chapterLines.FindAllIndexOf(f => Regex.IsMatch(f, $@"^#+\s*{name}"));

                    var instance = repeatIndexes.IndexOf(tableHeaderLineIndex) + 1;

                    var searchTerm = _globalSearchTermRepository.CreateSectionSearchTermFromName(name,
                        GlobalSearchTermType.Table, ContentType.Core, chapterName, $"/rules/sotg/{path}",
                        instance > 1 ? $"{name} {instance}" : null);
                    _globalSearchTermRepository.SearchTerms.Add(searchTerm);

                    chapterLineIndexesToExclude.Add(tableHeaderLineIndex);
                }
            }
        }
    }
}
