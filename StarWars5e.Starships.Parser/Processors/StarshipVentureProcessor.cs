using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarWars5e.Models.Starship;

namespace StarWars5e.Starships.Parser.Processors
{
    public class StarshipVentureProcessor : StarshipBaseProcessor<StarshipVenture>
    {
        public override Task<List<StarshipVenture>> FindBlocks(List<string> lines)
        {
            var starshipVenture = new List<StarshipVenture>();

            var venturesSectionStartingIndex = lines.FindIndex(f => f.Contains("## Ventures"));
            var venturesSectionEndIndex = lines.FindIndex(venturesSectionStartingIndex, f => f.Contains("# Chapter 7"));
            var venturesSectionLines = lines.Skip(venturesSectionStartingIndex).Take(venturesSectionEndIndex - venturesSectionStartingIndex).ToList();

            var ventureRulesStartingIndex = venturesSectionLines.FindIndex(f => f.Contains(""));
            var ventureRulesEndIndex = lines.FindIndex(venturesSectionStartingIndex, f => f.Contains("# Chapter 7"));
            var ventureRulesLines = lines.Skip(venturesSectionStartingIndex).Take(venturesSectionEndIndex - venturesSectionStartingIndex).ToList();

            var venturesStartingIndex = venturesSectionLines.FindIndex(f => f.Contains(""));
            var venturesEndIndex = lines.FindIndex(venturesSectionStartingIndex, f => f.Contains("# Chapter 7"));
            var venturesLines = lines.Skip(venturesSectionStartingIndex).Take(venturesSectionEndIndex - venturesSectionStartingIndex).ToList();

            starshipVenture.AddRange(CreateVentures(venturesLines));
            

            return Task.FromResult(starshipVenture);
        }

        private static IEnumerable<StarshipVenture> CreateVentures(List<string> ventureLines)
        {
            var ventureList = new List<StarshipVenture>();
            return ventureList;
        }

        private static void CreateVentureRules(List<string> ventureRulesLines)
        {

        }
    }
}
