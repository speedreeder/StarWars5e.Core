using System;
using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;

namespace StarWars.MonsterManual.Parser.SectionProcessors
{
    class AttributeSectionProcessor : IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\|\d");
        public MonsterSections IsMatch(string input)
        {
            return this.regex.IsMatch(input)
                ? MonsterSections.AttributeValues
                : MonsterSections.Unknown;
        }

        public Monster Process(Monster monster, string input)
        {
            input = input.Substring(1).Trim('|');
            var groups = input.Split('|');
            var strength = this.ProcessStatGroup(groups[0]);
            var dexterity = this.ProcessStatGroup(groups[1]);
            var constitution = this.ProcessStatGroup(groups[2]);
            var intelligence = this.ProcessStatGroup(groups[3]);
            var wisdom = this.ProcessStatGroup(groups[4]);
            var charisma = this.ProcessStatGroup(groups[5]);
            monster.Strength = strength.Item1;
            monster.StrengthModifier = strength.Item2;
            monster.Dexterity = dexterity.Item1;
            monster.DexterityModifier = dexterity.Item2;
            monster.Constitution = constitution.Item1;
            monster.ConstitutionModifer = constitution.Item2;
            monster.Intelligence = intelligence.Item1;
            monster.IntelligenceModifier = intelligence.Item2;
            monster.Wisdom = wisdom.Item1;
            monster.WisdomModifier = wisdom.Item2;
            monster.Charisma = charisma.Item1;
            monster.CharismaModifier = charisma.Item2;

            return monster;
        }

        private Tuple<int, int> ProcessStatGroup(string input)
        {
            var groups = input.Trim().Split(' ');
            if (groups.Length == 1)
            {
                groups = input.Split('(');
                groups[1] = $"({groups[1]}";
            }
            var attrValue = groups[0];
            var modifier = this.ExtractValueFromParenthesis(groups[1]);
            return new Tuple<int, int>(int.Parse(attrValue), int.Parse(modifier));
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