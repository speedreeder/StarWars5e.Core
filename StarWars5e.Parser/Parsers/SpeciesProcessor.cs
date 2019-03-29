using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarWars5e.Models.Species;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Parsers
{
    public class SpeciesProcessor : BaseProcessor<Species>
    {
        public override Task<List<Species>> FindBlocks(List<string> lines)
        {
            var species = new List<Species>();

            for (var i = 0; i < lines.Count; i++)
            {
                if (!lines[i].StartsWith("> ## ") && !lines[i].StartsWith(">##")) continue;

                var speciesEndIndex = lines.FindIndex(i, f => f == string.Empty);
                var speciesLines = lines.Skip(i).Take(speciesEndIndex - i).CleanListOfStrings().ToList();


                species.Add(ParseSpecies(speciesLines));
            }

            return Task.FromResult(species);
        }

        private static Species ParseSpecies(List<string> speciesLines)
        {
            var species = new Species();


            return species;
        }
    }
}
