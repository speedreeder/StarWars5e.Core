using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Starship;

namespace StarWars5e.Parser.Parsers
{
    public class StarshipVentureProcessor : StarshipBaseProcessor<StarshipVenture>
    {
        public override Task<List<StarshipVenture>> FindBlocks(List<string> lines)
        {
            var starshipVenture = new List<StarshipVenture>();

            var venturesSectionStartingIndex = lines.FindIndex(f => f.Contains("## Ventures"));
            var venturesSectionEndIndex = lines.FindIndex(venturesSectionStartingIndex, f => f.Contains("# Chapter 7"));
            var venturesSectionLines = lines.Skip(venturesSectionStartingIndex).Take(venturesSectionEndIndex - venturesSectionStartingIndex).ToList();

            var ventureRulesStartingIndex = venturesSectionLines.FindIndex(f => f.Contains("## Ventures"));
            var ventureRulesEndIndex =
                venturesSectionLines.FindIndex(ventureRulesStartingIndex, f => f.StartsWith("### "));

            var venturesLines = venturesSectionLines.Skip(ventureRulesEndIndex).Take(venturesSectionEndIndex - ventureRulesEndIndex).ToList();

            starshipVenture.AddRange(CreateVentures(venturesLines));

            return Task.FromResult(starshipVenture);
        }

        private static IEnumerable<StarshipVenture> CreateVentures(List<string> ventureLines)
        {
            var ventureList = new List<StarshipVenture>();

            for (var i = 0; i < ventureLines.Count; i++)
            {
                if (!ventureLines[i].StartsWith("### ")) continue;

                var endIndex = ventureLines.FindIndex(i + 1, string.IsNullOrWhiteSpace);
                var currentVentureLines = ventureLines.Skip(i).Take((endIndex == -1 ? ventureLines.Count - 1 : endIndex) - i).ToList();

                var venture = new StarshipVenture
                {
                    PartitionKey = ContentType.Base.ToString(),
                    RowKey = ventureLines[i].Substring(ventureLines[i].IndexOf(' ') + 1).Trim(),
                    Name = ventureLines[i].Substring(ventureLines[i].IndexOf(' ') + 1).Trim(),
                    Prerequisites = currentVentureLines.Where(s => s.StartsWith("_prerequisite",
                            StringComparison.InvariantCultureIgnoreCase)).Select(s =>
                            s.Substring(s.IndexOf(' ') + 1).Replace("_", string.Empty).Replace("<br>", string.Empty))
                        .ToList(),
                    Content = string.Join("\r\n",
                        currentVentureLines.Where(s =>
                            !string.IsNullOrWhiteSpace(s) &&
                            !s.StartsWith('/') &&
                            !s.StartsWith('<') &&
                            !s.StartsWith('#') &&
                            !s.StartsWith("_Prerequisite", StringComparison.InvariantCultureIgnoreCase)))
                };

                ventureList.Add(venture);
            }

            return ventureList;
        }
    }
}
