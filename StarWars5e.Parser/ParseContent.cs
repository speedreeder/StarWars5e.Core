using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure.Search.Documents.Indexes;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Managers;
using StarWars5e.Parser.Storage;

namespace StarWars5e.Parser
{
    public static class ParseContent
    {
        public static async Task Parse(IServiceProvider serviceProvider, IAzureTableStorage azureTableStorage,
            GlobalSearchTermRepository globalSearchTermRepository, ILocalization localization)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();

            var playerHandbookManager = new PlayerHandbookManager(serviceProvider, localization);
            var wretchedHivesManager = new WretchedHivesManager(serviceProvider, localization);
            var starshipManager = new StarshipsOfTheGalaxyManager(serviceProvider, localization);
            var monsterManualManager =
                new MonsterManualManager(azureTableStorage, globalSearchTermRepository, localization);
            var extendedContentSpeciesManager =
                new ExpandedContentSpeciesManager(serviceProvider, localization);
            var extendedContentBackgroundManager =
                new ExpandedContentBackgroundsManager(azureTableStorage, globalSearchTermRepository, localization);
            var extendedContentEquipmentManager =
                new ExpandedContentEquipmentManager(azureTableStorage, globalSearchTermRepository, localization);
            var extendedContentArchetypesManager =
                new ExpandedContentArchetypesManager(serviceProvider, localization);
            var extendedContentVariantRulesManager =
                new ExpandedContentVariantRulesManager(serviceProvider, localization);
            var expandedContentManager =
                new ExpandedContentManager(serviceProvider, localization);
            var extendedContentCustomizationOptionsManager =
                new ExpandedContentCustomizationOptionsManager(azureTableStorage, globalSearchTermRepository,
                    localization);
            var extendedContentForcePowersManager =
                new ExpandedContentForcePowersManager(azureTableStorage, globalSearchTermRepository, localization);
            var extendedContentTechPowersManager =
                new ExpandedContentTechPowersManager(azureTableStorage, globalSearchTermRepository, localization);
            var referenceTableManager = new ReferenceTableManager(azureTableStorage, localization);
            var creditsManager = new CreditsManager(serviceProvider, localization);
            var extendedContentEnhancedItemManager =
                new ExpandedContentEnhancedItemsManager(azureTableStorage, globalSearchTermRepository, localization);

            var referenceTables = await referenceTableManager.Parse();
            var powers = await playerHandbookManager.Parse();
            powers.AddRange(await extendedContentTechPowersManager.Parse());
            powers.AddRange(await extendedContentForcePowersManager.Parse());
            await wretchedHivesManager.Parse();
            await starshipManager.Parse(referenceTables);
            await monsterManualManager.Parse(powers);
            await extendedContentSpeciesManager.Parse();
            await extendedContentBackgroundManager.Parse();
            await extendedContentEquipmentManager.Parse();
            await extendedContentArchetypesManager.Parse();
            await extendedContentVariantRulesManager.Parse();
            await expandedContentManager.Parse();
            await extendedContentCustomizationOptionsManager.Parse();
            await creditsManager.Parse();
            await extendedContentEnhancedItemManager.Parse();

            try
            {
                var searchServiceClient = serviceProvider.GetService<SearchIndexClient>();

                if (searchServiceClient != null)
                {
                    var searchManager = new SearchManager(serviceProvider, localization);
                    await searchManager.Upload();
                }
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
                var features = serviceProvider.GetService<FeatureRepository>()?.Features;
                var sheetOperations = new SheetOperations(serviceProvider);

                if (features != null && serviceProvider.GetService<IConfiguration>()?["FeaturesSheetId"] != null &&
                    configuration != null &&
                    configuration["FeatureLanguages"].Split(',').Contains(localization.Language.ToString()))
                {
                    var featureSheetData = features.Select(c => new List<object> {c.RowKey, c.Level} as IList<object>)
                        .ToList();
                    await sheetOperations.UpdateFeatureSheetAsync(featureSheetData);
                    Console.WriteLine("Successfully wrote features to Features Parsed sheet.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to write features to sheet.");
            }

            try
            {
                var currentVersion =
                    (await azureTableStorage.GetAsync<DataVersion>($"dataVersion{localization.Language}", ContentType.Core.ToString(),
                        "MASTERVERSION"))?.Version;

                var dataNames = new List<string>
                {
                    "MASTERVERSION", "archetypes", "armorProperties", "backgrounds", "classes", "credits",
                    "enhancedItems", "equipment", "feats", "features", "monsters", "powers", "referenceTables", "species", "starshipBaseSizes",
                    "starshipDeployments", "starshipEquipment", "starshipModifications", "starshipVentures", "weaponProperties",
                    "player-handbook-rules", "starships-rules", "variant-rules", "wretched-hives-rules",
                    "characterAdvancementLU", "conditionsLU", "featureDataLU", "featureLevelLU", "skillsLU", "fightingStyle",
                    "fightingMastery", "expanded-content"
                };
                var dataVersions = dataNames.Select(d => new DataVersion
                {
                    LastUpdated = DateTime.Now,
                    Name = d,
                    PartitionKey = ContentType.Core.ToString(),
                    RowKey = d,
                    Version = currentVersion + 1 ?? 1
                });

                await azureTableStorage.AddBatchAsync<DataVersion>($"dataVersion{localization.Language}", dataVersions,
                    new BatchOperationOptions {BatchInsertMethod = BatchInsertMethod.InsertOrReplace});
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to update data versions.");
            }
        }
    }
}
