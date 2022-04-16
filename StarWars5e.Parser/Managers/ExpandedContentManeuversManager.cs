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
    public class ExpandedContentManeuversManager
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly List<string> _ecManeuversFileName = new() { "ec_13.txt" };
        private readonly ILocalization _localization;

        public ExpandedContentManeuversManager(IAzureTableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository, ILocalization localization)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _localization = localization;
        }

        public async Task<List<Maneuver>> Parse()
        {
            var maneuvers = new List<Maneuver>();
            try
            {
                var maneuversProcessor = new ExpandedContentManeuversProcessor(_localization);
                maneuvers = await maneuversProcessor.Process(_ecManeuversFileName, _localization, ContentType.ExpandedContent);

                foreach (var maneuver in maneuvers)
                {
                    maneuver.ContentSourceEnum = ContentSource.EC;

                    var maneuverSearchTerm = _globalSearchTermRepository.CreateSearchTerm(maneuver.Name,
                        GlobalSearchTermType.Maneuver, ContentType.ExpandedContent,
                        $"/characters/maneuvers/?search={maneuver.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(maneuverSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Maneuver>($"maneuvers{_localization.Language}", maneuvers,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload EC maneuvers.");
            }

            return maneuvers;
        }
    }
}
