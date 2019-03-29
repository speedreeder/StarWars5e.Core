using System;
using System.Linq;
using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;

namespace StarWars5e.MonsterManual.Parser.SectionProcessors
{
    class ArmorClassSectionProcessor : IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\s-\s\*\*Armor");
        public MonsterSections IsMatch(string input)
        {
            return this.regex.IsMatch(input)
                ? MonsterSections.ArmorClass
                : MonsterSections.Unknown;
        }

        public MonsterOld Process(MonsterOld monster, string input)
        {
            var toRemove = "> - **Armor Class** ";
            var val = input.Substring(toRemove.Length);
            var ac = this.ExtractNumberFromString(val);
            var armorType = this.ExtractValueFromParenthesis(val);
            monster.ArmorClass = ac;
            monster.ArmorType = armorType;
            // find the thing we care about....
            return monster;
        }

        private int ExtractNumberFromString(string input)
        {
            var val = input;
            if (input.Contains('('))
            {
                val= input.Split('(')[0];
            }
            var digits = Int32.Parse(new String(val.Where(Char.IsDigit).ToArray()));
            return digits;
        }

        private string ExtractValueFromParenthesis(string input)
        {

            Regex regex = new Regex(@"\((?'ParensValue'.*)\)");
            string parensValue = string.Empty;
            Match match = regex.Match(input);
            if (match.Success)
            {
                parensValue = match.Groups["ParensValue"].Value;
            }

            return parensValue;
        }
    }
}