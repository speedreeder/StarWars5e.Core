using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars.Storage.Clients;
using StarWars5e.Models.Starship;
using StarWars5e.Models.Utils;

namespace StarWars.Storage.Repositories
{
    public class ModificationRepository : IModificationRepository
    {
        private readonly ITableStorageConnection _db;

        public ModificationRepository(ITableStorageConnection db)
        {
            _db = db;
        }


        public async Task Insert(List<Modification> modifications, string partitionKey)
        {
            
            var chunkedLists = modifications.ChunkBy(100);

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

    public interface IModificationRepository
    {
        Task Insert(List<Modification> modifications, string partitionKey);
    }
}