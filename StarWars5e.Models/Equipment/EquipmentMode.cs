using System.Collections.Generic;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Equipment
{
    public class EquipmentMode
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
        public int DamageNumberOfDice { get; set; }
        public string DamageType { get; set; }
        public int DamageDieModifier { get; set; }
        public int Weight { get; set; }

        public List<string> Properties { get; set; }
        public string PropertiesJson
        {
            get => Properties == null ? "" : JsonConvert.SerializeObject(Properties);
            set => Properties = JsonConvert.DeserializeObject<List<string>>(value);
        }

        public DiceType DamageDiceDieTypeEnum { get; set; }
        public int DamageDieType
        {
            get => (int)DamageDiceDieTypeEnum;
            set => DamageDiceDieTypeEnum = (DiceType)value;
        }
    }
}
