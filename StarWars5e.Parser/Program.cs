using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Storage.Blobs;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Storage;

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
            var cloudBlobClient = new BlobServiceClient(config["StorageAccountConnectionString"]);

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IAzureTableStorage>(tableStorage)
                .AddSingleton(storageAccount)
                .AddSingleton(globalSearchTermRepository)
                .AddSingleton(cloudBlobClient);
            
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

                var sheetsService = new SheetsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = googleCredential,
                    ApplicationName = ApplicationName
                });

                serviceProvider.AddSingleton(sheetsService);
            }

            if (!string.IsNullOrWhiteSpace(config["SearchKey"]))
            {
                var searchIndexClient = new SearchIndexClient(new Uri("https://sw5esearch.search.windows.net"),
                    new AzureKeyCredential(config["SearchKey"]));
                //var searchClient = new SearchClient(new Uri("https://sw5esearch.search.windows.net"),
                //    "searchterms-index",
                //    new AzureKeyCredential(config["SearchKey"]));
                var searchIndexerClient = new SearchIndexerClient(new Uri("https://sw5esearch.search.windows.net"),
                    new AzureKeyCredential(config["SearchKey"]));

                //serviceProvider.AddSingleton(searchClient);
                serviceProvider.AddSingleton(searchIndexClient);
                serviceProvider.AddSingleton(searchIndexerClient);
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

                await ParseContent.Parse(serviceProviderBuilt, serviceProviderBuilt.GetService<IAzureTableStorage>(),
                    serviceProviderBuilt.GetService<CloudStorageAccount>(),
                    serviceProviderBuilt.GetService<GlobalSearchTermRepository>(), stringsClass);
            }
        }
    }
}
