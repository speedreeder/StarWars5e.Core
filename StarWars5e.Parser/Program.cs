using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            var tableStorage = new AzureTableStorage(config["TableStorageConnectionString"]);

            var serviceProvider = new ServiceCollection()
                .AddSingleton<ITableStorage>(tableStorage)
                .BuildServiceProvider();

            var starshipManager = new StarshipsOfTheGalaxyManager(serviceProvider.GetService<ITableStorage>());
            var speciesManager = new ExpandedContentSpeciesManager(serviceProvider.GetService<ITableStorage>());
            var monsterManualManager = new MonsterManualManager(serviceProvider.GetService<ITableStorage>());
            var extendedContentSpeciesManager = new ExpandedContentSpeciesManager(serviceProvider.GetService<ITableStorage>());
            var extendedContentBackgroundManager = new ExpandedContentBackgroundsManager(serviceProvider.GetService<ITableStorage>());

            await starshipManager.Parse();
            await speciesManager.Parse();
            await monsterManualManager.Parse();
            await extendedContentSpeciesManager.Parse();
            await extendedContentBackgroundManager.Parse();
        }
    }
}
