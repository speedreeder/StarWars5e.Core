using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars.Background.Parser.Processors
{
    public class FeatProcessor : IBackgroundProcessor
    {
        private Regex regex = new Regex(@"###\sBackground\sFeat");

        public BackgroundSection Section { get; } = BackgroundSection.Feat;

        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public BackgroundViewModel Process(List<string> lines, BackgroundViewModel background)
        {
            var feat = new DieRollViewModel();
            feat.Name = "Background Feat";
            foreach (var line in lines)
            {
                if (line.StartsWith("#"))
                {
                    continue; // skip the title line
                }
                if (line.StartsWith('|'))
                {
                    
                    var trimmed = line.Trim('|');
                    if (trimmed.StartsWith("d"))
                    {
                        feat.DiceType = ProcessorHelpers.DetermineDiceType(trimmed.Split('|')[0]);
                        continue;
                    }
                    var lineVm = new DieRollValueViewModel();

                    var groups = trimmed.Split('|');
                    lineVm.MinValue = ProcessorHelpers.ExtractNumberFromString(groups[0]);
                    lineVm.MaxValue = lineVm.MinValue;
                    lineVm.Result = groups[1];
                    feat.Values.Add(lineVm);
                }
                else
                {
                    ;
                    background.BackgroundFeatMarkdown = line;
                }
            }

            background.FeatOptions = feat;
            return background;
        }
    }
}