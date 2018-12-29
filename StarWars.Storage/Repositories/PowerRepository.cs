using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars.Storage.Clients;
using StarWars5e.Models;

namespace StarWars.Storage.Repositories
{
    public class PowerRepository : IPowerRepository
    {
        private readonly ITableStorageConnection _db;

        public PowerRepository(ITableStorageConnection db)
        {
            _db = db;
        }
        public async Task InsertPowers(List<Power> powers)
        {
            await this._db.UpsertPowers(powers);
        }
    }

    public interface IPowerRepository
    {
        Task InsertPowers(List<Power> powers);
    }
}