using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Equipment;
using StarWars5e.Models.Search;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Managers
{
    public class EquipmentManager : IEquipmentManager
    {
        private readonly ITableStorage _tableStorage;

        public EquipmentManager(ITableStorage tableStorage)
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

            return new PagedSearchResult<Equipment>(equipment.ToList(), equipmentSearch.PageSize, equipmentSearch.CurrentPage);
        }
    }
}
