using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Interfaces
{
    public interface IPowerManager
    {
        Task<PagedSearchResult<Power>> SearchPowers(PowerSearch powerSearch);
    }
}
