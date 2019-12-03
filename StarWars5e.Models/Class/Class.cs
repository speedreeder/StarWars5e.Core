using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Class
{
    public class Class : BaseEntity
    {
        public Class()
        {
            ImageUrls = new List<string>();
        }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string PrimaryAbility { get; set; }
        public string FlavorText { get; set; }
        public string CreatingText { get; set; }
        public string QuickBuildText { get; set; }
        public string LevelChangeHeadersJson { get; set; }

        [IgnoreProperty]
        public Dictionary<int, Dictionary<string, string>> LevelChanges { get; set; }
        public string LevelChangesJson {
            get => LevelChanges == null ? "" : JsonConvert.SerializeObject(LevelChanges);
            set => LevelChanges = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<string, string>>>(value);
        }

        public DiceType HitDiceDieTypeEnum { get; set; }
        public int HitDiceDieType
        {
            get => (int)HitDiceDieTypeEnum;
            set => HitDiceDieTypeEnum = (DiceType)value;
        }

        public string HitPointsAtFirstLevel { get; set; }
        public string HitPointsAtHigherLevels { get; set; }
        public int HitPointsAtFirstLevelNumber { get; set; }
        public int HitPointsAtHigherLevelsNumber { get; set; }

        public List<string> ArmorProficiencies { get; set; }
        public string ArmorProficienciesJson
        {
            get => ArmorProficiencies == null ? "" : JsonConvert.SerializeObject(ArmorProficiencies);
            set => ArmorProficiencies = JsonConvert.DeserializeObject<List<string>>(value);
        }

        public List<string> WeaponProficiencies { get; set; }
        public string WeaponProficienciesJson
        {
            get => WeaponProficiencies == null ? "" : JsonConvert.SerializeObject(WeaponProficiencies);
            set => WeaponProficiencies = JsonConvert.DeserializeObject<List<string>>(value);
        }

        public List<string> ToolProficiencies { get; set; }
        public string ToolProficienciesJson
        {
            get => ToolProficiencies == null ? "" : JsonConvert.SerializeObject(ToolProficiencies);
            set => ToolProficiencies = JsonConvert.DeserializeObject<List<string>>(value);
        }

        public List<string> SavingThrows { get; set; }
        public string SavingThrowsJson
        {
            get => SavingThrows == null ? "" : JsonConvert.SerializeObject(SavingThrows);
            set => SavingThrows = JsonConvert.DeserializeObject<List<string>>(value);
        }

        public string SkillChoices { get; set; }

        public List<string> EquipmentLines { get; set; }
        public string EquipmentLinesJson
        {
            get => EquipmentLines == null ? "" : JsonConvert.SerializeObject(EquipmentLines);
            set => EquipmentLines = JsonConvert.DeserializeObject<List<string>>(value);
        }

        public string StartingWealthVariant { get; set; }

        public string ClassFeatureText { get; set; }
        public string ClassFeatureText2 { get; set; }
        public string ArchetypeFlavorText { get; set; }
        public string ArchetypeFlavorName { get; set; }

        [IgnoreProperty]
        public List<Archetype> Archetypes { get; set; }

        public List<string> ImageUrls { get; set; }
        public string ImageUrlsJson
        {
            get => ImageUrls == null ? "" : JsonConvert.SerializeObject(ImageUrls);
            set => ImageUrls = JsonConvert.DeserializeObject<List<string>>(value);
        }
    }
}
