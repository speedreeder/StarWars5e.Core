using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Api.Storage;
using StarWars5e.Models.EnhancedItems;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Managers
{
    public class EnhancedItemManager : IEnhancedItemManager
    {
        private readonly IAzureTableStorage _tableStorage;

        public EnhancedItemManager(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<PagedSearchResult<EnhancedItem>> SearchEnhancedItems(EnhancedItemSearch enhancedItemSearch)
        {
            var filter = "";

            if (!string.IsNullOrEmpty(enhancedItemSearch.Name))
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} Name eq '{enhancedItemSearch.Name}'";
            }
            if (enhancedItemSearch.ContentType.HasValue && enhancedItemSearch.ContentType != ContentType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} ContentType eq '{enhancedItemSearch.ContentType.ToString()}'";
            }
            if (enhancedItemSearch.RequiresAttunement.HasValue)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} RequiresAttunement eq {enhancedItemSearch.RequiresAttunement.ToString().ToLower()}";
            }
            if (enhancedItemSearch.Rarity.HasValue && enhancedItemSearch.Rarity != EnhancedItemRarity.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} SearchableRarity eq '{enhancedItemSearch.Rarity.ToString()}'";
            }
            if (enhancedItemSearch.Type.HasValue && enhancedItemSearch.Type != EnhancedItemType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} Type eq '{enhancedItemSearch.Type.ToString()}'";
            }
            if (enhancedItemSearch.AdventuringGearType.HasValue && enhancedItemSearch.AdventuringGearType != AdventuringGearType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} AdventuringGearType eq '{enhancedItemSearch.AdventuringGearType.ToString()}'";
            }
            if (enhancedItemSearch.ConsumableType.HasValue && enhancedItemSearch.ConsumableType != ConsumableType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} ConsumableType eq '{enhancedItemSearch.ConsumableType.ToString()}'";
            }
            if (enhancedItemSearch.CyberneticAugmentationType.HasValue && enhancedItemSearch.CyberneticAugmentationType != CyberneticAugmentationType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} CyberneticAugmentationType eq '{enhancedItemSearch.CyberneticAugmentationType.ToString()}'";
            }
            if (enhancedItemSearch.DroidCustomizationType.HasValue && enhancedItemSearch.DroidCustomizationType != DroidCustomizationType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} DroidCustomizationType eq '{enhancedItemSearch.DroidCustomizationType.ToString()}'";
            }
            if (enhancedItemSearch.EnhancedWeaponType.HasValue && enhancedItemSearch.EnhancedWeaponType != EnhancedWeaponType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} EnhancedWeaponType eq '{enhancedItemSearch.EnhancedWeaponType.ToString()}'";
            }
            if (enhancedItemSearch.EnhancedArmorType.HasValue && enhancedItemSearch.EnhancedArmorType != EnhancedArmorType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} EnhancedArmorType eq '{enhancedItemSearch.EnhancedArmorType.ToString()}'";
            }
            if (enhancedItemSearch.EnhancedShieldType.HasValue && enhancedItemSearch.EnhancedShieldType != EnhancedShieldType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} EnhancedShieldType eq '{enhancedItemSearch.EnhancedShieldType.ToString()}'";
            }
            if (enhancedItemSearch.FocusType.HasValue && enhancedItemSearch.FocusType != FocusType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} FocusType eq '{enhancedItemSearch.FocusType.ToString()}'";
            }
            if (enhancedItemSearch.ItemModificationType.HasValue && enhancedItemSearch.ItemModificationType != ItemModificationType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} ItemModificationType eq '{enhancedItemSearch.ItemModificationType.ToString()}'";
            }
            if (enhancedItemSearch.ValuableType.HasValue && enhancedItemSearch.ValuableType != ValuableType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} ValuableType eq '{enhancedItemSearch.ValuableType.ToString()}'";
            }
            if (enhancedItemSearch.HasPrerequisite.HasValue)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} HasPrerequisite eq {enhancedItemSearch.HasPrerequisite.ToString().ToLower()}";
            }

            var query = new TableQuery<EnhancedItem>().Where(filter);
            var enhancedItems = await _tableStorage.QueryAsync("enhancedItems", query);

            switch (enhancedItemSearch.EnhancedItemSearchOrdering)
            {
                case EnhancedItemSearchOrdering.None:
                    break;
                case EnhancedItemSearchOrdering.NameAscending:
                    enhancedItems = enhancedItems.OrderBy(p => p.Name);
                    break;
                case EnhancedItemSearchOrdering.NameDescending:
                    enhancedItems = enhancedItems.OrderByDescending(p => p.Name);
                    break;
                case EnhancedItemSearchOrdering.ContentTypeAscending:
                    enhancedItems = enhancedItems.OrderBy(p => p.ContentType);
                    break;
                case EnhancedItemSearchOrdering.ContentTypeDescending:
                    enhancedItems = enhancedItems.OrderByDescending(p => p.ContentType);
                    break;
                case EnhancedItemSearchOrdering.RequiresAttunementAscending:
                    enhancedItems = enhancedItems.OrderBy(e => e.RequiresAttunement);
                    break;
                case EnhancedItemSearchOrdering.RequiresAttunementDescending:
                    enhancedItems = enhancedItems.OrderByDescending(e => e.RequiresAttunement);
                    break;
                case EnhancedItemSearchOrdering.RarityOptionsAscending:
                    enhancedItems = enhancedItems.OrderBy(p => p.RarityOptions.FirstOrDefault());
                    break;
                case EnhancedItemSearchOrdering.RarityOptionsDescending:
                    enhancedItems = enhancedItems.OrderByDescending(p => p.RarityOptions.LastOrDefault());
                    break;
                case EnhancedItemSearchOrdering.TypeAscending:
                    enhancedItems = enhancedItems.OrderBy(p => p.Type);
                    break;
                case EnhancedItemSearchOrdering.TypeDescending:
                    enhancedItems = enhancedItems.OrderByDescending(p => p.Type);
                    break;
                case EnhancedItemSearchOrdering.AdventuringGearTypeAscending:
                    enhancedItems = enhancedItems.OrderBy(p => p.AdventuringGearType);
                    break;
                case EnhancedItemSearchOrdering.AdventuringGearTypeDescending:
                    enhancedItems = enhancedItems.OrderByDescending(p => p.AdventuringGearType);
                    break;
                case EnhancedItemSearchOrdering.ConsumableTypeAscending:
                    enhancedItems = enhancedItems.OrderBy(p => p.ConsumableType);
                    break;
                case EnhancedItemSearchOrdering.ConsumableTypeDescending:
                    enhancedItems = enhancedItems.OrderByDescending(p => p.ConsumableType);
                    break;
                case EnhancedItemSearchOrdering.CyberneticAugmentationTypeAscending:
                    enhancedItems = enhancedItems.OrderBy(p => p.CyberneticAugmentationType);
                    break;
                case EnhancedItemSearchOrdering.CyberneticAugmentationTypeDescending:
                    enhancedItems = enhancedItems.OrderByDescending(p => p.CyberneticAugmentationType);
                    break;
                case EnhancedItemSearchOrdering.DroidCustomizationTypeAscending:
                    enhancedItems = enhancedItems.OrderBy(p => p.DroidCustomizationType);
                    break;
                case EnhancedItemSearchOrdering.DroidCustomizationTypeDescending:
                    enhancedItems = enhancedItems.OrderByDescending(p => p.DroidCustomizationType);
                    break;
                case EnhancedItemSearchOrdering.EnhancedArmorTypeAscending:
                    enhancedItems = enhancedItems.OrderBy(p => p.EnhancedArmorType);
                    break;
                case EnhancedItemSearchOrdering.EnhancedArmorTypeDescending:
                    enhancedItems = enhancedItems.OrderByDescending(p => p.EnhancedArmorType);
                    break;
                case EnhancedItemSearchOrdering.EnhancedShieldTypeAscending:
                    enhancedItems = enhancedItems.OrderBy(p => p.EnhancedShieldType);
                    break;
                case EnhancedItemSearchOrdering.EnhancedShieldTypeDescending:
                    enhancedItems = enhancedItems.OrderByDescending(p => p.EnhancedShieldType);
                    break;
                case EnhancedItemSearchOrdering.ItemModificationTypeAscending:
                    enhancedItems = enhancedItems.OrderBy(p => p.ItemModificationType);
                    break;
                case EnhancedItemSearchOrdering.ItemModificationTypeDescending:
                    enhancedItems = enhancedItems.OrderByDescending(p => p.ItemModificationType);
                    break;
                case EnhancedItemSearchOrdering.ValuableTypeAscending:
                    enhancedItems = enhancedItems.OrderBy(p => p.ValuableType);
                    break;
                case EnhancedItemSearchOrdering.ValuableTypeDescending:
                    enhancedItems = enhancedItems.OrderByDescending(p => p.ValuableType);
                    break;
                case EnhancedItemSearchOrdering.EnhancedWeaponTypeAscending:
                    enhancedItems = enhancedItems.OrderBy(p => p.EnhancedWeaponType);
                    break;
                case EnhancedItemSearchOrdering.EnhancedWeaponTypeDescending:
                    enhancedItems = enhancedItems.OrderByDescending(p => p.EnhancedWeaponType);
                    break;
                case EnhancedItemSearchOrdering.FocusTypeAscending:
                    enhancedItems = enhancedItems.OrderBy(p => p.FocusType);
                    break;
                case EnhancedItemSearchOrdering.FocusTypeDescending:
                    enhancedItems = enhancedItems.OrderByDescending(p => p.FocusType);
                    break;
                case EnhancedItemSearchOrdering.HasPrerequisiteAscending:
                    enhancedItems = enhancedItems.OrderBy(p => p.HasPrerequisite);
                    break;
                case EnhancedItemSearchOrdering.HasPrerequisiteDescending:
                    enhancedItems = enhancedItems.OrderByDescending(p => p.HasPrerequisite);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new PagedSearchResult<EnhancedItem>(enhancedItems.ToList(), enhancedItemSearch.PageSize, enhancedItemSearch.CurrentPage);
        }
    }
}
