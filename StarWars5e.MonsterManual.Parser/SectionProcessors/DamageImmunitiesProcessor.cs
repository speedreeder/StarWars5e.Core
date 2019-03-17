using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;

namespace StarWars.MonsterManual.Parser.SectionProcessors
{
    class DamageImmunitiesProcessor : IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\s-\s\*\*Damage Immunities\*\*");

        public Monster Process(Monster monster, string input)
        {
            var resistList = new List<string>();

            var toRemove = "> - **Damage Immunities** ";
            var skillsVal = input.Substring(toRemove.Length).Trim();
            var resists = skillsVal.Split(',');
            foreach (var item in resists)
            {
                resistList.Add(item.Trim());
            }


            monster.DamageImmunities = resistList;
            return monster;
        }

        public MonsterSections IsMatch(string input)
        {
            return this.regex.IsMatch(input)
                ? MonsterSections.DamageVulnerabilities
                : MonsterSections.Unknown;
        }
    }
}