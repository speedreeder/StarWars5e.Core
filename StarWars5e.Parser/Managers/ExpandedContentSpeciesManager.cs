using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models.Species;
using StarWars5e.Parser.Parsers;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentSpeciesManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly IBaseProcessor<Species> _speciesProcessor;
        private readonly List<string> _ecSpeciesFileName = new List<string> { "EC_Species.md" };

        public ExpandedContentSpeciesManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
            _speciesProcessor = new ExpandedContentSpeciesProcessor();
        }

        public async Task Parse()
        {
            var species = await _speciesProcessor.Process(_ecSpeciesFileName);
            await _tableStorage.AddBatchAsync<Species>("species", species,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
        }
    }
}
