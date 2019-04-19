using System.Threading.Tasks;
using StarWars5e.Models.Equipment;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Interfaces
{
    public interface IEquipmentManager
    {
        Task<PagedSearchResult<Equipment>> SearchEquipment(EquipmentSearch equipmentSearch);
    }
}
