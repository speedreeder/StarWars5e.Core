using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using StarWars5e.Models;
using StarWars5e.Models.EnhancedItems;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Starship;
using StarWars5e.Parser.Parsers;
using StarWars5e.Parser.Parsers.WH;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class WretchedHivesManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly CloudBlobContainer _cloudBlobContainer;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly IBaseProcessor<ChapterRules> _wretchedHivesChapterRulesProcessor;
        private readonly IBaseProcessor<EnhancedItem> _enhancedItemProcessor;

        private readonly List<string> _whFilesName = new List<string>
        {
            "WH.wh_02.txt", "WH.wh_03-04.txt", "WH.wh_05-11.txt", "WH.wh_aa.txt"
        };

        public WretchedHivesManager(ITableStorage tableStorage, CloudStorageAccount cloudStorageAccount, GlobalSearchTermRepository globalSearchTermRepository)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _wretchedHivesChapterRulesProcessor = new WretchedHivesChapterRulesProcessor(globalSearchTermRepository);
            _enhancedItemProcessor = new EnhancedItemProcessor();

            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            _cloudBlobContainer = cloudBlobClient.GetContainerReference("wretched-hives-rules");
        }

        public async Task Parse()
        {
            try
            {
                var rules = await _wretchedHivesChapterRulesProcessor.Process(_whFilesName);

                await _cloudBlobContainer.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Off, null, null);
                foreach (var chapterRules in rules)
                {
                    var json = JsonConvert.SerializeObject(chapterRules);
                    var blob = _cloudBlobContainer.GetBlockBlobReference($"{chapterRules.ChapterName}.json");

                    await blob.UploadTextAsync(json);
                }
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload WH rules.");
            }

            try
            {
                var enhancedItems = await _enhancedItemProcessor.Process(_whFilesName.Where(f => f.Equals("WH.wh_aa.txt")).ToList());

                foreach (var enhancedItem in enhancedItems)
                {
                    var enhancedItemSearchTerm = _globalSearchTermRepository.CreateSearchTerm(enhancedItem.Name, GlobalSearchTermType.EnhancedItem, ContentType.Core,
                        $"/loot/enhancedItems?search={enhancedItem.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(enhancedItemSearchTerm);
                }

                await _tableStorage.AddBatchAsync<StarshipModification>("enhancedItems", enhancedItems,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload WH enhanced items.");
            }
        }
    }
}
