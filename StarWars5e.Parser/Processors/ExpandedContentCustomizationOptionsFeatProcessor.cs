using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;
using StarWars5e.Parser.Processors.PHB;

namespace StarWars5e.Parser.Processors
{
    public class ExpandedContentCustomizationOptionsFeatProcessor : BaseProcessor<Feat>
    {
        public override Task<List<Feat>> FindBlocks(List<string> lines)
        {
            var feats = new List<Feat>();
            lines = lines.CleanListOfStrings().ToList();

            var featsStart = lines.FindIndex(f => f.StartsWith("## Feats"));
            var featsEndIndex = lines.FindIndex(featsStart + 1, f => f.StartsWith("## ") && !f.Contains("feat", StringComparison.InvariantCultureIgnoreCase));

            for (var i = 0; i < (featsEndIndex != -1 ? featsEndIndex : lines.Count); i++)
            {
                if (!lines[i].StartsWith("### ")) continue;

                var featStartIndex = i;
                var featEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("##"));

                var featLines = lines.Skip(featStartIndex);
                if (featEndIndex != -1)
                {
                    featLines = lines.Skip(featStartIndex).Take(featEndIndex - featStartIndex);
                }

                var playerHandbookFeatProcessor = new PlayerHandbookFeatProcessor(Localization);
                var feat = playerHandbookFeatProcessor.ParseFeat(featLines.ToList(), ContentType.ExpandedContent);
                feats.Add(feat);
            }

            return Task.FromResult(feats);
        }
    }
}
