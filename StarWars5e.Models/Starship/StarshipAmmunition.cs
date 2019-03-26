using System;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Starship
{
    public class StarshipAmmunition : StarshipEquipment
    {
        public StarshipWeaponCategory StarshipWeaponCategoryEnum { get; set; }
        public string StarshipWeaponCategory
        {
            get => StarshipWeaponCategoryEnum.ToString();
            set => StarshipWeaponCategoryEnum = Enum.Parse<StarshipWeaponCategory>(value);
        }
        public int Cost { get; set; }
    }
}
