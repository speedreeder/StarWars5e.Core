using System.Collections.Generic;
using StarWars5e.Models.Enums;
using System.Collections.Generic;

namespace StarWars5e.Parser.Globalization
{
    public interface IGlobalization
    {
        public Language Language { get; }
        
        public List<(string name, string startLine, int occurence)> WretchedHivesWeaponProperties { get; }
        public List<(string name, string startLine, int occurence)> WretchedHivesArmorProperties { get; }
        public List<(string name, string startLine, int occurence)> PlayerHandbookWeaponProperties { get; }
        public List<(string name, string startLine, int occurence)> PlayerHandbookArmorProperties { get; }

    }
}
