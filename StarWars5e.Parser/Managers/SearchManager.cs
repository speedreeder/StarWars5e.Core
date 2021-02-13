using System;
using System.Linq;
using System.Threading.Tasks;
using Azure.Search.Documents.Indexes;
using Microsoft.Extensions.DependencyInjection;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Storage;

namespace StarWars5e.Parser.Managers
{
    public class SearchManager
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly ILocalization _localization;
        private readonly SearchIndexClient _searchIndexClient;
        private readonly SearchIndexerClient _searchIndexerClient;

        public SearchManager(IServiceProvider serviceProvider, ILocalization localization)
        {
            _tableStorage = serviceProvider.GetService<IAzureTableStorage>();
            _globalSearchTermRepository = serviceProvider.GetService<GlobalSearchTermRepository>();
            _localization = localization;
            _searchIndexClient = serviceProvider.GetService<SearchIndexClient>();
            _searchIndexerClient = serviceProvider.GetService<SearchIndexerClient>();
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

            var index = await _searchIndexClient.GetIndexAsync("searchterms-index");
            await _searchIndexClient.DeleteIndexAsync("searchterms-index");

            await _searchIndexClient.CreateIndexAsync(index);

            var oldIndexer = await _searchIndexerClient.GetIndexerAsync("searchterms-indexer");
            await _searchIndexerClient.DeleteIndexerAsync(oldIndexer);

            await _searchIndexerClient.CreateIndexerAsync(oldIndexer);

            await _searchIndexerClient.RunIndexerAsync("searchterms-indexer");
        }
    }
}
