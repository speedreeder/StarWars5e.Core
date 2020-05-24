using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models.Background;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Processors.PHB
{
    public class PlayerHandbookBackgroundsProcessor : BaseProcessor<Background>
    {
        public override Task<List<Background>> FindBlocks(List<string> lines)
        {
            var backgrounds = new List<Background>();

            var backgroundsStart =
                lines.FindIndex(f => f.Equals(Localization.StartingBackground, StringComparison.InvariantCultureIgnoreCase));

            for (var i = backgroundsStart; i < lines.Count; i++)
            {
                if (!lines[i].StartsWith("## ") && !lines[i].StartsWith("## ") || lines[i].Contains(Localization.Changelog)) continue;

                var backgroundEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("## "));
                var backgroundLines = lines.Skip(i).ToList();
                if (backgroundEndIndex != -1)
                {
                    backgroundLines = lines.Skip(i).Take(backgroundEndIndex - i).CleanListOfStrings().ToList();
                }

                backgrounds.Add(ExpandedContentBackgroundProcessor.ParseBackground(backgroundLines, ContentType.Core));
            }

            return Task.FromResult(backgrounds);
        }
    }
}
