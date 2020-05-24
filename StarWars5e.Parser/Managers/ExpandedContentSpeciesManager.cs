using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Lookup;
using StarWars5e.Models.Species;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentSpeciesManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly List<string> _ecSpeciesFileName = new List<string> { "ec_species.txt" };
        private readonly ILocalization _localization;

        public ExpandedContentSpeciesManager(ITableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository, ILocalization localization)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _localization = localization;
        }

        public async Task Parse()
        {
            try
            {
                var speciesImageUrlsLU = await _tableStorage.GetAllAsync<SpeciesImageUrlLU>("speciesImageUrlsLU");
                var speciesProcessor = new ExpandedContentSpeciesProcessor(_localization, speciesImageUrlsLU.ToList());

                var species = await speciesProcessor.Process(_ecSpeciesFileName, _localization);

                foreach (var specie in species)
                {
                    specie.ContentSourceEnum = ContentSource.EC;

                    var specieSearchTerm = _globalSearchTermRepository.CreateSearchTerm(specie.Name, GlobalSearchTermType.Species, ContentType.ExpandedContent,
                        $"/characters/species/{specie.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(specieSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Species>($"species{_localization.Language}", species,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload EC species.");
            }
        }
    }
}
