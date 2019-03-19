using System;
using System.Linq;
using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;

namespace StarWars5e.MonsterManual.Parser.SectionProcessors
{
    class SpeedSectionProcessor : IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\s-\s\*\*Speed");
        public MonsterSections IsMatch(string input)
        {
            return this.regex.IsMatch(input)
                ? MonsterSections.Speed
                : MonsterSections.Unknown;
        }

        public Monster Process(Monster monster, string input)
        {
            var toRemove = "> - **Speed** ";
            var value = input.Substring(toRemove.Length);
            var groups = value.Split(',');
            foreach (var group in groups)
            {
                if (group.Contains("swim"))
                {
                    monster.SwimmingSpeed= this.ExtractSpeedFromString(group);
                }
                else if (group.Contains("fly"))
                {
                    monster.FlyingSpeed = this.ExtractSpeedFromString(group);
                }
                else if (group.Contains("climb"))
                {
                    monster.ClimbingSpeed = this.ExtractSpeedFromString(group);
                }
                else
                {
                    monster.WalkingSpeed = this.ExtractSpeedFromString(group);
                }
            }
            return monster;
        }

        private int ExtractSpeedFromString(string input)
        {
            var val = Int32.Parse(new String(input.Where(Char.IsDigit).ToArray()));
            return val;
        }
    }
}