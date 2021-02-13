using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Api.Storage;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Equipment;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Managers
{
    public class EquipmentManager : IEquipmentManager
    {
        private readonly IAzureTableStorage _tableStorage;

        public EquipmentManager(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<PagedSearchResult<Equipment>> SearchEquipment(EquipmentSearch equipmentSearch)
        {
            var filter = "";
            if (!string.IsNullOrEmpty(equipmentSearch.Name))
            {
                filter = $"Name eq '{equipmentSearch.Name}'";
            }
            if (equipmentSearch.ContentType.HasValue && equipmentSearch.ContentType != ContentType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} ContentType eq '{equipmentSearch.ContentType.ToString()}'";
            }
            if (equipmentSearch.EquipmentCategory.HasValue && equipmentSearch.EquipmentCategory != EquipmentCategory.Unknown)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} EquipmentCategory eq '{equipmentSearch.EquipmentCategory.ToString()}'";
            }
            if (equipmentSearch.ArmorClassification.HasValue && equipmentSearch.ArmorClassification != ArmorClassification.Unknown)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} ArmorClassification eq '{equipmentSearch.ArmorClassification.ToString()}'";
            }
            if (equipmentSearch.WeaponClassification.HasValue && equipmentSearch.WeaponClassification != WeaponClassification.Unknown)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} WeaponClassification eq '{equipmentSearch.WeaponClassification.ToString()}'";
            }

            var query = new TableQuery<Equipment>().Where(filter);
            var equipment = await _tableStorage.QueryAsync("equipment", query);

            switch (equipmentSearch.EquipmentSearchOrdering)
            {
                case EquipmentSearchOrdering.NameAscending:
                    equipment = equipment.OrderBy(p => p.Name);
                    break;
                case EquipmentSearchOrdering.NameDescending:
                    equipment = equipment.OrderByDescending(p => p.Name);
                    break;
                case EquipmentSearchOrdering.ContentTypeAscending:
                    equipment = equipment.OrderBy(p => p.ContentType);
                    break;
                case EquipmentSearchOrdering.ContentTypeDescending:
                    equipment = equipment.OrderByDescending(p => p.ContentType);
                    break;
                case EquipmentSearchOrdering.EquipmentCategoryAscending:
                    equipment = equipment.OrderBy(p => p.EquipmentCategory);
                    break;
                case EquipmentSearchOrdering.EquipmentCategoryDescending:
                    equipment = equipment.OrderByDescending(p => p.EquipmentCategory);
                    break;
                case EquipmentSearchOrdering.ArmorClassificationAscending:
                    equipment = equipment.OrderBy(p => p.ArmorClassification);
                    break;
                case EquipmentSearchOrdering.ArmorClassificationDescending:
                    equipment = equipment.OrderByDescending(p => p.ArmorClassification);
                    break;
                case EquipmentSearchOrdering.WeaponClassificationAscending:
                    equipment = equipment.OrderBy(p => p.WeaponClassification);
                    break;
                case EquipmentSearchOrdering.WeaponClassificationDescending:
                    equipment = equipment.OrderByDescending(p => p.WeaponClassification);
                    break;
            }

            return new PagedSearchResult<Equipment>(equipment.ToList(), equipmentSearch.PageSize, equipmentSearch.CurrentPage);
        }
    }
}
