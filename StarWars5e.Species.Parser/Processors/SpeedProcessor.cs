using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Species.Parser.Processors
{
    public class SpeedProcessor: ISpeciesProcessor
    {
        private Regex regex = new Regex(@"\*\*\*Speed.\*\*\*");
        private string toRemove = "***Speed.*** ";

        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public SpeciesViewModel ProcessSection(List<string> input, SpeciesViewModel species)
        {
            if (input.Count > 1)
            {
                throw new ArgumentException("Speed Section should never have more than one line");
            }
            var trimmed = input[0].Substring(this.toRemove.Length).Trim();
            species.SpeedMarkdown = trimmed;
            return species;
        }

        public SpeciesSection SectionName { get; } = SpeciesSection.Speed;
    }
}