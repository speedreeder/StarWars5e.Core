using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;

namespace StarWars.MonsterManual.Parser.SectionProcessors
{
    class AttributeHeaderSectionProcessor : IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\|S");
        public MonsterSections IsMatch(string input)
        {
            return this.regex.IsMatch(input)
                ? MonsterSections.AttributeHeader
                : MonsterSections.Unknown;
        }

        public Monster Process(Monster monster, string input)
        {
            // yay? Good job, we found something that doesn't matter
            return monster;
        }
    }
}