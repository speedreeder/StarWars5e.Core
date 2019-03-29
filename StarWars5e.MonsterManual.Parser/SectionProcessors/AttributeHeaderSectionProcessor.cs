using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;

namespace StarWars5e.MonsterManual.Parser.SectionProcessors
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

        public MonsterOld Process(MonsterOld monster, string input)
        {
            // yay? Good job, we found something that doesn't matter
            return monster;
        }
    }
}