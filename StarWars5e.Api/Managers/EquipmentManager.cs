using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Equipment;
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

        public async Task<IEnumerable<Equipment>> SearchEquipment(EquipmentSearch equipmentSearch)
        {
            var filters = new List<string>();

            if (!string.IsNullOrWhiteSpace(equipmentSearch.Name))
            {
                filters.Add(TableQuery.GenerateFilterCondition("Name", QueryComparisons.Equal, equipmentSearch.Name));
            }

            if (equipmentSearch.EquipmentCategory.HasValue)
            {
                filters.Add(TableQuery.GenerateFilterCondition("EquipmentCategory", QueryComparisons.Equal, equipmentSearch.EquipmentCategory.ToString()));
            }

            //var query = new TableQuery<Equipment>();
            //foreach (var filter in filters)
            //{
            //    query.
            //}

            var equipment = await _tableStorage.GetAllAsync<Equipment>("equipment");

            return equipment;
        }
    }
}
