using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Background.Parser.Processors
{
    public class SpecialityProcessor : IBackgroundProcessor
    {
        private Regex regex = new Regex(@"####\sSuggested\sCharacteristics");

        public BackgroundSection Section { get; } = BackgroundSection.Speciality;

        public bool IsMatch(string line)
        {
            return line.Contains("####") && 
                   (line.Contains("Speciality") || line.Contains("Specialty") || line.Contains("Routines") || line.Contains("Clan")
                    || line.Contains("Field of Research") || line.Contains("Schemes")
                    || line.Contains("Why Are You Here?") || line.Contains("Life of Seclusion") || line.Contains("Defining Event")
                    || line.Contains("Clone Specialization") || line.Contains("Criminal Motivation"));
        }

        public BackgroundViewModel Process(List<string> lines, BackgroundViewModel background)
        {
            // this is the personality die roll and the suggestec char markdown line
            var die = new DieRollViewModel();
            foreach (var line in lines)
            {
                if (line.StartsWith('#'))
                {
                    background.SpecialityName = line.Trim().Trim('#').Trim();
                    continue;
                }

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
                    background.SpecialityMarkdown = line;
                }
            }

            background.SpecialityOptions = die;
            return background;
        }
    }
}