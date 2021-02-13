using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentVariantRulesManager
    {
        private readonly BlobContainerClient _blobContainerClient;
        private readonly ExpandedContentVariantRulesProcessor _expandedContentVariantRulesProcessor;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly List<string> _ecVariantRulesFileName = new List<string> { "ec_variantrules.txt" };
        private readonly ILocalization _localization;

        public ExpandedContentVariantRulesManager(IServiceProvider serviceProvider, ILocalization localization)
        {
            _localization = localization;
            _globalSearchTermRepository = serviceProvider.GetService<GlobalSearchTermRepository>();
            _expandedContentVariantRulesProcessor = new ExpandedContentVariantRulesProcessor();

            var blobServiceClient = serviceProvider.GetService<BlobServiceClient>();
            _blobContainerClient = blobServiceClient.GetBlobContainerClient($"variant-rules-{_localization.Language}");
        }

        public async Task Parse()
        {
            var rules = await _expandedContentVariantRulesProcessor.Process(_ecVariantRulesFileName, _localization);

            await _blobContainerClient.CreateIfNotExistsAsync();
            foreach (var variantRule in rules)
            {
                var json = JsonConvert.SerializeObject(variantRule);
                var blobClient = _blobContainerClient.GetBlobClient($"{variantRule.ChapterName}.json");

                var content = Encoding.UTF8.GetBytes(json);
                using (var ms = new MemoryStream(content))
                {
                    await blobClient.UploadAsync(ms, true);
                }

                var searchTerm = _globalSearchTermRepository.CreateSearchTerm(variantRule.ChapterName, GlobalSearchTermType.VariantRule,
                    ContentType.ExpandedContent, $"/rules/variantRules/{variantRule.ChapterName}");
                _globalSearchTermRepository.SearchTerms.Add(searchTerm);
            }
        }
    }
}
