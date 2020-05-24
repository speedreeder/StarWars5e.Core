using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Models.EnhancedItems;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Parsers;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentEnhancedItemsManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly ExpandedContentEnhancedItemsProcessor _expandedContentEnhancedItemsProcessor;
        private readonly List<string> _ecEnhancedItemsFileName = new List<string> { "ec_enhanced_items.txt" };
        private readonly Language _language;

        public ExpandedContentEnhancedItemsManager(ITableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository, Language language)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _language = language;
            _expandedContentEnhancedItemsProcessor = new ExpandedContentEnhancedItemsProcessor();
        }

        public async Task Parse()
        {
            try
            {
                var enhancedItems = await _expandedContentEnhancedItemsProcessor.Process(_ecEnhancedItemsFileName, _language);

                foreach (var enhancedItem in enhancedItems)
                {
                    enhancedItem.ContentSourceEnum = ContentSource.EC;

                    var enhancedItemSearchTerm = _globalSearchTermRepository.CreateSearchTerm(enhancedItem.Name, GlobalSearchTermType.EnhancedItem, ContentType.ExpandedContent,
                        $"/loot/enhancedItems?search={enhancedItem.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(enhancedItemSearchTerm);
                }

                await _tableStorage.AddBatchAsync<EnhancedItem>("enhancedItems", enhancedItems,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload EC enhanced items.");
            }
        }
    }
}
