using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars.Species.Parser.Processors
{
    public class SocioCulturalProcess : ISpeciesProcessor
    {
        private Regex regex = new Regex(@">\s####\sSociocultural\sCharacteristics");
        private string homeworldLine = "> |***Homeworld***|&nbsp;&nbsp;|";
        private string languageLine = "> |***Language***|&nbsp;&nbsp;|";

        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public SpeciesViewModel ProcessSection(List<string> input, SpeciesViewModel species)
        {
            foreach(var line in input)
            {
                if (line.StartsWith(this.homeworldLine))
                {
                    species.Homeworld = line.Substring(homeworldLine.Length).Trim().Trim('|').Trim();
                }
                else if (line.StartsWith(this.languageLine))
                {
                    var langs = line.Substring(languageLine.Length).Trim().Trim('|').Trim();
                    species.DefaultLanguages = langs.Split(',').ToList();
                }
                else if (this.regex.IsMatch(line))
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

        public SpeciesSection SectionName { get; } = SpeciesSection.SocioCultural;
    }

    public class CreationCharacteristics : ISpeciesProcessor
    {
        private Regex regex = new Regex(@">\s####\sCreation\sCharacteristics");
        private string mfgLine = "> |***Manufacturer***|";
        private string primaryLangLine = "> |***Primary Language***|";

        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public SpeciesViewModel ProcessSection(List<string> input, SpeciesViewModel species)
        {
            foreach (var line in input)
            {
                if (line.StartsWith(this.mfgLine))
                {
                    var mfgs = line.Substring(primaryLangLine.Length).Trim().Trim('|').Trim();
                    species.Manufacturers = mfgs.Split(',').ToList();
                }
                else if (line.StartsWith(this.primaryLangLine))
                {
                    var langs = line.Substring(primaryLangLine.Length).Trim().Trim('|').Trim();
                    species.DefaultLanguages = langs.Split(',').ToList();
                }
                else if (this.regex.IsMatch(line))
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

        public SpeciesSection SectionName { get; } = SpeciesSection.DroidCreationSection;
    }
}