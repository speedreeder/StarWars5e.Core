using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Models.Starship;
using StarWars5e.Models.Utils;
using StarWars5e.Storage.Clients;

namespace StarWars5e.Storage.Repositories
{
    public class StarshipBaseSizeRepository : IStarshipBaseSizeRepository
    {
        private readonly ITableStorageConnection _db;

        public StarshipBaseSizeRepository(ITableStorageConnection db)
        {
            _db = db;
        }

        public async Task Insert(List<StarshipBaseSize> starshipBaseSizes, string partitionKey)
        {
            var chunkedLists = starshipBaseSizes.ChunkBy(100);

            foreach (var chunkedList in chunkedLists)
            {
                var modificationOperation = new TableBatchOperation();
                foreach (var modification in chunkedList)
                {
                    modification.PartitionKey = partitionKey;

                    if (string.IsNullOrEmpty(modification.RowKey))
                    {
                        modification.RowKey = Guid.NewGuid().ToString();
                    }

                    modificationOperation.Insert(modification);
                }
                await _db.AddModifications(modificationOperation);
            }
        }
    }

    public interface IStarshipBaseSizeRepository
    {
        Task Insert(List<StarshipBaseSize> starshipBaseSizes, string partitionKey);
    }
}
