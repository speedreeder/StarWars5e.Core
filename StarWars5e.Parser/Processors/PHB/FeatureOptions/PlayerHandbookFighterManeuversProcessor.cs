using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models;

namespace StarWars5e.Parser.Processors.PHB.FeatureOptions
{
    public class PlayerHandbookFighterManeuversProcessor : BaseProcessor<FeatureOption>
    {
        public PlayerHandbookFighterManeuversProcessor()
        {
        }

        public override Task<List<FeatureOption>> FindBlocks(List<string> lines)
        {
            var featureOptions = new List<FeatureOption>();
            var fighterStartIndex = lines.FindIndex(0, f => f.Contains("CHAPTER 3 | CLASSES | FIGHTER"));
            var maneuverStartIndex = lines.FindIndex(fighterStartIndex, f => f.StartsWith("### Maneuvers"));
            var maneuverEndIndex = lines.FindIndex(maneuverStartIndex+1, f => f.StartsWith("### "));
            var maneuversLines = lines.GetRange(maneuverStartIndex, maneuverEndIndex - (maneuverStartIndex-1));

            for (var i = 0; i < maneuversLines.Count; i++)
            {
                var maneuver = new FeatureOption();
                if (maneuversLines[i].StartsWith("#### "))
                {
                    maneuver.Name = maneuversLines[i].Replace("#", "").Trim();
                    maneuver.Text = maneuversLines[i + 1];
                    maneuver.Feature = "Class-Fighter-Maneuvers";
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
