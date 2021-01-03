using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models;

namespace StarWars5e.Parser.Processors.PHB.FeatureOptions
{
    class PlayerHandbookScholarManeuversProcessor : BaseProcessor<FeatureOption>
    {
        public override Task<List<FeatureOption>> FindBlocks(List<string> lines)
        {
            var featureOptions = new List<FeatureOption>();
            var scholarStartIndex = lines.FindIndex(0, f => f.Contains("CHAPTER 3 | CLASSES | SCHOLAR"));
            var maneuverStartIndex = lines.FindIndex(scholarStartIndex, f => f.StartsWith("### Maneuvers"));
            var maneuverEndIndex = lines.FindIndex(maneuverStartIndex + 1, f => f.StartsWith("### "));
            var maneuversLines = lines.GetRange(maneuverStartIndex, maneuverEndIndex - (maneuverStartIndex - 1));

            for (var i = 0; i < maneuversLines.Count; i++)
            {
                var maneuver = new FeatureOption();
                if (maneuversLines[i].StartsWith("#### "))
                {
                    maneuver.Name = maneuversLines[i].Replace("#", "").Trim();
                    maneuver.Text = maneuversLines[i + 1];
                    maneuver.Feature = "Class-Scholar-Maneuvers";
                    maneuver.RowKey = "Maneuvers-" + maneuver.Name;
                    maneuver.PartitionKey = "Core";
                    i++;
                    featureOptions.Add(maneuver);
                }
            }

            return Task.FromResult(featureOptions);
        }
    }
}
