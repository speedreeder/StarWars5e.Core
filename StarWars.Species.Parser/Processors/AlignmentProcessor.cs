using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars.Species.Parser.Processors
{
    public class AlignmentProcessor: ISpeciesProcessor
    {
        private Regex regex = new Regex(@"\*\*\*Alignment.\*\*\*");
        private string toRemove = "***Alignment.*** ";

        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public SpeciesViewModel ProcessSection(List<string> input, SpeciesViewModel species)
        {
            if (input.Count > 1)
            {
                throw new ArgumentException("Alignment should never be more than a single line");
            }
            species.Alignment = input[0].Substring(this.toRemove.Length).Trim();
            return species;
        }

        public SpeciesSection SectionName { get; } = SpeciesSection.Alignment;
    }
}