using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models;

namespace StarWars5e.Api.Managers
{
    public class ChapterRuleManager : IChapterRuleManager
    {
        private readonly CloudBlobClient _cloudBlobClient;

        public ChapterRuleManager(CloudBlobClient cloudBlobClient)
        {
            _cloudBlobClient = cloudBlobClient;
        }

        public async Task<List<ChapterRules>> GetChapterRulesFromBlobContainer(string containerName)
        {
            var container = _cloudBlobClient.GetContainerReference(containerName);
            var chapterRules = new List<ChapterRules>();
            BlobContinuationToken blobContinuationToken = null;
            do
            {
                var results = await container.ListBlobsSegmentedAsync(null, blobContinuationToken);
                blobContinuationToken = results.ContinuationToken;
                foreach (var item in results.Results)
                {
                    var blob = new CloudBlockBlob(item.Uri, container.ServiceClient);
                    var chapterRule = JsonConvert.DeserializeObject<ChapterRules>(await blob.DownloadTextAsync());
                    chapterRules.Add(chapterRule);
                }
            } while (blobContinuationToken != null);

            return chapterRules;
        }

        public async Task<ChapterRules> GetChapterRuleFromBlobContainer(string containerName, string chapterName)
        {
            var container = _cloudBlobClient.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(chapterName);
            var chapterRule = JsonConvert.DeserializeObject<ChapterRules>(await blob.DownloadTextAsync());

            return chapterRule;
        }
    }
}
