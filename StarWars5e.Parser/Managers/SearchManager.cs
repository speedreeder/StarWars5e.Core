using System;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;
using StarWars5e.Parser.Localization;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class SearchManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly ILocalization _localization;

        public SearchManager(ITableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository, ILocalization localization)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _localization = localization;
        }

        public async Task Upload()
        {
            var existingSearchTerms = (await _tableStorage.GetAllAsync<GlobalSearchTerm>("searchTerms")).ToList();

            var i = 1;
            Parallel.ForEach(existingSearchTerms, new ParallelOptions{ MaxDegreeOfParallelism = Environment.ProcessorCount}, async existingSearchTerm =>
            {
                await _tableStorage.DeleteAsync("searchTerms", existingSearchTerm);
                Console.WriteLine($"Deleted {i} of {existingSearchTerms.Count} existing search terms.");
                i++;
            });

            foreach (var globalSearchTerm in _globalSearchTermRepository.SearchTerms)
            {
                globalSearchTerm.LanguageEnum = _localization.Language;
            }

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
