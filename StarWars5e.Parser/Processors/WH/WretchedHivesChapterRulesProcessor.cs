using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            if (Localization.Language == Language.en) chapter0Lines[2] = chapter0Lines[2].Insert(0, "T");

            chapters.Add(CreateWretchedHivesChapterRules(chapter0Lines, 1, Localization.WHChapter0Title,
                "introduction"));

            var chapter1Lines = lines.Skip(chapter1StartIndex).Take(chapter2StartIndex - chapter1StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter1Lines[2] = chapter1Lines[2].Insert(0, "M");
            chapters.Add(CreateWretchedHivesChapterRules(chapter1Lines, 1, Localization.WHChapter1Title,
                "stepByStep"));

            var chapter2Lines = lines.Skip(chapter2StartIndex).Take(chapter3StartIndex - chapter2StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter2Lines[2] = chapter2Lines[2].Insert(0, "A");
            chapters.Add(CreateWretchedHivesChapterRules(chapter2Lines, 2, Localization.WHChapter2Title,
                "downtime"));

            var chapter3Lines = lines.Skip(chapter3StartIndex).Take(chapter5StartIndex - chapter3StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter3Lines[2] = chapter3Lines[2].Insert(0, "A");
            chapters.Add(CreateWretchedHivesChapterRules(chapter3Lines, 3, Localization.WHChapter3Title,
                "factionsAndMembership"));

            var chapter4Lines = lines.Skip(chapter4StartIndex).Take(chapter5StartIndex - chapter4StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter4Lines[2] = chapter4Lines[2].Insert(0, "S");
            chapters.Add(CreateWretchedHivesChapterRules(chapter4Lines, 4, Localization.WHChapter4Title,
                "abilityScores"));

            var chapter5Lines = lines.Skip(chapter5StartIndex).Take(chapter6StartIndex - chapter5StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter5Lines[2] = chapter5Lines[2].Insert(0, "T");
            chapters.Add(CreateWretchedHivesChapterRules(chapter5Lines, 5, Localization.WHChapter5Title,
                "equipment"));

            var chapter6Lines = lines.Skip(chapter6StartIndex).Take(chapter7StartIndex - chapter6StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter6Lines[2] = chapter6Lines[2].Insert(0, "T");
            chapters.Add(CreateWretchedHivesChapterRules(chapter6Lines, 6, Localization.WHChapter6Title,
                "customizationOptions"));

            var chapter7Lines = lines.Skip(chapter7StartIndex).Take(chapter8StartIndex - chapter7StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter7Lines[2] = chapter7Lines[2].Insert(0, "T");
            chapters.Add(CreateWretchedHivesChapterRules(chapter7Lines, 7, Localization.WHChapter7Title,
                "enhancedItems"));

            var chapter8Lines = lines.Skip(chapter8StartIndex).Take(appendixAStartIndex - chapter8StartIndex)
                .CleanListOfStrings().ToList();
            if (Localization.Language == Language.en) chapter8Lines[2] = chapter8Lines[2].Insert(0, "T");
            chapters.Add(CreateWretchedHivesChapterRules(chapter8Lines, 8, Localization.WHChapter8Title,
                "toolProficiencies"));

            var changelogLines = lines.Skip(changelogStartIndex).CleanListOfStrings().ToList();
            chapters.Add(CreateWretchedHivesChapterRules(changelogLines, 99, Localization.WHChangelogTitle));

            var whSections =
                new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
                {
                    (Localization.WretchedHives, GlobalSearchTermType.Book, "/rules/wh"),
                    (Localization.WretchedHivesChangelog, GlobalSearchTermType.Changelog, "/rules/wh/changelog")
                };

            foreach (var whSection in whSections)
            {
                var searchTerm = _globalSearchTermRepository.CreateSearchTerm(whSection.name,
                    whSection.globalSearchTermType, ContentType.Core, whSection.pathOverride);
                _globalSearchTermRepository.SearchTerms.Add(searchTerm);
            }

            return Task.FromResult(chapters);
        }

        private ChapterRules CreateWretchedHivesChapterRules(List<string> chapterLines, int chapterNumber,
            string chapterName, string path = null)
        {
            var chapterLineIndexesToExclude = new List<int>();

            if (chapterNumber != 99)
            {
                var weaponPropertiesStartIndex = chapterLines.FindIndex(f => f.StartsWith($"### {Localization.WeaponProperties}"));
                if (weaponPropertiesStartIndex > 0)
                {
                    HandleSearchTermForProperty(chapterLines, weaponPropertiesStartIndex, chapterName,
                        path, GlobalSearchTermType.WeaponProperty, chapterLineIndexesToExclude);
                }

                var armorPropertiesStartIndex = chapterLines.FindIndex(f => f.StartsWith($"### {Localization.ArmorProperties}"));
                if (armorPropertiesStartIndex > 0)
                {
                    HandleSearchTermForProperty(chapterLines, armorPropertiesStartIndex, chapterName,
                        path, GlobalSearchTermType.ArmorProperty, chapterLineIndexesToExclude);
                }

                HandleTables(chapterLines, chapterName, path, chapterLineIndexesToExclude);

                var headerLineIndexes = chapterLines
                    .FindAllIndexOf(f => f.StartsWith('#') || f.StartsWith("> #")).Except(chapterLineIndexesToExclude)
                    .ToList();

                foreach (var headerLineIndex in headerLineIndexes)
                {
                    var line = chapterLines.ElementAt(headerLineIndex);

                    var searchTermType = GlobalSearchTermType.WHRule;

                    if (line.Equals(chapterLines.First(f => f.StartsWith("# "))))
                    {
                        searchTermType = GlobalSearchTermType.WretchedHivesChapter;
                    }

                    if (line.Contains("variant:", StringComparison.InvariantCultureIgnoreCase))
                    {
                        searchTermType = GlobalSearchTermType.VariantRule;
                    }

                    var name = line.RemoveHashtagCharacters().Trim();
                    var repeatIndexes = chapterLines.FindAllIndexOf(f => Regex.IsMatch(f, $@"^#+\s*{name}"));

                    var instance = repeatIndexes.IndexOf(headerLineIndex) + 1;
                    var searchTerm = _globalSearchTermRepository.CreateSectionSearchTermFromName(name,
                        searchTermType, ContentType.Core, chapterName, $"/rules/wh/{path}",
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

        private void HandleSearchTermForProperty(List<string> chapterLines, int startingIndex, string chapterName, string path, GlobalSearchTermType globalSearchTermType, List<int> chapterLineIndexesToExclude)
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
                        globalSearchTermType, ContentType.Core, chapterName, $"/rules/wh/{path}",
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
                        GlobalSearchTermType.Table, ContentType.Core, chapterName, $"/rules/wh/{path}",
                        instance > 1 ? $"{name} {instance}" : null);
                    _globalSearchTermRepository.SearchTerms.Add(searchTerm);

                    chapterLineIndexesToExclude.Add(tableHeaderLineIndex);
                }
            }
        }
    }
}
