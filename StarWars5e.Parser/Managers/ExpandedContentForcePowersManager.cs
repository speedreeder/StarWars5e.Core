using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentForcePowersManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly List<string> _ecForcePowersFileName = new List<string> { "ec_12.txt" };
        private readonly ILocalization _localization;

        public ExpandedContentForcePowersManager(ITableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository, ILocalization localization)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _localization = localization;
        }

        public async Task Parse()
        {
            try
            {
                var forcePowersProcessor = new ExpandedContentForcePowersProcessor();
                var forcePowers = await forcePowersProcessor.Process(_ecForcePowersFileName, _localization);

                foreach (var forcePower in forcePowers)
                {
                    forcePower.ContentSourceEnum = ContentSource.EC;

                    var forcePowerSearchTerm = _globalSearchTermRepository.CreateSearchTerm(forcePower.Name,
                        GlobalSearchTermType.ForcePower, ContentType.ExpandedContent,
                        $"/characters/forcePowers/?search={forcePower.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(forcePowerSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Power>($"powers{_localization.Language}", forcePowers,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload EC force powers.");
            }
        }
    }
}
