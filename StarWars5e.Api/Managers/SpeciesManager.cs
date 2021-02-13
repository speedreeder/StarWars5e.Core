using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Api.Storage;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;
using StarWars5e.Models.Species;

namespace StarWars5e.Api.Managers
{
    public class SpeciesManager : ISpeciesManager
    {
        private readonly IAzureTableStorage _tableStorage;

        public SpeciesManager(IAzureTableStorage tableStorage)
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

            switch (speciesSearch.SpeciesSearchOrdering)
            {
                case SpeciesSearchOrdering.NameAscending:
                    species = species.OrderBy(p => p.Name);
                    break;
                case SpeciesSearchOrdering.NameDescending:
                    species = species.OrderByDescending(p => p.Name);
                    break;
                case SpeciesSearchOrdering.ContentTypeAscending:
                    species = species.OrderBy(p => p.ContentType);
                    break;
                case SpeciesSearchOrdering.ContentTypeDescending:
                    species = species.OrderByDescending(p => p.ContentType);
                    break;
            }

            return new PagedSearchResult<Species>(species.ToList(), speciesSearch.PageSize, speciesSearch.CurrentPage);
        }
    }
}
