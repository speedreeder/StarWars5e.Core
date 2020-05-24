using System.Collections.Generic;
using StarWars5e.Models.Enums;

namespace StarWars5e.Parser.Globalization
{
    public class GlobalizationEn : IGlobalization
    {
        public Language Language => Language.En;

        public List<(string name, string startLine, int occurence)> PlayerHandbookWeaponProperties => new List<(string name, string startLine, int occurence)>
        {
            ("Ammunition", "#### Ammunition", 1),
            ("Burst", "#### Burst", 1),
            ("Dexterity", "#### Dexterity", 1),
            ("Double", "#### Double", 1),
            ("Finesse", "#### Finesse", 1),
            ("Fixed", "#### Fixed", 1),
            ("Heavy", "#### Heavy", 3 ),
            ("Hidden", "#### Hidden", 1),
            ("Light", "#### Light", 2),
            ("Luminous", "#### Luminous", 1),
            ("Range", "#### Range", 1),
            ("Reach", "#### Reach", 1),
            ("Reload", "#### Reload", 1),
            ("Returning", "#### Returning", 1),
            ("Special", "#### Special", 1),
            ("Strength", "#### Strength", 2),
            ("Thrown", "#### Thrown", 1),
            ("Two-Handed", "#### Two-Handed", 1),
            ("Versatile", "#### Versatile", 1)
        };

        public List<(string name, string startLine, int occurence)> PlayerHandbookArmorProperties => new List<(string name, string startLine, int occurence)>
        {
            ("Bulky", "#### Bulky", 1),
            ("Obtrusive", "#### Obtrusive", 1),
            ("Strength", "#### Strength", 1)
        };
    }
}
