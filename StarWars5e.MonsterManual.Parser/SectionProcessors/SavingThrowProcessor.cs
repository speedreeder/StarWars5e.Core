using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;
using StarWars5e.Models.Utils;

namespace StarWars5e.MonsterManual.Parser.SectionProcessors
{
    class SavingThrowProcessor : IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\s-\s\*\*Saving Throws\*\*");

        public Monster Process(Monster monster, string input)
        {
            var savingThrowList = new List<KvPair>();

            var toRemove = "> - **Saving Throws** ";
            var skillsVal = input.Substring(toRemove.Length).Trim();
            var skills = skillsVal.Split(',');
            foreach (var skill in skills)
            {
                savingThrowList.Add(this.ProcessSavingThrow(skill));
            }


            monster.SavingThrows = savingThrowList;
            return monster;
        }

        private KvPair ProcessSavingThrow(string input)
        {
            // TODO: Should we change the short names to long???
            input = input.Trim();
            var groups = input.Split(' ');
            if (groups.Length == 1)
            {
                groups = groups[0].Split('+');
                groups[1] = $"+{groups[1]}";
            }
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
                ? MonsterSections.SavingThrows
                : MonsterSections.Unknown;
        }
    }
}