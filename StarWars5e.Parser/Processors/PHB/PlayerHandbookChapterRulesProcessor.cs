using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            chapters.Add(CreateChapterRulesAndSearchTerms(prefaceLines, -2, Localization.PHBPrefaceTitle));

            var whatsDifferentLines = lines.Skip(whatsDifferentStartIndex).Take(chapter0StartIndex - whatsDifferentStartIndex)
                .CleanListOfStrings().ToList();
            chapters.Add(CreateChapterRulesAndSearchTerms(whatsDifferentLines, -1, Localization.PHBWhatsDifferentTitle));

            var introLines = lines.Skip(chapter0StartIndex).Take(chapter1StartIndex - chapter0StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) introLines[2] = introLines[2].Insert(0, "T");
            chapters.Add(CreateChapterRulesAndSearchTerms(introLines, 0, Localization.PHBIntroductionTitle));

            var chapter1Lines = lines.Skip(chapter1StartIndex).Take(chapter2StartIndex - chapter1StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter1Lines[2] = chapter1Lines[2].Insert(0, "Y");
            chapters.Add(CreateChapterRulesAndSearchTerms(chapter1Lines, 1, Localization.PHBChapter1Title, "stepByStep"));

            var chapter2EndIndex = lines.FindIndex(chapter2StartIndex, f => f.StartsWith("___"));
            var chapter2Lines = lines.Skip(chapter2StartIndex).Take(chapter2EndIndex - chapter2StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter2Lines[2] = chapter2Lines[2].Insert(0, "A ");
            chapters.Add(CreateChapterRulesAndSearchTerms(chapter2Lines, 2, Localization.PHBChapter2Title, "species"));

            var chapter3EndIndex = lines.FindIndex(chapter3StartIndex, f => f.StartsWith(Localization.PHBClassesStartLine));
            var chapter3Lines = lines.Skip(chapter3StartIndex).Take(chapter3EndIndex - chapter3StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter3Lines[2] = chapter3Lines[2].Insert(0, "A");
            chapters.Add(CreateChapterRulesAndSearchTerms(chapter3Lines, 3, Localization.PHBChapter3Title, "classes"));

            var chapter4EndIndex = lines.FindIndex(chapter4StartIndex, f => f.StartsWith(Localization.PHBBackgroundsStartLine));
            var chapter4Lines = lines.Skip(chapter4StartIndex).Take(chapter4EndIndex - chapter4StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter4Lines[2] = chapter4Lines[2].Insert(0, "C");
            chapters.Add(CreateChapterRulesAndSearchTerms(chapter4Lines, 4, Localization.PHBChapter4Title, "backgrounds"));

            var chapter5Lines = lines.Skip(chapter5StartIndex).Take(chapter6StartIndex - chapter5StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter5Lines[2] = chapter5Lines[2].Insert(0, "T");
            chapters.Add(CreateChapterRulesAndSearchTerms(chapter5Lines, 5, Localization.PHBChapter5Title, "equipment"));

            var chapter6Lines = lines.Skip(chapter6StartIndex).Take(chapter7StartIndex - chapter6StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter6Lines[2] = chapter6Lines[2].Insert(0, "T");
            chapters.Add(CreateChapterRulesAndSearchTerms(chapter6Lines, 6, Localization.PHBChapter6Title, "customization"));

            var chapter7Lines = lines.Skip(chapter7StartIndex).Take(chapter8StartIndex - chapter7StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter7Lines[2] = chapter7Lines[2].Insert(0, "S");
            chapters.Add(CreateChapterRulesAndSearchTerms(chapter7Lines, 7, Localization.PHBChapter7Title, "abilityScores"));

            var chapter8Lines = lines.Skip(chapter8StartIndex).Take(chapter9StartIndex - chapter8StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter8Lines[2] = chapter8Lines[2].Insert(0, "D");
            chapters.Add(CreateChapterRulesAndSearchTerms(chapter8Lines, 8, Localization.PHBChapter8Title, "adventuring"));

            var chapter9Lines = lines.Skip(chapter9StartIndex).Take(chapter10StartIndex - chapter9StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter9Lines[2] = chapter9Lines[2].Insert(0, "T");
            chapters.Add(CreateChapterRulesAndSearchTerms(chapter9Lines, 9, Localization.PHBChapter9Title, "combat"));

            var chapter10Lines = lines.Skip(chapter10StartIndex).Take(chapter11StartIndex - chapter10StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter10Lines[2] = chapter10Lines[2].Insert(0, "M");
            chapters.Add(CreateChapterRulesAndSearchTerms(chapter10Lines, 10, Localization.PHBChapter10Title, "casting"));

            var appendixALines = lines.Skip(appendixAStartIndex).Take(appendixBStartIndex - appendixAStartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) appendixALines[2] = appendixALines[2].Insert(0, "C");
            chapters.Add(CreateChapterRulesAndSearchTerms(appendixALines, 13, Localization.PHBAppendixATitle, "conditions"));

            var appendixBLines = lines.Skip(appendixBStartIndex).Take(changelogStartIndex - appendixBStartIndex).CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) appendixBLines[2] = appendixBLines[2].Insert(0, "H");
            chapters.Add(CreateChapterRulesAndSearchTerms(appendixBLines, 14, Localization.PHBAppendixBTitle, "variantRules"));

            var changelogLines = lines.Skip(changelogStartIndex).CleanListOfStrings().ToList();
            chapters.Add(CreateChapterRulesAndSearchTerms(changelogLines, 99, Localization.PHBChangelogTitle));

            var phbSections = new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( Localization.PlayersHandbook, GlobalSearchTermType.Book, "/rules/phb"),
                ( Localization.PlayersHandbookChangelog, GlobalSearchTermType.Changelog, "/rules/phb/changelog")
            };

            foreach (var phbSection in phbSections)
            {
                var searchTerm = _globalSearchTermRepository.CreateSearchTerm(phbSection.name,
                    phbSection.globalSearchTermType, ContentType.Core, phbSection.pathOverride);
                _globalSearchTermRepository.SearchTerms.Add(searchTerm);
            }

            return Task.FromResult(chapters);
        }

        private ChapterRules CreateChapterRulesAndSearchTerms(List<string> chapterLines, int chapterNumber, string chapterName, string path = null)
        {
            List<int> headerLineIndexes;
            var chapterLineIndexesToExclude = new List<int>();

            if (chapterNumber == 13)
            {
                headerLineIndexes = chapterLines.FindAllIndexOf(f => f.StartsWith("#### ") || f.StartsWith("# ")).Except(chapterLineIndexesToExclude)
                    .ToList();

                foreach (var headerLineIndex in headerLineIndexes)
                {
                    var line = chapterLines.ElementAt(headerLineIndex);

                    var searchTermType = GlobalSearchTermType.Condition;

                    if (line.Equals(chapterLines.First(f => f.StartsWith("# "))))
                    {
                        searchTermType = GlobalSearchTermType.HandbookChapter;
                    }

                    var repeatIndexes = chapterLines.FindAllIndexOf(f => f == line);
                    var name = line.RemoveHashtagCharacters().Trim();

                    var instance = repeatIndexes.IndexOf(headerLineIndex) + 1;
                    var searchTerm = _globalSearchTermRepository.CreateSectionSearchTermFromName(name,
                        searchTermType, ContentType.Core, chapterName, $"/rules/phb/{path}",
                        instance > 1 ? $"{name} {instance}" : null);
                    _globalSearchTermRepository.SearchTerms.Add(searchTerm);
                }
            }
            else if (chapterNumber == 14)
            {
                headerLineIndexes = chapterLines
                    .FindAllIndexOf(f => f.StartsWith("## ") || f.StartsWith("# ")).Except(chapterLineIndexesToExclude)
                    .ToList();

                foreach (var headerLineIndex in headerLineIndexes)
                {
                    var line = chapterLines.ElementAt(headerLineIndex);

                    var searchTermType = GlobalSearchTermType.VariantRule;

                    if (line.Equals(chapterLines.First(f => f.StartsWith("# "))))
                    {
                        searchTermType = GlobalSearchTermType.HandbookChapter;
                    }

                    var repeatIndexes = chapterLines.FindAllIndexOf(f => f == line);
                    var name = line.RemoveHashtagCharacters().Trim();

                    var instance = repeatIndexes.IndexOf(headerLineIndex) + 1;
                    var searchTerm = _globalSearchTermRepository.CreateSectionSearchTermFromName(name,
                        searchTermType, ContentType.Core, chapterName, $"/rules/phb/{path}",
                        instance > 1 ? $"{name} {instance}" : null);
                    _globalSearchTermRepository.SearchTerms.Add(searchTerm);
                }
            }
            else if (chapterNumber != 99 && chapterNumber != -1 && chapterNumber != -2)
            {
                var weaponPropertiesStartIndex = chapterLines.FindIndex(f => f.StartsWith($"### {Localization.WeaponProperties}"));
                if (weaponPropertiesStartIndex > 0)
                {
                    HandleSearchTermForPropertyOrGear(chapterLines, weaponPropertiesStartIndex, chapterName,
                        path, GlobalSearchTermType.WeaponProperty, chapterLineIndexesToExclude);
                }

                var armorPropertiesStartIndex = chapterLines.FindIndex(f => f.StartsWith($"### {Localization.ArmorProperties}"));
                if (armorPropertiesStartIndex > 0)
                {
                    HandleSearchTermForPropertyOrGear(chapterLines, armorPropertiesStartIndex, chapterName,
                        path, GlobalSearchTermType.ArmorProperty, chapterLineIndexesToExclude);
                }

                var adventuringGearStartIndex = chapterLines.FindIndex(f => f.StartsWith($"## {Localization.AdventuringGear}"));
                if (adventuringGearStartIndex > 0)
                {
                    HandleSearchTermForPropertyOrGear(chapterLines, adventuringGearStartIndex, chapterName,
                        path, GlobalSearchTermType.AdventuringGear, chapterLineIndexesToExclude);
                }

                HandleTables(chapterLines, chapterName, path, chapterLineIndexesToExclude);

                headerLineIndexes = chapterLines
                    .FindAllIndexOf(f => f.StartsWith('#') || f.StartsWith("> #")).Except(chapterLineIndexesToExclude)
                    .ToList();

                foreach (var headerLineIndex in headerLineIndexes)
                {
                    var line = chapterLines.ElementAt(headerLineIndex);

                    var searchTermType = GlobalSearchTermType.PHBRule;

                    if (line.Equals(chapterLines.First(f => f.StartsWith("# "))))
                    {
                        searchTermType = GlobalSearchTermType.HandbookChapter;
                    }

                    if (line.Contains("variant:", StringComparison.InvariantCultureIgnoreCase))
                    {
                        searchTermType = GlobalSearchTermType.VariantRule;
                    }

                    var repeatIndexes = chapterLines.FindAllIndexOf(f => f == line);
                    var name = line.RemoveHashtagCharacters().Trim();

                    var instance = repeatIndexes.IndexOf(headerLineIndex) + 1;
                    var searchTerm = _globalSearchTermRepository.CreateSectionSearchTermFromName(name,
                        searchTermType, ContentType.Core, chapterName, $"/rules/phb/{path}",
                        instance > 1 ? $"{name} {instance}" : null);
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

        private void HandleSearchTermForPropertyOrGear(List<string> chapterLines, int startingIndex, string chapterName, string path, GlobalSearchTermType globalSearchTermType, List<int> chapterLineIndexesToExclude)
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
                        globalSearchTermType, ContentType.Core, chapterName, $"/rules/phb/{path}",
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
                        GlobalSearchTermType.Table, ContentType.Core, chapterName, $"/rules/phb/{path}",
                        instance > 1 ? $"{name} {instance}" : null);
                    _globalSearchTermRepository.SearchTerms.Add(searchTerm);

                    chapterLineIndexesToExclude.Add(tableHeaderLineIndex);
                }
            }
        }
    }
}
