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

        public async Task<List<Power>> GetAllPowers()
        {
            return await this._db.GetAllPowers();
        }
    }

    public interface IPowerRepository
    {
        Task InsertPowers(List<Power> powers);

        Task<List<Power>> GetAllPowers();
    }
}