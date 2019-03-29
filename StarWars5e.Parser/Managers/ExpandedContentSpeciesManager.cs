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
        private const string EcSpeciesFileName = "EC_Species";

        public ExpandedContentSpeciesManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
            _speciesProcessor = new SpeciesProcessor();
        }

        public async Task Parse()
        {
            var species = await _speciesProcessor.Process(EcSpeciesFileName);
            await _tableStorage.AddBatchAsync<Species>("species", species,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
        }
    }
}
