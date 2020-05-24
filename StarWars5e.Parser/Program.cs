using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Globalization;
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

            var languages = config["Languages"].Split(',');

            if (!languages.Any())
            {
                Console.WriteLine("No languages found.");
                return;
            }

            foreach (var language in languages)
            {
                var languageEnum = Enum.TryParse<Language>(language, out var parsedLanguage) ? parsedLanguage : Language.None;

                if (languageEnum == Language.None)
                {
                    Console.WriteLine($"Language {language} not supported.");
                    return;
                }

                var stringsClass = GlobalizationFactory.Get(languageEnum);

                await ParseContent.Parse(serviceProvider.GetService<ITableStorage>(),
                    serviceProvider.GetService<CloudStorageAccount>(),
                    serviceProvider.GetService<GlobalSearchTermRepository>(), stringsClass);
            }
        }
    }
}
