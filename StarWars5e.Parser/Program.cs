using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Parser.Managers;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json", true, true)
                .AddUserSecrets<Program>()
                .Build();

            var tableStorage = new AzureTableStorage(config["StorageAccountConnectionString"]);
            
            var storageAccount = CloudStorageAccount.Parse(config["StorageAccountConnectionString"]);
            var globalSearchTermRepository = new GlobalSearchTermRepository();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<ITableStorage>(tableStorage)
                .AddSingleton(storageAccount)
                .AddSingleton(globalSearchTermRepository)
                .BuildServiceProvider();

            var starshipManager = new StarshipsOfTheGalaxyManager(serviceProvider.GetService<ITableStorage>(), serviceProvider.GetService<CloudStorageAccount>());
            var monsterManualManager = new MonsterManualManager(serviceProvider.GetService<ITableStorage>());
            var extendedContentSpeciesManager = new ExpandedContentSpeciesManager(serviceProvider.GetService<ITableStorage>());
            var extendedContentBackgroundManager = new ExpandedContentBackgroundsManager(serviceProvider.GetService<ITableStorage>());
            var extendedContentEquipmentManager = new ExpandedContentEquipmentManager(serviceProvider.GetService<ITableStorage>());
            var extendedContentArchetypesManager = new ExpandedContentArchetypesManager(serviceProvider.GetService<ITableStorage>());
            var extendedContentVariantRulesManager = new ExpandedContentVariantRulesManager(serviceProvider.GetService<CloudStorageAccount>());
            var playerHandbookManager = new PlayerHandbookManager(serviceProvider.GetService<ITableStorage>(), serviceProvider.GetService<CloudStorageAccount>(), globalSearchTermRepository);
            var referenceTableManager = new ReferenceTableManager(serviceProvider.GetService<ITableStorage>());
            var searchManager = new SearchManager(serviceProvider.GetService<ITableStorage>(), serviceProvider.GetService<GlobalSearchTermRepository>());

            var referenceTables = await referenceTableManager.Parse();
            await starshipManager.Parse(referenceTables);
            await monsterManualManager.Parse();
            await extendedContentSpeciesManager.Parse();
            await extendedContentBackgroundManager.Parse();
            await extendedContentEquipmentManager.Parse();
            await extendedContentArchetypesManager.Parse();
            await extendedContentVariantRulesManager.Parse();
            await playerHandbookManager.Parse();
            await searchManager.Upload();
        }
    }
}
