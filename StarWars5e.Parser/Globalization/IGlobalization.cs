using System.Collections.Generic;
using StarWars5e.Models.Enums;

namespace StarWars5e.Parser.Globalization
{
    public interface IGlobalization
    {
        public Language Language { get; }
        public List<(string name, string startLine, int occurence)> PlayerHandbookWeaponProperties { get; }
        public List<(string name, string startLine, int occurence)> PlayerHandbookArmorProperties { get; }

    }
}
