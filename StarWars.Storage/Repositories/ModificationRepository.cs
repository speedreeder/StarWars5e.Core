using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars.Storage.Clients;
using StarWars5e.Models;

namespace StarWars.Storage.Repositories
{
    public class ModificationRepository : IModificationRepository
    {
        private readonly ITableStorageConnection _db;

        public ModificationRepository(ITableStorageConnection db)
        {
            _db = db;
        }


        public async Task InsertModifications(List<Modification> modifications, string partitionKey)
        {
            foreach (var modification in modifications)
            {
                modification.PartitionKey = partitionKey;

                if (string.IsNullOrEmpty(modification.RowKey))
                {
                    modification.RowKey = Guid.NewGuid().ToString();
                }

                await _db.AddModification(modification);
            }
        }
    }

    public interface IModificationRepository
    {
        Task InsertModifications(List<Modification> modifications, string partitionKey);
    }
}