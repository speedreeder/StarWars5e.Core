using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.Utils;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Species.Parser.Processors
{
    public class AttributesProcessor: ISpeciesProcessor
    {
        private Regex regex = new Regex(@"^\*\*\*");

        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public SpeciesViewModel ProcessSection(List<string> input, SpeciesViewModel species)
        {
            var pairs = new List<KvPair>();
            var proficiencies = new List<string>();
            foreach (var line in input)
            {
                var trimmed = line.Trim('*').Trim();
                var groups = trimmed.Split("***");
                if (groups.Length == 2)
                {
                    var pair = new KvPair();
                    pair.Name = groups[0].Trim();
                    pair.Value = groups[1].Trim();
                    pairs.Add(pair);
                    if (pair.Name.ToLower().Contains("darkvision"))
                    {
                        species.Darkvision = true;
                    }
                    this.CheckForProficiencies(pair.Value, proficiencies);
                }
            }

            species.ProficienciesGained = proficiencies;
            species.OtherAttributes = pairs;
            return species;
        }

        private void CheckForProficiencies(string value, List<string> proficiencies)
        {
            if (value.Contains("Technology"))
            {
                proficiencies.Add("Technology");
            }
            if (value.Contains("Animal Handling"))
            {
                proficiencies.Add("Animal Handling");
            }
            if (value.Contains("Lore"))
            {
                proficiencies.Add("Lore");
            }
            if (value.Contains("Intimidation"))
            {
                proficiencies.Add("Intimidation");
            }
            if (value.Contains("Perception"))
            {
                proficiencies.Add("Perception");
            }
            if (value.Contains("Piloting"))
            {
                proficiencies.Add("Piloting");
            }
            if (value.Contains("Sleight of Hand"))
            {
                proficiencies.Add("Sleight of Hand");
            }
            if (value.Contains("Survival"))
            {
                proficiencies.Add("Survival");
            }
            if (value.Contains("Stealth"))
            {
                proficiencies.Add("Stealth");
            }
            if (value.Contains("Performance"))
            {
                proficiencies.Add("Performance");
            }

        }

        public SpeciesSection SectionName { get; } = SpeciesSection.Attributes;
    }
}