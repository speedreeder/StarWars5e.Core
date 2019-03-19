using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Species.Parser.Processors
{
    public class NameProcessor: ISpeciesProcessor {
        private Regex regex = new Regex(@">\s##\s");

        public bool IsMatch(string line)
        {
            return regex.IsMatch(line);
        }

        public SpeciesViewModel ProcessSection(List<string> input, SpeciesViewModel species)
        {
            if (input.Count > 1)
            {
                throw new ArgumentException("Species name should only ever be one line");
            }

            var line = input[0];
            species.SpeciesName = line.Substring("> ## ".Length).Trim();
            return species;
        }

        public SpeciesSection SectionName { get; } = SpeciesSection.SpeciesName;
    }
}