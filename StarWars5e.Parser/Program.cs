using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.Azure.Search;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Localization;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser
{
    public class Program
    {
        private static readonly string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        private const string ApplicationName = "SW5E Sheets API";

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
                .AddSingleton(globalSearchTermRepository);
            
            if (!string.IsNullOrWhiteSpace(config["GoogleApiClientId"]))
            {
                var clientSecrets = new ClientSecrets
                {
                    ClientId = config["GoogleApiClientId"],
                    ClientSecret = config["GoogleApiClientSecret"]
                };

                var googleCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    clientSecrets,
                    Scopes,
                    "user",
                    CancellationToken.None);

                // Create Google Sheets API service.
                var sheetsService = new SheetsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = googleCredential,
                    ApplicationName = ApplicationName
                });

                serviceProvider.AddSingleton(sheetsService);
            }

            if (!string.IsNullOrWhiteSpace(config["SearchKey"]))
            {
                var searchClient = new SearchServiceClient("sw5esearch", new SearchCredentials(config["SearchKey"]));
                serviceProvider.AddSingleton(searchClient);
            }

            var serviceProviderBuilt = serviceProvider.BuildServiceProvider();

            var languages = config["Languages"].Split(',');

            if (!languages.Any())
            {
                Console.WriteLine("No languages found.");
                return;
            }

            foreach (var language in languages)
            {
                var languageEnum = Enum.TryParse<Language>(language, true, out var parsedLanguage) ? parsedLanguage : Language.None;

                if (languageEnum == Language.None)
                {
                    Console.WriteLine($"Language {language} not supported.");
                    return;
                }

                var stringsClass = LocalizationFactory.Get(languageEnum);

                await ParseContent.Parse(serviceProviderBuilt.GetService<ITableStorage>(),
                    serviceProviderBuilt.GetService<CloudStorageAccount>(),
                    serviceProviderBuilt.GetService<GlobalSearchTermRepository>(), stringsClass,
                    serviceProviderBuilt.GetService<SearchServiceClient>());
            }
        }
    }
}
