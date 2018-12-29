using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars.Species.Parser.Processors
{
    public class VisualProcessor: ISpeciesProcessor {
        private Regex regex = new Regex(@">\s####\sVisual\sCharacteristics");
        private string eyeLine = "> |***Eye Color***|&nbsp;&nbsp;|";
        private string hairLine = "> |***Hair Color***|&nbsp;&nbsp;|";
        private string skinLine = "> |***Skin Color***|&nbsp;&nbsp;|";
        private string colorSchemeLine = "> |***Color Scheme***|&nbsp;&nbsp;|";
        private string distinctionLine = "> |***Distinctions***|&nbsp;&nbsp;|";
        private string distinctionLineDroid = "> |***Distinctions***&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;|";

        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public SpeciesViewModel ProcessSection(List<string> input, SpeciesViewModel species)
        {
            foreach (var line in input)
            {
                if (line.StartsWith(this.eyeLine))
                {
                    species.EyeColors = this.BuildStringListFromLine(line.Substring(this.eyeLine.Length));
                } else if (line.StartsWith(this.hairLine))
                {
                    species.HairColors = this.BuildStringListFromLine(line.Substring(this.hairLine.Length));
                } else if (line.StartsWith(this.skinLine))
                {
                    species.SkinColors = this.BuildStringListFromLine(line.Substring(this.skinLine.Length));
                } else if (line.StartsWith(distinctionLine) || line.StartsWith(this.distinctionLineDroid))
                {
                    species.Distinctions = this.BuildDistinctionsLine(line.Substring(this.distinctionLine.Length));
                }
                else if (line.StartsWith(this.colorSchemeLine))
                {
                    var langs = line.Substring(this.colorSchemeLine.Length).Trim().Trim('|').Trim();
                    species.ColorSchemes = langs.Split(',').ToList();
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

        private IEnumerable<string> BuildStringListFromLine(string input)
        {
            var trimmed = input.Trim().Trim('|').Trim();

            var vals = trimmed.Split(',').ToList().Select(e => e.Trim());
            return vals;
        }

        private string BuildDistinctionsLine(string input)
        {
            return input.Trim().Trim('|').Trim();
        }

        public SpeciesSection SectionName { get; } = SpeciesSection.VisualCharacteristic;
    }
}