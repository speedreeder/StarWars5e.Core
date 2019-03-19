using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Species.Parser.Processors
{
    public class AbilityIncreaseProcessor : ISpeciesProcessor
    {
        private Regex regex = new Regex(@"\*\*\*Ability\sScore\sIncrease.\*\*\*\s");
        private string toRemove = "***Ability Score Increase.*** ";

        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public SpeciesViewModel ProcessSection(List<string> input, SpeciesViewModel species)
        {
            if (input.Count != 2)
            {
                throw new ArgumentException("The Ability Score Section should never be more than two lines (val and title)");
            }

            var trimmed = input[1].Substring(this.toRemove.Length).Trim();
            species.StatisticIncreaseMarkdown = trimmed;
            var increases = new List<StatisticIncrease>();
            var skills = trimmed.ToLower().Split(',');
            foreach (var skill in skills)
            {
                var val = new StatisticIncrease();
                var digit = int.Parse(new String(skill.Where(Char.IsDigit).ToArray()));
                if (skill.Contains("strength"))
                {
                    val.Attribute = CharacterAttribute.Strength;
                    val.Increase = digit;
                }
                else if (skill.Contains("dexterity"))
                {
                    val.Attribute = CharacterAttribute.Dexterity;
                    val.Increase = digit;
                }
                else if (skill.Contains("constitution"))
                {
                    val.Attribute = CharacterAttribute.Constitution;
                    val.Increase = digit;
                }
                else if (skill.Contains("intelligence"))
                {
                    val.Attribute = CharacterAttribute.Intelligence;
                    val.Increase = digit;
                }
                else if (skill.Contains("wisdom"))
                {
                    val.Attribute = CharacterAttribute.Wisdom;
                    val.Increase = digit;
                }
                else if (skill.Contains("charisma"))
                {
                    val.Attribute = CharacterAttribute.Charisma;
                    val.Increase = digit;
                }
                else
                {
                    ;
                }

                if (val.Increase > 0)
                {
                    increases.Add(val);
                }
            }

            species.StatisticIncreases = increases;
            return species;
        }

        public SpeciesSection SectionName { get; } = SpeciesSection.AbilityIncreases;
    }
}