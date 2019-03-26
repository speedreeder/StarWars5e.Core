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

    }
}
