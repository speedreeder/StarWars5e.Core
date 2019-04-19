using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;
using StarWars5e.Models.Species;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Managers
{
    public class SpeciesManager : ISpeciesManager
    {
        private readonly ITableStorage _tableStorage;

        public SpeciesManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }
        public async Task<PagedSearchResult<Species>> SearchSpecies(SpeciesSearch speciesSearch)
        {
            var filter = "";
            if (!string.IsNullOrEmpty(speciesSearch.Name))
            {
                filter = $"Name eq '{speciesSearch.Name}'";
            }
            if (speciesSearch.ContentType.HasValue && speciesSearch.ContentType != ContentType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} ContentType eq '{speciesSearch.ContentType.ToString()}'";
            }

            var query = new TableQuery<Species>().Where(filter);
            var species = await _tableStorage.QueryAsync("species", query);

            return new PagedSearchResult<Species>(species.ToList(), speciesSearch.PageSize, speciesSearch.CurrentPage);
        }
    }
}
