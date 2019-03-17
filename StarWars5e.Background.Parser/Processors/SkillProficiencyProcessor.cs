using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars.Background.Parser.Processors
{
    public class SkillProficiencyProcessor : IBackgroundProcessor
    {
        private Regex regex = new Regex(@"-\s\*\*Skill\sProficiencies:\*\*");
        private string toRemove = "- **Skill Proficiencies:** ";
        public BackgroundSection Section { get; } = BackgroundSection.SkillProficiency;

        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public BackgroundViewModel Process(List<string> lines, BackgroundViewModel background)
        {
            if (lines.Count > 1)
            {
                throw new ArgumentException("The skill proficiency section should only have a single line");
            }

            var trimmed = lines[0].Substring(this.toRemove.Length).Trim();
            background.ProficiencyMarkdown = trimmed;
            background.ProficiencyCount = this.DetermineNumberToChoose(trimmed);
            background.ProficiencyOptions = this.BuildProficiencyOptions(trimmed);
            return background;
        }

        private List<string> BuildProficiencyOptions(string value)
        {
            var proficiencies = new List<string>();
            if (value.Contains("Acrobatics"))
            {
                proficiencies.Add("Acrobatics");
            }
            if (value.Contains("Animal Handling"))
            {
                proficiencies.Add("Animal Handling");
            }
            if (value.Contains("Athletics"))
            {
                proficiencies.Add("Athletics");
            }
            if (value.Contains("Deception"))
            {
                proficiencies.Add("Deception");
            }
            if (value.Contains("Investigation"))
            {
                proficiencies.Add("Investigation");
            }
            if (value.Contains("Insight"))
            {
                proficiencies.Add("Insight");
            }
            if (value.Contains("Intimidation"))
            {
                proficiencies.Add("Intimidation");
            }
            if (value.Contains("Lore"))
            {
                proficiencies.Add("Lore");
            }
            if (value.Contains("Medicine"))
            {
                proficiencies.Add("Medicine");
            }
            if (value.Contains("Nature"))
            {
                proficiencies.Add("Nature");
            }
            if (value.Contains("Perception"))
            {
                proficiencies.Add("Perception");
            }
            if (value.Contains("Piloting"))
            {
                proficiencies.Add("Piloting");
            }
            if (value.Contains("Persuasion"))
            {
                proficiencies.Add("Persuasion");
            }
            if (value.Contains("Performance"))
            {
                proficiencies.Add("Performance");
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
            if (value.Contains("Technology"))
            {
                proficiencies.Add("Technology");
            }
            return proficiencies;
        }

        private int DetermineNumberToChoose(string line)
        {
            if (line.Contains("two"))
            {
                return 2;
            }

            if (line.Contains("four"))
            {
                return 4;
            }

            if (line.Contains("three"))
            {
                return 3;
            }
            return 0;
        }
    }
}