using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;

namespace StarWars.MonsterManual.Parser.SectionProcessors
{
    class NameSectionProcessor : IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\s##\s");

        public Monster Process(Monster monster, string input)
        {
            // find the thing we care about....
            var name = input.Substring(5);
            monster.Name = name;
            return monster;
        }

        public MonsterSections IsMatch(string input)
        {
            return this.regex.IsMatch(input) 
                ? MonsterSections.Name 
                : MonsterSections.Unknown;
        }
    }
}