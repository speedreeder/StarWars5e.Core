using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentManager
    {
        private readonly CloudBlobContainer _cloudBlobContainer;
        private readonly ExpandedContentProcessor _expandedContentProcessor;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly List<string> _expandedContentFileNames = new List<string>
        {
            "ec_archetypes.txt", "ec_backgrounds.txt", "ec_customization_options.txt", "ec_enhanced_items.txt",
            "ec_equipment.txt", "ec_force_powers.txt", "ec_species.txt", "ec_tech_powers.txt", "ec_variantrules.txt"
        };
        private readonly ILocalization _localization;

        public ExpandedContentManager(CloudStorageAccount cloudStorageAccount, ILocalization localization, GlobalSearchTermRepository globalSearchTermRepository)
        {
            _localization = localization;
            _globalSearchTermRepository = globalSearchTermRepository;
            _expandedContentProcessor = new ExpandedContentProcessor();

            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            _cloudBlobContainer = cloudBlobClient.GetContainerReference($"expanded-content-{_localization.Language}");
        }

        public async Task Parse()
        {
            var extendedContentChapters = new List<ChapterRules>();

            foreach (var ecFile in _expandedContentFileNames)
            {
                var extendedContent = await _expandedContentProcessor.Process(new List<string> {ecFile}, _localization);

                if (extendedContent.SingleOrDefault() != null)
                {
                    extendedContentChapters.Add(extendedContent.Single());
                }
            }

            await _cloudBlobContainer.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Off, null, null);
            foreach (var ec in extendedContentChapters)
            {
                var json = JsonConvert.SerializeObject(ec);
                var blob = _cloudBlobContainer.GetBlockBlobReference($"{ec.ChapterName}.json");

                await blob.UploadTextAsync(json);

                var searchTerm = _globalSearchTermRepository.CreateSearchTerm(ec.ChapterName, GlobalSearchTermType.ExpandedContent,
                    ContentType.ExpandedContent, $"/rules/expandedContent/{ec.ChapterName}");
                _globalSearchTermRepository.SearchTerms.Add(searchTerm);
            }
        }
    }
}
