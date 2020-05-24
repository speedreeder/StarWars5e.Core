using System.Collections.Generic;
using StarWars5e.Models.Enums;

namespace StarWars5e.Parser.Localization
{
    public class LocalizationRu : ILocalization
    {
        public Language Language => Language.Ru;

        public List<(string name, string startLine, int occurence)> PlayerHandbookWeaponProperties => throw new System.NotImplementedException();

        public List<(string name, string startLine, int occurence)> PlayerHandbookArmorProperties => throw new System.NotImplementedException();

        public List<(string name, string startLine, int occurence)> WretchedHivesWeaponProperties => throw new System.NotImplementedException();

        public List<(string name, string startLine, int occurence)> WretchedHivesArmorProperties => throw new System.NotImplementedException();
    }
}
