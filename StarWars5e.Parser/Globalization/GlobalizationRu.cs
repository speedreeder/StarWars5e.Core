using StarWars5e.Models.Enums;
using System.Collections.Generic;

namespace StarWars5e.Parser.Globalization
{
    public class GlobalizationRu : IGlobalization
    {
        public Language Language => Language.Ru;

        public List<(string name, string startLine, int occurence)> PlayerHandbookWeaponProperties => throw new System.NotImplementedException();

        public List<(string name, string startLine, int occurence)> PlayerHandbookArmorProperties => throw new System.NotImplementedException();
    }
}
