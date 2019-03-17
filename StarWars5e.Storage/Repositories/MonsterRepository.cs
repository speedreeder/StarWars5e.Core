using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars.Storage.Clients;
using StarWars5e.Models.Monster;

namespace StarWars.Storage
{
    public class MonsterRepository : IMonsterRepository
    {
        readonly ITableStorageConnection _db;

        public MonsterRepository(ITableStorageConnection db)
        {
            _db = db;
        }

        public async Task InsertMonsters(List<Monster> monsters)
        {
            await this._db.AddMonsters(monsters);
        }
    }

    public interface IMonsterRepository
    {
        Task InsertMonsters(List<Monster> monsters);
    }
}
