using System.Threading.Tasks;
using StarWars5e.Models.Search;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class SearchManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;

        public SearchManager(ITableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
        }

        public async Task Upload()
        {
            //var dupes = _globalSearchTermRepository.SearchTerms
            //    .GroupBy(i => i.RowKey)
            //    .Where(g => g.Count() > 1)
            //    .Select(g => g.Key);

            await _tableStorage.AddBatchAsync<GlobalSearchTerm>("searchTerms", _globalSearchTermRepository.SearchTerms,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
        }
    }
}
