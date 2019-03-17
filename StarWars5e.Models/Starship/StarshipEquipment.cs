using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Starship
{
    public class StarshipEquipment : TableEntity
    {
        public string Name { get; set; }
        public StarshipEquipmentType TypeEnum { get; set; }
        public int Type
        {
            get => (int)TypeEnum;
            set => TypeEnum = (StarshipEquipmentType)value;
        }

    }
}
