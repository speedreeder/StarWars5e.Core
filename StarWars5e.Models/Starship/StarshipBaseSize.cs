using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Starship
{
    public class StarshipBaseSize : BaseEntity
    {
        public string Name { get; set; }
        public int Strength { get; set; }
        public int StrengthModifier { get; set; }
        public int Dexterity { get; set; }
        public int DexterityModifier { get; set; }
        public int Constitution { get; set; }
        public int ConstitutionModifier { get; set; }
        public int HitDiceNumberOfDice { get; set; }
        public int HitDiceDieType {
            get => (int)HitDiceDieTypeEnum;
            set => HitDiceDieTypeEnum = (DiceType)value;
        }
        [IgnoreProperty]
        public DiceType HitDiceDieTypeEnum { get; set; }
        public int MaxSuiteSystems { get; set; }
        public int ModSlotsAtTier0 { get; set; }
        [IgnoreProperty]
        public List<string> StockModificationNames { get; set; }
        public string StockModificationNamesJson
        {
            get => StockModificationNames == null ? "" : JsonConvert.SerializeObject(StockModificationNames);
            set => StockModificationNames = JsonConvert.DeserializeObject<List<string>>(value);

        }
        [IgnoreProperty]
        public List<string> StockModificationSuiteChoices { get; set; }
        public string StockModificationSuiteChoicesJson
        {
            get => StockModificationSuiteChoices == null ? "" : JsonConvert.SerializeObject(StockModificationSuiteChoices);
            set => StockModificationSuiteChoices = JsonConvert.DeserializeObject<List<string>>(value);

        }
        [IgnoreProperty]
        public List<string> StartingEquipmentNonShield { get; set; }
        public string StartingEquipmentNonShieldJson
        {
            get => StartingEquipmentNonShield == null ? "" : JsonConvert.SerializeObject(StartingEquipmentNonShield);
            set => StartingEquipmentNonShield = JsonConvert.DeserializeObject<List<string>>(value);

        }
        [IgnoreProperty]
        public List<string> StartingEquipmentArmorChoices { get; set; }
        public string StartingEquipmentArmorChoicesJson
        {
            get => StartingEquipmentArmorChoices == null ? "" : JsonConvert.SerializeObject(StartingEquipmentArmorChoices);
            set => StartingEquipmentArmorChoices = JsonConvert.DeserializeObject<List<string>>(value);

        }
        public int ModSlotsPerLevel { get; set; }
        public string AdditionalHitDiceText { get; set; }
        [IgnoreProperty]
        public List<string> SavingThrowOptions { get; set; }
        public string SavingThrowOptionsJson
        {
            get => SavingThrowOptions == null ? "" : JsonConvert.SerializeObject(SavingThrowOptions);
            set => SavingThrowOptions = JsonConvert.DeserializeObject<List<string>>(value);

        }
        [IgnoreProperty]
        public List<string> StockEquipmentNames { get; set; }
        public string StockEquipmentNamesJson
        {
            get => StockEquipmentNames == null ? "" : JsonConvert.SerializeObject(StockEquipmentNames);
            set => StockEquipmentNames = JsonConvert.DeserializeObject<List<string>>(value);

        }
        [IgnoreProperty]
        public List<StarshipFeature> Features { get; set; }
        public string FeaturesJson
        {
            get => Features == null ? "" : JsonConvert.SerializeObject(Features);
            set => Features = JsonConvert.DeserializeObject<List<StarshipFeature>>(value);

        }
    }
}
