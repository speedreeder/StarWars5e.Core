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
            featureOptions.AddRange(parseOptions(page, "Fighter", "Maneuvers"));
            featureOptions.AddRange(parseOptions(page, "Scholar", "Maneuvers"));
            featureOptions.AddRange(parseOptions(page, "Scholar", "Discoveries"));
            featureOptions.AddRange(parseOptions(page, "Sentinel", "Sentinel Ideals"));
            featureOptions.AddRange(parseOptions(page, "Guardian", "Guardian Auras"));
            featureOptions.AddRange(parseOptions(page, "Scout", "Hunter's Prey"));
            featureOptions.AddRange(parseOptions(page, "Consular", "Force-Empowered Casting"));

            return Task.FromResult(featureOptions);
        }

        private List<FeatureOption> parseOptions(List<string> lines, string className, string optionType)
        {
            var featureOptions = new List<FeatureOption>();
            var classStartIndex = lines.FindIndex(0, f => f.Contains("CHAPTER 3 | CLASSES | " + className.ToUpper()));
            var optionsStartIndex = 0;
            if(optionType.Equals("Sentinel Ideals"))
            {
                // why the fuck is FindLastIndex returning -1?
                optionsStartIndex = lines.FindIndex(classStartIndex, f => f.StartsWith("### " + optionType));
                optionsStartIndex = lines.FindIndex(optionsStartIndex+1, f => f.StartsWith("### " + optionType));
            }
            else
            {
                optionsStartIndex = lines.FindIndex(classStartIndex, f => f.StartsWith("### " + optionType));
            }
            var optionsEndIndex = lines.FindIndex(optionsStartIndex + 1, f => f.StartsWith("### "));
            var optionsLines = lines.GetRange(optionsStartIndex, optionsEndIndex - (optionsStartIndex - 1));

            for (var i = 0; i < optionsLines.Count; i++)
            {
                var featureOption = new FeatureOption();
                if (optionsLines[i].StartsWith("#### "))
                {
                    var nextOptionStartIndex = optionsLines.FindIndex(i+1, f => f.StartsWith("#"))-1;
                    var numTextLines = nextOptionStartIndex - i;
                    featureOption.Name = optionsLines[i].Replace("#", "").Trim();
                    featureOption.Text = "";
                    
                    for (var x = 1; x < numTextLines; x++)
                    {
                        featureOption.Text += optionsLines[i + x];
                    }
                    featureOption.Feature = $"Class-{className}-{optionType}" ;
                    featureOption.RowKey = $"{className}-{optionType}-{featureOption.Name}";
                    featureOption.PartitionKey = "Core";
                    featureOptions.Add(featureOption);
                }
            }

            return featureOptions;
        }
    }
}
