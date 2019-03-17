using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars.Species.Parser.Processors
{
    public class PhysicalProcessor : ISpeciesProcessor
    {
        private Regex regex = new Regex(@">\s####\sPhysical\sCharacteristics");
        private string heightLine = "> |***Height***|&nbsp;&nbsp;|";
        private string weightLine = "> |***Weight***|&nbsp;&nbsp;|";

        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public SpeciesViewModel ProcessSection(List<string> input, SpeciesViewModel species)
        {
            foreach (var line in input)
            {
                if (line.StartsWith(this.heightLine))
                {
                    var groups = line.Substring(heightLine.Length).Split('|');
                    species.MininumHeight = groups[0].Trim().Trim('|').Trim();
                    species.HeightModifier = groups[1].Trim().Trim('|').Trim();
                } else if (line.StartsWith(this.weightLine))
                {
                    var groups = line.Substring(weightLine.Length).Split('|');
                    species.MinimumWeight = groups[0].Trim().Trim('|').Trim();
                    species.WeightModifier = groups[1].Trim().Trim('|').Trim();
                } else if (this.regex.IsMatch(line))
                {
                    continue;
                }
                else
                {
                    ;
                }
            }
            return species;
        }

        public SpeciesSection SectionName { get; } = SpeciesSection.PhysicalCharacteristic;
    }
}