using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars.Species.Parser.Processors
{
    public class AgeProcessor: ISpeciesProcessor
    {
        private Regex regex = new Regex(@"\*\*\*Age.\*\*\*\s");
        private string toRemove = "***Age.*** ";
        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public SpeciesViewModel ProcessSection(List<string> input, SpeciesViewModel species)
        {
            if (input.Count > 1)
            {
                throw new ArgumentException("Age should never be more than a single line");
            }
            species.Age = input[0].Substring(this.toRemove.Length).Trim();
            return species;
        }

        public SpeciesSection SectionName { get; } = SpeciesSection.Age;
    }
}