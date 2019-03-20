using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models.Starship;
using StarWars5e.Models.Utils;

namespace StarWars5e.Starships.Parser.Processors
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
            var appendixAStartIndex = lines.FindIndex(f => f == "# Appendix A: Conditions");

            chapters.Add(CreateStarshipChapterRules(
                lines.Skip(chapter0StartIndex).Take(chapter1StartIndex - chapter0StartIndex).CleanListOfStrings(), 0,
                "Introduction"));
            chapters.Add(CreateStarshipChapterRules(
                lines.Skip(chapter1StartIndex).Take(chapter2StartIndex - chapter1StartIndex).CleanListOfStrings(), 0,
                "Step-By-Step Starships"));
            chapters.Add(CreateStarshipChapterRules(
                lines.Skip(chapter7StartIndex).Take(chapter8StartIndex - chapter7StartIndex).CleanListOfStrings(), 7,
                "Using Ability Scores"));
            chapters.Add(CreateStarshipChapterRules(
                lines.Skip(chapter8StartIndex).Take(chapter9StartIndex - chapter8StartIndex).CleanListOfStrings(), 8,
                "Adventuring"));
            chapters.Add(CreateStarshipChapterRules(
                lines.Skip(chapter9StartIndex).Take(appendixAStartIndex - chapter9StartIndex).CleanListOfStrings(), 9,
                "Combat"));

            return Task.FromResult(chapters);
        }

        private static StarshipChapterRules CreateStarshipChapterRules(IEnumerable<string> chapterLines, int chapterNumber, string chapterName)
        {
            var chapter = new StarshipChapterRules
            {
                ChapterNumber = chapterNumber,
                ChapterName = chapterName,
                ContentMarkdown = string.Join("\r\n", chapterLines)

            };
            return chapter;
        }
    }
}
