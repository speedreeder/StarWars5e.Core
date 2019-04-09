using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
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
                .Build();

            var tableStorage = new AzureTableStorage(config["StorageAccountConnectionString"]);
            
            var storageAccount = CloudStorageAccount.Parse(config["StorageAccountConnectionString"]);

            var serviceProvider = new ServiceCollection()
                .AddSingleton<ITableStorage>(tableStorage)
                .AddSingleton(storageAccount)
                .BuildServiceProvider();

            var starshipManager = new StarshipsOfTheGalaxyManager(serviceProvider.GetService<ITableStorage>(), serviceProvider.GetService<CloudStorageAccount>());
            var monsterManualManager = new MonsterManualManager(serviceProvider.GetService<ITableStorage>());
            var extendedContentSpeciesManager = new ExpandedContentSpeciesManager(serviceProvider.GetService<ITableStorage>());
            var extendedContentBackgroundManager = new ExpandedContentBackgroundsManager(serviceProvider.GetService<ITableStorage>());
            var extendedContentEquipmentManager = new ExpandedContentEquipmentManager(serviceProvider.GetService<ITableStorage>());
            var extendedContentArchetypesManager = new ExpandedContentArchetypesManager(serviceProvider.GetService<ITableStorage>());
            var playerHandbookManager = new PlayerHandbookManager(serviceProvider.GetService<ITableStorage>(), serviceProvider.GetService<CloudStorageAccount>());

            await starshipManager.Parse();
            await monsterManualManager.Parse();
            await extendedContentSpeciesManager.Parse();
            await extendedContentBackgroundManager.Parse();
            await extendedContentEquipmentManager.Parse();
            await extendedContentArchetypesManager.Parse();
            await playerHandbookManager.Parse();
        }
    }
}
