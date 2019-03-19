using System.Collections.Generic;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Equipment;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Models.Interfaces
{
    public interface IWeapon: IEquipment
    {
        int BurstRoundsUsed { get; set; }
        int ClipSize { get; set; }
        DamageType DamageType { get; set; }
        DiceType DiceType { get; set; }
        bool IsBurst { get; set; }
        bool IsDouble { get; set; }
        bool IsFinesse { get; set; }
        bool IsHeavy { get; set; }
        bool IsHidden { get; set; }
        bool IsLight { get; set; }
        bool IsLuminous { get; set; }
        bool IsReach { get; set; }
        bool IsThrown { get; set; }
        bool IsVersatile { get; set; }
        int LongRange { get; set; }
        int NumberOfDie { get; set; }
        int RequiredHands { get; set; }
        int ShortRange { get; set; }
        int StrengthRequirement { get; set; }
        DiceType VersatileDiceType { get; set; }
        int VersatileNumberOfDie { get; set; }
        WeaponClassification WeaponClassification { get; set; }
        IEnumerable<WeaponPropertyViewModel> WeaponProperties { get; set; }
    }
}