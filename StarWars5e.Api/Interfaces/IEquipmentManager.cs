using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models.Equipment;

namespace StarWars5e.Api.Interfaces
{
    public interface IEquipmentManager
    {
        Task<IEnumerable<Equipment>> SearchEquipment(EquipmentSearch equipmentSearch);
    }
}
