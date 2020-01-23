using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace StarWars5e.Models.EnhancedItems
{
    public class EnhancedItem : BaseEntity
    {
        public EnhancedItem()
        {
            RarityOptionsEnum = new List<EnhancedItemRarity>();
        }
        public string Name { get; set; }
        public EnhancedItemType TypeEnum { get; set; }
        public string Type
        {
            get => TypeEnum.ToString();
            set => TypeEnum = Enum.Parse<EnhancedItemType>(value);
        }
        public List<EnhancedItemRarity> RarityOptionsEnum { get; set; }
        public List<string> RarityOptions
        {
            get => RarityOptionsEnum.Select(r => r.ToString()).ToList();
            set => RarityOptionsEnum = value.Select(Enum.Parse<EnhancedItemRarity>).ToList();
        }
        public string RarityOptionsJson
        {
            get => RarityOptions == null ? "" : JsonConvert.SerializeObject(RarityOptions);
            set => RarityOptions = JsonConvert.DeserializeObject<List<string>>(value);
        }
        public string RarityText { get; set; }

        public string SearchableRarity
        {
            get
            {
                if (RarityOptionsEnum.Count > 1)
                {
                    return "Multiple";
                }

                if (RarityOptionsEnum.Count == 1)
                {
                    return RarityOptionsEnum.Single().ToString();
                }

                return null;
            }
        }
        public bool RequiresAttunement { get; set; }
        public string ValueText { get; set; }
        public string Text { get; set; }
        public bool HasPrerequisite { get; set; }
        public string Prerequisite { get; set; }
        public string Subtype { get; set; }
        public CyberneticAugmentationType CyberneticAugmentationTypeEnum { get; set; }
        public string CyberneticAugmentationType
        {
            get => CyberneticAugmentationTypeEnum.ToString();
            set => CyberneticAugmentationTypeEnum = Enum.Parse<CyberneticAugmentationType>(value);
        }
        public DroidCustomizationType DroidCustomizationTypeEnum { get; set; }
        public string DroidCustomizationType
        {
            get => DroidCustomizationTypeEnum.ToString();
            set => DroidCustomizationTypeEnum = Enum.Parse<DroidCustomizationType>(value);
        }
        public AdventuringGearType AdventuringGearTypeEnum { get; set; }
        public string AdventuringGearType
        {
            get => AdventuringGearTypeEnum.ToString();
            set => AdventuringGearTypeEnum = Enum.Parse<AdventuringGearType>(value);
        }
        public EnhancedArmorType EnhancedArmorTypeEnum { get; set; }
        public string EnhancedArmorType
        {
            get => EnhancedArmorTypeEnum.ToString();
            set => EnhancedArmorTypeEnum = Enum.Parse<EnhancedArmorType>(value);
        }
        public ConsumableType ConsumableTypeEnum { get; set; }
        public string ConsumableType
        {
            get => ConsumableTypeEnum.ToString();
            set => ConsumableTypeEnum = Enum.Parse<ConsumableType>(value);
        }
        public FocusType FocusTypeEnum { get; set; }
        public string FocusType
        {
            get => FocusTypeEnum.ToString();
            set => FocusTypeEnum = Enum.Parse<FocusType>(value);
        }
        public EnhancedShieldType EnhancedShieldTypeEnum { get; set; }
        public string EnhancedShieldType
        {
            get => EnhancedShieldTypeEnum.ToString();
            set => EnhancedShieldTypeEnum = Enum.Parse<EnhancedShieldType>(value);
        }
        public EnhancedWeaponType EnhancedWeaponTypeEnum { get; set; }
        public string EnhancedWeaponType
        {
            get => EnhancedWeaponTypeEnum.ToString();
            set => EnhancedWeaponTypeEnum = Enum.Parse<EnhancedWeaponType>(value);
        }
        public ItemModificationType ItemModificationTypeEnum { get; set; }
        public string ItemModificationType
        {
            get => ItemModificationTypeEnum.ToString();
            set => ItemModificationTypeEnum = Enum.Parse<ItemModificationType>(value);
        }
        public ValuableType ValuableTypeEnum { get; set; }
        public string ValuableType
        {
            get => ValuableTypeEnum.ToString();
            set => ValuableTypeEnum = Enum.Parse<ValuableType>(value);
        }
    }
}
