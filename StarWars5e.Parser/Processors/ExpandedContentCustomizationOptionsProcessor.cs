using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;
using StarWars5e.Parser.Parsers.PHB;

namespace StarWars5e.Parser.Parsers
{
    public class ExpandedContentCustomizationOptionsProcessor : BaseProcessor<Feat>
    {
        public override Task<List<Feat>> FindBlocks(List<string> lines)
        {
            var feats = new List<Feat>();
            lines = lines.CleanListOfStrings().ToList();

            for (var i = 0; i < lines.Count; i++)
            {
                if (!lines[i].StartsWith("### ")) continue;

                var featStartIndex = i;
                var featEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("### "));

                var featLines = lines.Skip(featStartIndex);
                if (featEndIndex != -1)
                {
                    featLines = lines.Skip(featStartIndex).Take(featEndIndex - featStartIndex);
                }

                var feat = PlayerHandbookFeatProcessor.ParseFeat(featLines.ToList(), ContentType.ExpandedContent);
                feats.Add(feat);
            }

            return Task.FromResult(feats);
        }
    }
}
