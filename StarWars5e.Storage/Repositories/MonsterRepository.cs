using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models.Monster;
using StarWars5e.Storage.Clients;

namespace StarWars5e.Storage.Repositories
{
    public class MonsterRepository : IMonsterRepository
    {
        readonly ITableStorageConnection _db;

        public MonsterRepository(ITableStorageConnection db)
        {
            _db = db;
        }

        public async Task InsertMonsters(List<MonsterOld> monsters)
        {
            await this._db.AddMonsters(monsters);
        }
    }

    public interface IMonsterRepository
    {
        Task InsertMonsters(List<MonsterOld> monsters);
    }
}
