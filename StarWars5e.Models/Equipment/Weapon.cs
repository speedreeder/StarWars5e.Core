using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Equipment
{
    public class Weapon : Equipment
    {
        public Weapon()
        {
            Modes = new List<Weapon>();
        }

        public int DamageNumberOfDice { get; set; }
        public DamageType DamageTypeEnum { get; set; }
        public string DamageType
        {
            get => DamageTypeEnum.ToString();
            set => DamageTypeEnum = Enum.Parse<DamageType>(value);
        }

        public int DamageDieModifier { get; set; }
        public WeaponClassification ClassificationEnum { get; set; }
        public string Classification
        {
            get => ClassificationEnum.ToString();
            set => ClassificationEnum = Enum.Parse<WeaponClassification>(value);
        }

        public DiceType DamageDiceDieTypeEnum { get; set; }
        public int DamageDieType
        {
            get => (int)DamageDiceDieTypeEnum;
            set => DamageDiceDieTypeEnum = (DiceType)value;
        }

        public List<string> Properties { get; set; }
        public string PropertiesJson
        {
            get => Properties == null ? "" : JsonConvert.SerializeObject(Properties);
            set => Properties = JsonConvert.DeserializeObject<List<string>>(value);
        }

        public List<Weapon> Modes { get; set; }
        public string ModesJson
        {
            get => Modes == null ? "" : JsonConvert.SerializeObject(Modes);
            set => Modes = JsonConvert.DeserializeObject<List<Weapon>>(value);
        }
    }
}
