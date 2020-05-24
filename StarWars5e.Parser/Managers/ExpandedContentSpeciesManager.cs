using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Species;
using StarWars5e.Parser.Globalization;
using StarWars5e.Parser.Parsers;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentSpeciesManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly IBaseProcessor<Species> _speciesProcessor;
        private readonly List<string> _ecSpeciesFileName = new List<string> { "ec_species.txt" };
        private readonly IGlobalization _globalization;

        public ExpandedContentSpeciesManager(ITableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository, IGlobalization globalization)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _globalization = globalization;
            _speciesProcessor = new ExpandedContentSpeciesProcessor();
        }

        public async Task Parse()
        {
            try
            {
                var species = await _speciesProcessor.Process(_ecSpeciesFileName, _globalization);

                foreach (var specie in species)
                {
                    specie.ContentSourceEnum = ContentSource.EC;

                    var specieSearchTerm = _globalSearchTermRepository.CreateSearchTerm(specie.Name, GlobalSearchTermType.Species, ContentType.ExpandedContent,
                        $"/characters/species/{specie.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(specieSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Species>("species", species,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload EC species.");
            }
        }
    }
}
