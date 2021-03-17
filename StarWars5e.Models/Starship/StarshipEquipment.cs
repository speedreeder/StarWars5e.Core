using System;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Starship
{
    public class StarshipEquipment : BaseEntity
    {
        public string Name { get; set; }
        public StarshipEquipmentType TypeEnum { get; set; }
        public string Type
        {
            get => TypeEnum.ToString();
            set => TypeEnum = Enum.Parse<StarshipEquipmentType>(value);
        }
        public int Cost { get; set; }
        public StarshipWeaponCategory StarshipWeaponCategoryEnum { get; set; }
        public string StarshipWeaponCategory
        {
            get => StarshipWeaponCategoryEnum.ToString();
            set => StarshipWeaponCategoryEnum = Enum.Parse<StarshipWeaponCategory>(value);
        }
        public int ArmorClassBonus { get; set; }
        public int? HitPointsPerHitDie { get; set; }
        public string HyperDriveClass { get; set; }
        public int NavcomputerBonus { get; set; }
        public string CapacityMultiplier { get; set; }
        public string RegenerationRateCoefficient { get; set; }
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
        public int DamageNumberOfDice { get; set; }
        public string DamageType { get; set; }
        public int DamageDieModifier { get; set; }
        public int DamageDieType
        {
            get => (int)DamageDiceDieTypeEnum;
            set => DamageDiceDieTypeEnum = (DiceType)value;
        }
        public DiceType DamageDiceDieTypeEnum { get; set; }
        public int AttackBonus { get; set; }
        public int AttacksPerRound { get; set; }
        public int ShortRange { get; set; }
        public int LongRange { get; set; }
        public int Reload { get; set; }
        public string Properties { get; set; }
        public string Description { get; set; }
        public string FuelCostsModifier { get; set; }
        public string PowerDiceRecover { get; set; }
        public string CentralStorageCapacity { get; set; }
        public string SystemStorageCapacity { get; set; }
    }
}
