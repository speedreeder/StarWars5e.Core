using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars.Background.Parser.Processors
{
    public class PersonalityProcessor : IBackgroundProcessor
    {
        private Regex regex = new Regex(@"####\sSuggested\sCharacteristics");

        public BackgroundSection Section { get; } = BackgroundSection.Personality;

        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public BackgroundViewModel Process(List<string> lines, BackgroundViewModel background)
        {
            // this is the personality die roll and the suggestec char markdown line
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
                        try
                        {
                            var val = ProcessorHelpers.ExtractNumberFromString(groups[0]);
                            lineVm.MinValue = val;
                            lineVm.MaxValue = lineVm.MinValue;
                            lineVm.Result = groups[1];
                        }
                        catch (Exception e)
                        {
                            ;

                        }
                        
                        die.Values.Add(lineVm);
                    }
                }
                else
                {
                    background.SuggestedCharacteristicsMarkdown = line;
                }
            }

            background.PersonalityTraits = die;
            return background;
        }
    }
}