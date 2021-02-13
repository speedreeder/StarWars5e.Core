using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentManager
    {
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly List<string> _expandedContentFileNames = new List<string>
        {
            "ec_02.txt", "ec_03.txt", "ec_04.txt", "ec_enhanced_items.txt",
            "ec_05.txt", "ec_06.txt", "ec_11.txt", "ec_12.txt", "ec_variantrules.txt"
        };
        private readonly ILocalization _localization;
        private readonly BlobContainerClient _blobContainerClient;

        public ExpandedContentManager(IServiceProvider serviceProvider, ILocalization localization)
        {
            _globalSearchTermRepository = serviceProvider.GetService<GlobalSearchTermRepository>();

            _localization = localization;

            var blobServiceClient = serviceProvider.GetService<BlobServiceClient>();
            _blobContainerClient = blobServiceClient.GetBlobContainerClient($"expanded-content-{_localization.Language}");
        }

        public async Task Parse()
        {
            var extendedContentChapters = new List<ChapterRules>();

            foreach (var ecFile in _expandedContentFileNames)
            {
                var expandedContentProcessor = new ExpandedContentProcessor();
                var extendedContent = await expandedContentProcessor.Process(new List<string> {ecFile}, _localization);

                if (extendedContent.SingleOrDefault() != null)
                {
                    extendedContentChapters.Add(extendedContent.Single());
                }
            }

            await _blobContainerClient.CreateIfNotExistsAsync();
            foreach (var ec in extendedContentChapters)
            {
                var json = JsonConvert.SerializeObject(ec);
                var blobClient = _blobContainerClient.GetBlobClient($"{ec.ChapterName}.json");

                var content = Encoding.UTF8.GetBytes(json);
                using (var ms = new MemoryStream(content))
                {
                    await blobClient.UploadAsync(ms, true);
                }

                var searchTerm = _globalSearchTermRepository.CreateSearchTerm(ec.ChapterName, GlobalSearchTermType.ExpandedContent,
                    ContentType.ExpandedContent, $"/rules/expandedContent/{ec.ChapterName}");
                _globalSearchTermRepository.SearchTerms.Add(searchTerm);
            }
        }
    }
}
