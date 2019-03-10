using StarWars5e.Models.Enums;
using StarWars5e.Models.Equipment;

namespace StarWars5e.Models.Interfaces
{
    public interface IArmor: IEquipment
    {
        int ArmorClass { get; set; }
        ArmorClassification ArmorClassification { get; set; }
        int DexterityMax { get; set; }
        bool IsShield { get; set; }
        bool StealthImpaired { get; set; }
        int StrengthRequirement { get; set; }
    }
}