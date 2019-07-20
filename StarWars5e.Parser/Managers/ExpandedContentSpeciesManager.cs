using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Species;
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

        public ExpandedContentSpeciesManager(ITableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _speciesProcessor = new ExpandedContentSpeciesProcessor();
        }

        public async Task Parse()
        {
            try
            {
                var species = await _speciesProcessor.Process(_ecSpeciesFileName);

                foreach (var specie in species)
                {
                    var specieSearchTerm = _globalSearchTermRepository.CreateSearchTerm(specie.Name, GlobalSearchTermType.Species, ContentType.ExpandedContent,
                        $"/reference/species/{specie.Name}");
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
