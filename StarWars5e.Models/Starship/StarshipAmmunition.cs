using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Starship
{
    public class StarshipAmmunition : StarshipEquipment
    {
        public StarshipWeaponCategory StarshipWeaponCategoryEnum { get; set; }
        public int StarshipWeaponCategory
        {
            get => (int)StarshipWeaponCategoryEnum;
            set => StarshipWeaponCategoryEnum = (StarshipWeaponCategory)value;
        }
        public int Cost { get; set; }
    }
}
