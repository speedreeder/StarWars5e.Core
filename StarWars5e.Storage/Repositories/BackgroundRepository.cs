using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars.Storage.Clients;
using StarWars5e.Models;
using StarWars5e.Models.ViewModels;

namespace StarWars.Storage.Repositories
{
    public interface IBackgroundRepository
    {
        Task InsertBackground(List<BackgroundViewModel> speciesModels);
    }

    public class BackgroundRepository : IBackgroundRepository
    {
        private readonly ITableStorageConnection _db;

        public BackgroundRepository(ITableStorageConnection db)
        {
            _db = db;
        }


        public async Task InsertBackground(List<BackgroundViewModel> backgrounds)
        {
            foreach (var background in backgrounds)
            {
                background.PartitionKey = "core";
                if (string.IsNullOrEmpty(background.PartitionKey))
                {
                    ;
                    throw new ArgumentException("Partition Key must be provided");
                }

                if (string.IsNullOrEmpty(background.RowKey))
                {
                    background.RowKey = Guid.NewGuid().ToString();
                }

                await this._db.AddBackground(background);
            }
        }
    }
}