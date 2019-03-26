using System;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Starship
{
    public class StarshipWeapon : StarshipEquipment
    {
        public StarshipWeaponCategory WeaponCategoryEnum { get; set; }
        public string WeaponCategory
        {
            get => WeaponCategoryEnum.ToString();
            set => WeaponCategoryEnum = Enum.Parse<StarshipWeaponCategory>(value);
        }

        public StarshipWeaponSize WeaponSizeEnum { get; set; }
        public string WeaponSize
        {
            get => WeaponSizeEnum.ToString();
            set => WeaponSizeEnum = Enum.Parse<StarshipWeaponSize>(value);
        }

        public int Cost { get; set; }
        public int DamageNumberOfDice { get; set; }
        public string DamageType { get; set; }
        public int DamageDieModifier{ get; set; }
        public int DamageDieType
        {
            get => (int)HitDiceDieTypeEnum;
            set => HitDiceDieTypeEnum = (DiceType)value;
        }
        [IgnoreProperty]
        public DiceType HitDiceDieTypeEnum { get; set; }

        public int AttackBonus { get; set; }
        public int AttacksPerRound { get; set; }
        public int ShortRange { get; set; }
        public int LongRange { get; set; }
        public int Reload { get; set; }
        public string Properties { get; set; }
    }
}
