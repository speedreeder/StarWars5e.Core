using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Search.Documents;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Managers
{
    public class SearchManager : ISearchManager
    {
        private readonly SearchClient _searchClient;

        public SearchManager(SearchClient searchClient)
        {
            _searchClient = searchClient;
        }

        public async Task<IEnumerable<GlobalSearchTerm>> RunGlobalSearch(string searchText, Language language)
        {
            var search = await _searchClient.SearchAsync<GlobalSearchTerm>(searchText);

            var results = search.Value.GetResults()
                .Select(r => r.Document).Where(d => d.LanguageEnum == language);
            return results;
        }
    }
}
