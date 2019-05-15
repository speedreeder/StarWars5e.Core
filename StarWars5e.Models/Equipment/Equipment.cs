using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Equipment
{
    public class Equipment : BaseEntity
    {
        public Equipment()
        {
            Modes = new List<Equipment>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }      
        public int Weight { get; set; }

        public EquipmentCategory EquipmentCategoryEnum { get; set; }
        public string EquipmentCategory
        {
            get => EquipmentCategoryEnum.ToString();
            set => EquipmentCategoryEnum = Enum.Parse<EquipmentCategory>(value);
        }

        public int DamageNumberOfDice { get; set; }
        public DamageType DamageTypeEnum { get; set; }
        public string DamageType
        {
            get => DamageTypeEnum.ToString();
            set => DamageTypeEnum = Enum.Parse<DamageType>(value);
        }

        public int DamageDieModifier { get; set; }

        public WeaponClassification WeaponClassificationEnum { get; set; }
        public string WeaponClassification
        {
            get => WeaponClassificationEnum.ToString();
            set => WeaponClassificationEnum = Enum.Parse<WeaponClassification>(value);
        }

        public ArmorClassification ArmorClassificationEnum { get; set; }
        public string ArmorClassification
        {
            get => ArmorClassificationEnum.ToString();
            set => ArmorClassificationEnum = Enum.Parse<ArmorClassification>(value);
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

        public Dictionary<string, string> PropertiesMap { get; set; }
        public string PropertiesMapJson
        {
            get => PropertiesMap == null ? "" : JsonConvert.SerializeObject(PropertiesMap);
            set => PropertiesMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
        }

        public List<Equipment> Modes { get; set; }
        public string ModesJson
        {
            get => Modes == null ? "" : JsonConvert.SerializeObject(Modes);
            set => Modes = JsonConvert.DeserializeObject<List<Equipment>>(value);
        }

        public string AC { get; set; }
        public string StrengthRequirement { get; set; }
        public bool StealthDisadvantage { get; set; }
    }
}
