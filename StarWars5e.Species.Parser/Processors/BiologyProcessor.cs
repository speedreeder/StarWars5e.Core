using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Species.Parser.Processors
{
    public class BiologyProcessor : ISpeciesProcessor
    {
        private Regex regex = new Regex(@"###\sBiology\sand\s");
        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public SpeciesViewModel ProcessSection(List<string> input, SpeciesViewModel species)
        {
            if (input.Count > 2)
            {
                throw new ArgumentException("Biology and Appearance section should only be a single line (plus title)");
            }

            species.BiologyName = "Biology and Appearance";
            species.BiologyMarkdown = input[1];
            return species;
        }

        public SpeciesSection SectionName { get; } = SpeciesSection.Biology;
    }

    public class DroidAppearanceProcessor: ISpeciesProcessor
    {
        private Regex regex = new Regex(@"###\sAppearance");
        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public SpeciesViewModel ProcessSection(List<string> input, SpeciesViewModel species)
        {
            if (input.Count > 2)
            {
                throw new ArgumentException("Droid Appearancesection should only be a single line (plus title)");
            }

            species.BiologyName = "Appearance";
            species.BiologyMarkdown = input[1];
            return species;
        }

        public SpeciesSection SectionName { get; } = SpeciesSection.DroidAppearance;
    }
}