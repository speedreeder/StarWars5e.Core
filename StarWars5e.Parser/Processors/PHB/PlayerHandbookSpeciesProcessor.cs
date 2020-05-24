using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Species;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Parsers.PHB
{
    public class PlayerHandbookSpeciesProcessor : BaseProcessor<Species>
    {
        public override Task<List<Species>> FindBlocks(List<string> lines)
        {
            var species = new List<Species>();

            for (var i = 0; i < lines.Count; i++)
            {
                if (!lines[i].StartsWith("> ## ") && !lines[i].StartsWith(">## ")) continue;

                var speciesEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("> ## ") || f.StartsWith(">## "));
                var speciesLines = lines.Skip(i).ToList();
                if (speciesEndIndex != -1)
                {
                    speciesLines = lines.Skip(i).Take(speciesEndIndex - i).CleanListOfStrings().ToList();
                }

                species.Add(ExpandedContentSpeciesProcessor.ParseSpecies(speciesLines, ContentType.Core));
            }

            ExpandedContentSpeciesProcessor.MapImageUrls(species);

            return Task.FromResult(species);
        }
    }
}
