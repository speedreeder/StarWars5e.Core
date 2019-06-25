using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Managers
{
    public class SearchManager : ISearchManager
    {
        private readonly ISearchIndexClient _searchIndexClient;

        public SearchManager(ISearchIndexClient searchIndexClient)
        {
            _searchIndexClient = searchIndexClient;
        }

        public async Task<IEnumerable<GlobalSearchTerm>> RunGlobalSearch(string searchText)
        {
            var results = (await _searchIndexClient.Documents.SearchAsync<GlobalSearchTerm>(searchText)).Results.Select(r => r.Document);
            return results;
        }
    }
}
