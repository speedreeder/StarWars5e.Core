using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Search
{
    public class EquipmentSearch : SearchBase
    {
        public EquipmentSearchOrdering EquipmentSearchOrdering { get; set; } = EquipmentSearchOrdering.None;
        public string Name { get; set; }
        public ContentType? ContentType { get; set; }
        public EquipmentCategory? EquipmentCategory { get; set; }
        public ArmorClassification? ArmorClassification { get; set; }
        public WeaponClassification? WeaponClassification { get; set; }
    }

    public enum EquipmentSearchOrdering
    {
        None,
        NameAscending,
        NameDescending,
        ContentTypeAscending,
        ContentTypeDescending,
        EquipmentCategoryAscending,
        EquipmentCategoryDescending,
        ArmorClassificationAscending,
        ArmorClassificationDescending,
        WeaponClassificationAscending,
        WeaponClassificationDescending
    }
}
