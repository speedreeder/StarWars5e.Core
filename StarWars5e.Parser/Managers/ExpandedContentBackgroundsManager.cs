using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Models.Background;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentBackgroundsManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly ExpandedContentBackgroundProcessor _backgroundProcessor;
        private readonly List<string> _ecBackgroundsFileName = new List<string> {"ec_backgrounds.txt"};
        private readonly ILocalization _localization;

        public ExpandedContentBackgroundsManager(ITableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository, ILocalization localization)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _localization = localization;
            _backgroundProcessor = new ExpandedContentBackgroundProcessor();
        }

        public async Task Parse()
        {
            try
            {
                var backgrounds = await _backgroundProcessor.Process(_ecBackgroundsFileName, _localization);

                foreach (var background in backgrounds)
                {
                    background.ContentSourceEnum = ContentSource.EC;

                    var backgroundSearchTerm = _globalSearchTermRepository.CreateSearchTerm(background.Name,
                        GlobalSearchTermType.Background, ContentType.ExpandedContent,
                        $"/characters/backgrounds/{background.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(backgroundSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Background>($"backgrounds{_localization.Language}", backgrounds,
                    new BatchOperationOptions {BatchInsertMethod = BatchInsertMethod.InsertOrReplace});

            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload EC backgrounds.");
            }
        }
    }
}
