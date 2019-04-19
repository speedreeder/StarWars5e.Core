using System.Threading.Tasks;
using StarWars5e.Models.Search;
using StarWars5e.Models.Species;

namespace StarWars5e.Api.Interfaces
{
    public interface ISpeciesManager
    {
        Task<PagedSearchResult<Species>> SearchSpecies(SpeciesSearch speciesSearch);
    }
}
