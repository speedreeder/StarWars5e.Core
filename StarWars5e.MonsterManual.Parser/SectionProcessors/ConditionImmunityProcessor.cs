using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;

namespace StarWars5e.MonsterManual.Parser.SectionProcessors
{
    class ConditionImmunityProcessor: IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\s-\s\*\*Condition Immunities\*\*");

        public Monster Process(Monster monster, string input)
        {
            var savingThrowList = new List<string>();

            var toRemove = "> - **Condition Immunities** ";
            var skillsVal = input.Substring(toRemove.Length).Trim();
            var immunities = skillsVal.Split(',');
            foreach (var item in immunities)
            {
                savingThrowList.Add(item.Trim());
            }


            monster.ConditionImmunities = savingThrowList;
            return monster;
        }

        public MonsterSections IsMatch(string input)
        {
            return this.regex.IsMatch(input)
                ? MonsterSections.ConditionImmunities
                : MonsterSections.Unknown;
        }
    }
}