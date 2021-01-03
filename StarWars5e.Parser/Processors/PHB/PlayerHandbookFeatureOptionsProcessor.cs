using StarWars5e.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StarWars5e.Parser.Processors.PHB
{
    class PlayerHandbookFeatureOptionsProcessor : BaseProcessor<FeatureOption>
    {
        public override Task<List<FeatureOption>> FindBlocks(List<string> page)
        {
            List<FeatureOption> featureOptions = new List<FeatureOption>();
            featureOptions.AddRange(parseManeuvers(page, "Fighter"));
            featureOptions.AddRange(parseManeuvers(page, "Scholar"));


            return Task.FromResult(featureOptions);
        }

        private List<FeatureOption> parseManeuvers(List<string> lines, string className)
        {
            var featureOptions = new List<FeatureOption>();
            var fighterStartIndex = lines.FindIndex(0, f => f.Contains("CHAPTER 3 | CLASSES | " + className.ToUpper()));
            var maneuverStartIndex = lines.FindIndex(fighterStartIndex, f => f.StartsWith("### Maneuvers"));
            var maneuverEndIndex = lines.FindIndex(maneuverStartIndex + 1, f => f.StartsWith("### "));
            var maneuversLines = lines.GetRange(maneuverStartIndex, maneuverEndIndex - (maneuverStartIndex - 1));

            for (var i = 0; i < maneuversLines.Count; i++)
            {
                var maneuver = new FeatureOption();
                if (maneuversLines[i].StartsWith("#### "))
                {
                    maneuver.Name = maneuversLines[i].Replace("#", "").Trim();
                    maneuver.Text = maneuversLines[i + 1];
                    maneuver.Feature = "Class-"+className+"-Maneuvers";
                    maneuver.RowKey = className+"-Maneuvers-" + maneuver.Name;
                    maneuver.PartitionKey = "Core";
                    i++;
                    featureOptions.Add(maneuver);
                    Console.WriteLine(maneuver.RowKey);
                }
            }

            return featureOptions;
        }
    }
}
