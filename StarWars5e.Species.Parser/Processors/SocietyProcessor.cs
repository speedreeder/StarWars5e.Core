using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars.Species.Parser.Processors
{
    public class SocietyProcessor: ISpeciesProcessor
    {

        private Regex regex = new Regex(@"###\sSociety\sand\s");
        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public SpeciesViewModel ProcessSection(List<string> input, SpeciesViewModel species)
        {
            if (input.Count > 2)
            {
                throw new ArgumentException("Society and Culture section should only be a single line (plus title)");
            }
            species.SocietyMarkdown = input[1];
            return species;
        }

        public SpeciesSection SectionName { get; } = SpeciesSection.Society;
    }

    public class UtilityProcessor: ISpeciesProcessor
    {

        private Regex regex = new Regex(@"###\sUtility");
        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public SpeciesViewModel ProcessSection(List<string> input, SpeciesViewModel species)
        {
            if (input.Count > 2)
            {
                throw new ArgumentException("(Droid) Utility section should only be a single line (plus title)");
            }
            species.UtilityMarkdown = input[1];
            return species;
        }

        public SpeciesSection SectionName { get; } = SpeciesSection.UtilitySection;
    }

    public class DroidNoteProcessor: ISpeciesProcessor
    {

        private Regex regex = new Regex(@"####\sPlayers\sas\sDroids");
        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public SpeciesViewModel ProcessSection(List<string> input, SpeciesViewModel species)
        {
            if (input.Count != 2)
            {
                throw new ArgumentException("GM Note section should only be a single line (plus title)");
            }

            species.GmNoteName = "Players as Droids";
            species.GmNote = input[1].Trim().Trim('>').Trim();
            return species;
        }

        public SpeciesSection SectionName { get; } = SpeciesSection.DroidNoteSection;
    }
}