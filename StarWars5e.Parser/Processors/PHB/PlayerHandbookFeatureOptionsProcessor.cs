using StarWars5e.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarWars5e.Parser.Processors.PHB
{
    public class PlayerHandbookFeatureOptionsProcessor : BaseProcessor<FeatureOption>
    {
        public override Task<List<FeatureOption>> FindBlocks(List<string> page)
        {
            List<FeatureOption> featureOptions = new List<FeatureOption>();
            featureOptions.AddRange(ParseOptions(page, "Fighter", "Maneuvers"));
            featureOptions.AddRange(ParseOptions(page, "Scholar", "Maneuvers"));
            featureOptions.AddRange(ParseOptions(page, "Scholar", "Discoveries"));
            featureOptions.AddRange(ParseOptions(page, "Sentinel", "Sentinel Ideals"));
            featureOptions.AddRange(ParseOptions(page, "Guardian", "Guardian Auras"));
            featureOptions.AddRange(ParseOptions(page, "Scout", "Hunter's Prey"));
            featureOptions.AddRange(ParseOptions(page, "Consular", "Force-Empowered Casting"));
            featureOptions.AddRange(ParseOptions(page, "Engineer", "Armormech Modifications"));
            featureOptions.AddRange(ParseOptions(page, "Engineer", "Armstech Modifications"));
            featureOptions.AddRange(ParseOptions(page, "Engineer", "Gadgeteer Contraptions"));
          //featureOptions.AddRange(parseOptions(page, "Engineer", "Unstable Modifications"));

            return Task.FromResult(featureOptions);
        }

        private List<FeatureOption> ParseOptions(List<string> lines, string className, string optionType)
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

                    var textLines = new List<string>();
                    for (var x = 1; x < numTextLines; x++)
                    {
                        if (!optionsLines[i + x].StartsWith("_") && !optionsLines[i + x].StartsWith("<") && !optionsLines[i + x].StartsWith("\\"))
                        {
                            textLines.Add(optionsLines[i + x]);
                        }
                    }
                    featureOption.Text = string.Join("\r\n", textLines);

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
