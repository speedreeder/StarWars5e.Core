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
        private readonly WeaponPropertyProcessor _weaponPropertyProcessor;
        private readonly ArmorPropertyProcessor _armorPropertyProcessor;
        private readonly ExpandedContentEquipmentProcessor _expandedContentEquipmentProcessor;


        private readonly List<string> _whFilesName = new List<string>
        {
            "WH.wh_01.txt", "WH.wh_02.txt", "WH.wh_03.txt", "WH.wh_04.txt", "WH.wh_05.txt", "WH.wh_06.txt",
            "WH.wh_07.txt", "WH.wh_08.txt", "WH.wh_aa.txt"
        };

        public WretchedHivesManager(ITableStorage tableStorage, CloudStorageAccount cloudStorageAccount,
            GlobalSearchTermRepository globalSearchTermRepository, ExpandedContentEquipmentProcessor expandedContentEquipmentProcessor)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _expandedContentEquipmentProcessor = expandedContentEquipmentProcessor;
            _wretchedHivesChapterRulesProcessor = new WretchedHivesChapterRulesProcessor(globalSearchTermRepository);
            _enhancedItemProcessor = new EnhancedItemProcessor();

            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            _cloudBlobContainer = cloudBlobClient.GetContainerReference("wretched-hives-rules");

            var nameStartingLineProperties = new List<(string name, string startLine, int occurence)>
            {
                ("Auto", "#### Auto", 1),
                ("Defensive", "#### Defensive", 1),
                ("Dire", "#### Dire", 1),
                ("Disarming", "#### Disarming", 1),
                ("Disguised", "#### Disguised", 1),
                ("Disintegrate", "#### Disintegrate", 1),
                ("Disruptive", "#### Disruptive", 1),
                ("Keen", "#### Keen", 1),
                ("Mighty", "#### Mighty", 1),
                ("Piercing", "#### Piercing", 1),
                ("Rapid", "#### Rapid", 1),
                ("Shocking", "#### Shocking", 1),
                ("Silent", "#### Silent", 1),
                ("Vicious", "#### Vicious", 1)
            };

            _weaponPropertyProcessor = new WeaponPropertyProcessor(ContentType.ExpandedContent, nameStartingLineProperties);

            var armorProperties = new List<(string name, string startLine, int occurence)>
            {
                ("Absorptive", "#### Absorptive", 1),
                ("Agile", "#### Agile", 1),
                ("Anchor", "#### Anchor", 1),
                ("Avoidant", "#### Avoidant", 1),
                ("Barbed", "#### Barbed", 1),
                ("Charging", "#### Charging", 1),
                ("Concealing", "#### Concealing", 1),
                ("Cumbersome", "#### Cumbersome", 1),
                ("Gauntleted", "#### Gauntleted", 1),
                ("Imbalanced", "#### Imbalanced", 1),
                ("Impermeable", "#### Impermeable", 1),
                ("Insulated", "#### Insulated", 1),
                ("Interlocking", "#### Interlocking", 1),
                ("Lambent", "#### Lambent", 1),
                ("Lightweight", "#### Lightweight", 1),
                ("Magnetic", "#### Magnetic", 1),
                ("Obscured", "#### Obscured", 1),
                ("Powered", "#### Powered", 1)
            };

            _armorPropertyProcessor = new ArmorPropertyProcessor(ContentType.Core, armorProperties);
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
                    enhancedItem.ContentSourceEnum = ContentSource.WH;

                    var enhancedItemSearchTerm = _globalSearchTermRepository.CreateSearchTerm(enhancedItem.Name, GlobalSearchTermType.EnhancedItem, ContentType.Core,
                        $"/loot/enhancedItems?search={enhancedItem.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(enhancedItemSearchTerm);
                }

                await _tableStorage.AddBatchAsync<EnhancedItem>("enhancedItems", enhancedItems,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload WH enhanced items.");
            }

            try
            {
                var weaponProperties = await _weaponPropertyProcessor.Process(new List<string> {"WH.wh_05.txt"});

                await _tableStorage.AddBatchAsync<WeaponProperty>("weaponProperties", weaponProperties,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload WH weapon properties.");
            }

            try
            {
                var armorProperties =
                    await _armorPropertyProcessor.Process(new List<string> { "WH.wh_05.txt" });

                await _tableStorage.AddBatchAsync<ArmorProperty>("armorProperties", armorProperties,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload WH weapon properties.");
            }
        }
    }
}
