using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Globalization;
using StarWars5e.Parser.Managers;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser
{
    public static class ParseContent
    {
        public static async Task Parse(ITableStorage azureTableStorage, CloudStorageAccount cloudStorageAccount,
            GlobalSearchTermRepository globalSearchTermRepository, IGlobalization globalization)
        {
            var starshipManager = new StarshipsOfTheGalaxyManager(azureTableStorage, cloudStorageAccount,
                globalSearchTermRepository, globalization);
            var monsterManualManager =
                new MonsterManualManager(azureTableStorage, globalSearchTermRepository, globalization);
            var extendedContentSpeciesManager =
                new ExpandedContentSpeciesManager(azureTableStorage, globalSearchTermRepository, globalization);
            var extendedContentBackgroundManager =
                new ExpandedContentBackgroundsManager(azureTableStorage, globalSearchTermRepository, globalization);
            var extendedContentEquipmentManager =
                new ExpandedContentEquipmentManager(azureTableStorage, globalSearchTermRepository, globalization);
            var extendedContentArchetypesManager =
                new ExpandedContentArchetypesManager(azureTableStorage, globalSearchTermRepository, globalization);
            var extendedContentVariantRulesManager =
                new ExpandedContentVariantRulesManager(cloudStorageAccount, globalization);
            var extendedContentCustomizationOptionsManager =
                new ExpandedContentCustomizationOptionsManager(azureTableStorage, globalSearchTermRepository,
                    globalization);
            var extendedContentForcePowersManager =
                new ExpandedContentForcePowersManager(azureTableStorage, globalSearchTermRepository, globalization);
            var playerHandbookManager = new PlayerHandbookManager(azureTableStorage, cloudStorageAccount,
                globalSearchTermRepository, globalization);
            var referenceTableManager = new ReferenceTableManager(azureTableStorage, globalization);
            var wretchedHivesManager = new WretchedHivesManager(azureTableStorage, cloudStorageAccount,
                globalSearchTermRepository, globalization);
            var creditsManager = new CreditsManager(cloudStorageAccount, globalization);
            var extendedContentEnhancedItemManager =
                new ExpandedContentEnhancedItemsManager(azureTableStorage, globalSearchTermRepository, globalization);

            var referenceTables = await referenceTableManager.Parse();
            await starshipManager.Parse(referenceTables);
            await monsterManualManager.Parse();
            await extendedContentSpeciesManager.Parse();
            await extendedContentBackgroundManager.Parse();
            await extendedContentEquipmentManager.Parse();
            await extendedContentArchetypesManager.Parse();
            await extendedContentVariantRulesManager.Parse();
            await extendedContentCustomizationOptionsManager.Parse();
            await extendedContentForcePowersManager.Parse();
            await wretchedHivesManager.Parse();
            await playerHandbookManager.Parse();
            await creditsManager.Parse();
            await extendedContentEnhancedItemManager.Parse();

            try
            {
                var searchManager = new SearchManager(azureTableStorage, globalSearchTermRepository);
                await searchManager.Upload();
            }
            catch (StorageException)
            {
                var searchTerms = globalSearchTermRepository.SearchTerms;
                var dupes = searchTerms
                    .GroupBy(i => i.RowKey)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key);

                foreach (var dupe in dupes)
                {
                    Console.WriteLine($"Dupe: {dupe}");
                }

                var nonConformingNames = searchTerms.Where(s => Regex.IsMatch(s.RowKey, @"[\\]|[/]|[#]|[?] "));
                foreach (var nonConformingName in nonConformingNames)
                {
                    Console.WriteLine($"Non-conforming: {nonConformingName}");
                }

                Console.WriteLine("Failed to upload search terms.");
            }

            try
            {
                var currentVersion =
                    (await azureTableStorage.GetAsync<DataVersion>("dataVersion", ContentType.Core.ToString(),
                        "MASTERVERSION")).Version;

                var dataNames = new List<string>
                {
                    "MASTERVERSION", "archetypes", "armorProperties", "backgrounds", "classes", "credits",
                    "enhancedItems", "equipment",
                    "feats", "features", "monsters", "powers", "referenceTables", "species", "starshipBaseSizes",
                    "starshipDeployments", "starshipEquipment", "starshipModifications", "starshipVentures",
                    "weaponProperties",
                    "player-handbook-rules", "starships-rules", "variant-rules", "wretched-hives-rules",
                    "characterAdvancementLU", "conditionsLU", "featureDataLU", "featureLevelLU", "skillsLU"
                };
                var dataVersions = dataNames.Select(d => new DataVersion
                {
                    LastUpdated = DateTime.Now,
                    Name = d,
                    PartitionKey = ContentType.Core.ToString(),
                    RowKey = d,
                    Version = currentVersion + 1
                });

                await azureTableStorage.AddBatchAsync<DataVersion>("dataVersion", dataVersions,
                    new BatchOperationOptions {BatchInsertMethod = BatchInsertMethod.InsertOrReplace});
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to update data versions.");
            }
        }
    }
}
