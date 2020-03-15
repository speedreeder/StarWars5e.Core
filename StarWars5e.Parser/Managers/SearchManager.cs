using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
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
            var existingSearchTerms = (await _tableStorage.GetAllAsync<GlobalSearchTerm>("searchTerms")).ToList();
            foreach (var existingSearchTerm in existingSearchTerms.Where(e => !e.IsDeleted))
            {
                existingSearchTerm.IsDeleted = true;
            }

            await _tableStorage.AddBatchAsync<GlobalSearchTerm>("searchTerms",
                existingSearchTerms.Where(e => e.PartitionKey == ContentType.ExpandedContent.ToString()),
                new BatchOperationOptions {BatchInsertMethod = BatchInsertMethod.InsertOrReplace});

            await _tableStorage.AddBatchAsync<GlobalSearchTerm>("searchTerms",
                existingSearchTerms.Where(e => e.PartitionKey == ContentType.Core.ToString()),
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            await _tableStorage.AddBatchAsync<GlobalSearchTerm>("searchTerms",
                _globalSearchTermRepository.SearchTerms.Where(s =>
                    s.PartitionKey == ContentType.ExpandedContent.ToString()),
                new BatchOperationOptions {BatchInsertMethod = BatchInsertMethod.Insert});

            await _tableStorage.AddBatchAsync<GlobalSearchTerm>("searchTerms",
                _globalSearchTermRepository.SearchTerms.Where(s =>
                    s.PartitionKey == ContentType.Core.ToString()),
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.Insert });
        }
    }
}
