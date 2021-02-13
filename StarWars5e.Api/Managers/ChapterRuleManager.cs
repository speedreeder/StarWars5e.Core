using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models;
using StarWars5e.Models.Enums;

namespace StarWars5e.Api.Managers
{
    public class ChapterRuleManager : IChapterRuleManager
    {
        private readonly IConfiguration _configuration;

        public ChapterRuleManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<ChapterRules>> GetChapterRulesFromBlobContainer(string containerName, Language language)
        {
            var blobContainerClient = new BlobContainerClient(_configuration["StorageAccountConnectionString"], $"{containerName}-{language}");

            if (!await blobContainerClient.ExistsAsync())
            {
                blobContainerClient = new BlobContainerClient(_configuration["StorageAccountConnectionString"], $"{containerName}-{Language.en}");
            }

            var chapterRules = new List<ChapterRules>();
            var chapterBlobs = new List<BlobItem>();

            await foreach (var blob in blobContainerClient.GetBlobsAsync())
            {
                chapterBlobs.Add(blob);
            }

            foreach (var chapterBlob in chapterBlobs)
            {
                var blobClient = blobContainerClient.GetBlobClient(chapterBlob.Name);

                var stream = new MemoryStream();
                await blobClient.DownloadToAsync(stream);

                stream.Seek(0, SeekOrigin.Begin);

                var chapterRule = JsonConvert.DeserializeObject<ChapterRules>(await new StreamReader(stream).ReadToEndAsync());
                chapterRules.Add(chapterRule);
            }

            return chapterRules;
        }

        public async Task<ChapterRules> GetChapterRuleFromBlobContainer(string containerName, string chapterName, Language language)
        {
            var blobContainerClient = new BlobContainerClient(_configuration["StorageAccountConnectionString"], $"{containerName}-{language}");

            if (!await blobContainerClient.ExistsAsync())
            {
                blobContainerClient = new BlobContainerClient(_configuration["StorageAccountConnectionString"], $"{containerName}-{Language.en}");
            }

            var blobClient = blobContainerClient.GetBlobClient(chapterName);

            var stream = new MemoryStream();
            await blobClient.DownloadToAsync(stream);

            stream.Seek(0, SeekOrigin.Begin);

            var chapterRule = JsonConvert.DeserializeObject<ChapterRules>(await new StreamReader(stream).ReadToEndAsync());

            return chapterRule;
        }
    }
}
