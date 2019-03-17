using System.Threading.Tasks;
using StarWars.Storage.Clients;
using StarWars5e.Models.Interfaces;

namespace StarWars.Storage.Repositories
{
    public class EquipmentRepository: IEquipmentRepository
    {
        private readonly ITableStorageConnection _db;

        public EquipmentRepository(ITableStorageConnection db)
        {
            _db = db;
        }

        public async Task InsertItem(IEquipment item)
        {
            await this._db.AddItem(item);
        }

        
    }

    public interface IEquipmentRepository
    {
        Task InsertItem(IEquipment item);
    }
}
