using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;
using StarWars5e.Models.Utils;

namespace StarWars.MonsterManual.Parser.SectionProcessors
{
    class SkillsSectionProcessor : IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\s-\s\*\*Skills\*\*");

        public Monster Process(Monster monster, string input)
        {
            var skillList = new List<KvPair>();

            var toRemove = "> - **Skills** ";
            var skillsVal = input.Substring(toRemove.Length).Trim();
            var skills = skillsVal.Split(',');
            foreach (var skill in skills)
            {
                skillList.Add(this.ProcessSkill(skill));
            }


            monster.Skills = skillList;
            return monster;
        }

        private KvPair ProcessSkill(string input)
        {
            input = input.Trim();
            var groups = input.Split(' ');
            var skill = new KvPair
            {
                Name = groups[0],
                Value = groups[1]
            };
            return skill;
        }

        public MonsterSections IsMatch(string input)
        {
            return this.regex.IsMatch(input)
                ? MonsterSections.Skills
                : MonsterSections.Unknown;
        }
    }
}