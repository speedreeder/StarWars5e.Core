using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;
using StarWars5e.Parser.Storage;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentTechPowersManager
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly List<string> _ecTechPowersFileName = new() { "ec_11.txt" };
        private readonly ILocalization _localization;

        public ExpandedContentTechPowersManager(IAzureTableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository, ILocalization localization)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _localization = localization;
        }

        public async Task<List<Power>> Parse()
        {
            var techPowers = new List<Power>();
            try
            {
                var techPowersProcessor = new ExpandedContentTechPowersProcessor();
                techPowers = await techPowersProcessor.Process(_ecTechPowersFileName, _localization);

                foreach (var techPower in techPowers)
                {
                    techPower.ContentSourceEnum = ContentSource.EC;

                    var forcePowerSearchTerm = _globalSearchTermRepository.CreateSearchTerm(techPower.Name,
                        GlobalSearchTermType.ForcePower, ContentType.ExpandedContent,
                        $"/characters/techPowers/?search={techPower.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(forcePowerSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Power>($"powers{_localization.Language}", techPowers,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload EC force powers.");
            }

            return techPowers;
        }
    }
}
