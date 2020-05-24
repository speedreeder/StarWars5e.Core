using System.Collections.Generic;
using StarWars5e.Models.Enums;

namespace StarWars5e.Parser.Localization
{
    public interface ILocalization
    {
        public Language Language { get; }
        
        public List<(string name, string startLine, int occurence)> WretchedHivesWeaponProperties { get; }
        public List<(string name, string startLine, int occurence)> WretchedHivesArmorProperties { get; }
        public List<(string name, string startLine, int occurence)> PlayerHandbookWeaponProperties { get; }
        public List<(string name, string startLine, int occurence)> PlayerHandbookArmorProperties { get; }

    }
}
