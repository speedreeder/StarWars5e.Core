using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.Enums;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Species.Parser.Processors
{
    public class SizeProcessor: ISpeciesProcessor
    {
        private Regex regex = new Regex(@"\*\*\*Size.\*\*\*");
        private string toRemove = "***Size.*** ";
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

            var trimmed = input[0].Substring(this.toRemove.Length).Trim();
            species.SizeMarkdown = trimmed;
            if (trimmed.Contains("Small"))
            {
                species.Size = StarshipSize.Small;
            }
            else if (trimmed.Contains("Medium"))
            {
                species.Size = StarshipSize.Medium;
            }
            else if (trimmed.Contains("Large"))
            {
                species.Size = StarshipSize.Large;
            }
            else
            {
                ;
            }
            return species;
        }

        public SpeciesSection SectionName { get; } = SpeciesSection.Size;
    }
}