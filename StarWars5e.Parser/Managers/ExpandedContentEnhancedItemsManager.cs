using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Models.EnhancedItems;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;
using StarWars5e.Parser.Storage;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentEnhancedItemsManager
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly ExpandedContentEnhancedItemsProcessor _expandedContentEnhancedItemsProcessor;
        private readonly List<string> _ecEnhancedItemsFileName = new List<string> { "ec_enhanced_items.txt" };
        private readonly ILocalization _localization;

        public ExpandedContentEnhancedItemsManager(IAzureTableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository, ILocalization localization)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _localization = localization;
            _expandedContentEnhancedItemsProcessor = new ExpandedContentEnhancedItemsProcessor();
        }

        public async Task Parse()
        {
            try
            {
                var enhancedItems = await _expandedContentEnhancedItemsProcessor.Process(_ecEnhancedItemsFileName, _localization);

                foreach (var enhancedItem in enhancedItems)
                {
                    enhancedItem.ContentSourceEnum = ContentSource.EC;

                    var enhancedItemSearchTerm = _globalSearchTermRepository.CreateSearchTerm(enhancedItem.Name, GlobalSearchTermType.EnhancedItem, ContentType.ExpandedContent,
                        $"/loot/enhancedItems?search={enhancedItem.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(enhancedItemSearchTerm);
                }

                await _tableStorage.AddBatchAsync<EnhancedItem>($"enhancedItems{_localization.Language}", enhancedItems,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload EC enhanced items.");
            }
        }
    }
}
