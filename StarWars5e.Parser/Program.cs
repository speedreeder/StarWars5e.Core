using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azure;
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
        public static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json", true, true)
                .AddUserSecrets<Program>()
                .Build();

            var tableStorage = new AzureTableStorage(config["StorageAccountConnectionString"], true);
            
            var storageAccount = CloudStorageAccount.Parse(config["StorageAccountConnectionString"]);
            var cloudBlobClient = new BlobServiceClient(config["StorageAccountConnectionString"]);

            var globalSearchTermRepository = new GlobalSearchTermRepository();
            var featureRepository = new FeatureRepository();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IAzureTableStorage>(tableStorage)
                .AddSingleton(storageAccount)
                .AddSingleton(globalSearchTermRepository)
                .AddSingleton(featureRepository)
                .AddSingleton(cloudBlobClient)
                .AddSingleton<IConfiguration>(config);

            await using var googleCredStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("StarWars5e.Parser.google_credentials.json");

            if (googleCredStream != null && googleCredStream.Length > 0)
            {
                await using var stream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("StarWars5e.Parser.google_credentials.json");
                var googleCredential = GoogleCredential.FromStream(stream).CreateScoped(SheetsService.ScopeConstants.Spreadsheets);

                var sheetsService = new SheetsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = googleCredential,
                    ApplicationName = "SW5E Sheets API"
                });

                serviceProvider.AddSingleton(sheetsService);
            }

            if (!string.IsNullOrWhiteSpace(config["SearchKey"]))
            {
                var searchEndpoint = config["SearchEndpoint"] ?? "https://sw5esearch.search.windows.net";
                var searchIndexClient = new SearchIndexClient(new Uri(searchEndpoint),
                    new AzureKeyCredential(config["SearchKey"]));
                var searchIndexerClient = new SearchIndexerClient(new Uri(searchEndpoint),
                    new AzureKeyCredential(config["SearchKey"]));

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
                    serviceProviderBuilt.GetService<GlobalSearchTermRepository>(), stringsClass);
            }
        }
    }
}
