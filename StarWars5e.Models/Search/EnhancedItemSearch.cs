using StarWars5e.Models.EnhancedItems;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Search
{
    public class EnhancedItemSearch : SearchBase
    {
        public EnhancedItemSearchOrdering EnhancedItemSearchOrdering { get; set; } = EnhancedItemSearchOrdering.None;
        public string Name { get; set; }
        public ContentType? ContentType { get; set; }
        public bool? RequiresAttunement { get; set; }
        public EnhancedItemRarity? Rarity { get; set; }
        public EnhancedItemType? Type { get; set; }
        public AdventuringGearType? AdventuringGearType { get; set; }
        public ConsumableType? ConsumableType { get; set; }
        public CyberneticAugmentationType? CyberneticAugmentationType { get; set; }
        public DroidCustomizationType? DroidCustomizationType { get; set; }
        public EnhancedArmorType? EnhancedArmorType { get; set; }
        public EnhancedShieldType? EnhancedShieldType { get; set; }
        public ItemModificationType? ItemModificationType { get; set; }
        public ValuableType? ValuableType { get; set; }
        public EnhancedWeaponType? EnhancedWeaponType { get; set; }
        public FocusType? FocusType { get; set; }
        public bool? HasPrerequisite { get; set; }
    }
    public enum EnhancedItemSearchOrdering
    {
        None,
        NameAscending,
        NameDescending,
        ContentTypeAscending,
        ContentTypeDescending,
        RequiresAttunementAscending,
        RequiresAttunementDescending,
        RarityOptionsAscending,
        RarityOptionsDescending,
        ValueOptionsAscending,
        ValueOptionsDescending,
        TypeAscending,
        TypeDescending,
        AdventuringGearTypeAscending,
        AdventuringGearTypeDescending,
        ConsumableTypeAscending,
        ConsumableTypeDescending,
        CyberneticAugmentationTypeAscending,
        CyberneticAugmentationTypeDescending,
        DroidCustomizationTypeAscending,
        DroidCustomizationTypeDescending,
        EnhancedArmorTypeAscending,
        EnhancedArmorTypeDescending,
        EnhancedShieldTypeAscending,
        EnhancedShieldTypeDescending,
        ItemModificationTypeAscending,
        ItemModificationTypeDescending,
        ValuableTypeAscending,
        ValuableTypeDescending,
        EnhancedWeaponTypeAscending,
        EnhancedWeaponTypeDescending,
        FocusTypeAscending,
        FocusTypeDescending,
        HasPrerequisiteAscending,
        HasPrerequisiteDescending
    }
}
