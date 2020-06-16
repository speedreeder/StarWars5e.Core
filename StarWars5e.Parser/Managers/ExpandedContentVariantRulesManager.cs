using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentVariantRulesManager
    {
        private readonly CloudBlobContainer _cloudBlobContainer;
        private readonly ExpandedContentVariantRulesProcessor _expandedContentVariantRulesProcessor;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly List<string> _ecVariantRulesFileName = new List<string> { "ec_variantrules.txt" };
        private readonly ILocalization _localization;

        public ExpandedContentVariantRulesManager(CloudStorageAccount cloudStorageAccount, ILocalization localization, GlobalSearchTermRepository globalSearchTermRepository)
        {
            _localization = localization;
            _globalSearchTermRepository = globalSearchTermRepository;
            _expandedContentVariantRulesProcessor = new ExpandedContentVariantRulesProcessor();

            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            _cloudBlobContainer = cloudBlobClient.GetContainerReference($"variant-rules-{_localization.Language}");
        }

        public async Task Parse()
        {
            var rules = await _expandedContentVariantRulesProcessor.Process(_ecVariantRulesFileName, _localization);
            await _cloudBlobContainer.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Off, null, null);
            foreach (var variantRule in rules)
            {
                var json = JsonConvert.SerializeObject(variantRule);
                var blob = _cloudBlobContainer.GetBlockBlobReference($"{variantRule.ChapterName}.json");

                await blob.UploadTextAsync(json);

                var searchTerm = _globalSearchTermRepository.CreateSearchTerm(variantRule.ChapterName, GlobalSearchTermType.VariantRule,
                    ContentType.ExpandedContent, $"/rules/variantRules/{variantRule.ChapterName}");
                _globalSearchTermRepository.SearchTerms.Add(searchTerm);
            }
        }
    }
}
