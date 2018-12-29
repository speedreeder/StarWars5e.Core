using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars.Background.Parser.Processors
{
    public class BondProcessor : IBackgroundProcessor
    {
        private Regex regex = new Regex(@"\|d6\|Bond\|");

        public BackgroundSection Section { get; } = BackgroundSection.Bond;

        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public BackgroundViewModel Process(List<string> lines, BackgroundViewModel background)
        {
            var die = new DieRollViewModel();
            foreach (var line in lines)
            {
                if (line.StartsWith('|'))
                {
                    var trimmed = line.Trim('|').Trim();
                    if (trimmed.StartsWith('d'))
                    {
                        // header...
                        var titleGroups = line.Trim('|').Split('|');
                        die.Name = titleGroups[1].Trim();
                        die.DiceType = ProcessorHelpers.DetermineDiceType(titleGroups[0].Trim());
                    }
                    else
                    {
                        // value...
                        var lineVm = new DieRollValueViewModel();

                        var groups = trimmed.Split('|');
                        lineVm.MinValue = ProcessorHelpers.ExtractNumberFromString(groups[0]);
                        lineVm.MaxValue = lineVm.MinValue;
                        lineVm.Result = groups[1];
                        die.Values.Add(lineVm);
                    }
                }
                else
                {
                    background.SuggestedCharacteristicsMarkdown = line;
                }
            }

            background.Bonds = die;
            return background;
        }
    }
}