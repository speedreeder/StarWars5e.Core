using StarWars5e.Models.Monster;

namespace StarWars5e.MonsterManual.Parser.SectionProcessors
{
    interface IMonsterSectionProcessor
    {

        MonsterSections IsMatch(string input);
        MonsterOld Process(MonsterOld monster, string line);
    }
}