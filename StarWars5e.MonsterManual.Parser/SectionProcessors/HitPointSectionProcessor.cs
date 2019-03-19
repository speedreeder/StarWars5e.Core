using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;

namespace StarWars5e.MonsterManual.Parser.SectionProcessors
{
    class HitPointSectionProcessor : IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\s-\s\*\*Hit");


        public Monster Process(Monster monster, string input)
        {
            var toRemove = "> - **Hit Points** ";
            var value = input.Substring(toRemove.Length);
            monster.HitPoints = value;
            return monster;
        }

        public MonsterSections IsMatch(string input)
        {
            return this.regex.IsMatch(input)
                ? MonsterSections.HitPoints
                : MonsterSections.Unknown;
        }
    }
}