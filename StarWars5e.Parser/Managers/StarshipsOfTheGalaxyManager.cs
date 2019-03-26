using System.Threading.Tasks;
using StarWars5e.Models.Starship;
using StarWars5e.Parser.Parsers;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class StarshipsOfTheGalaxyManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly IStarshipBaseProcessor<StarshipDeployment> _starshipDeploymentProcessor;
        private readonly IStarshipBaseProcessor<StarshipEquipment> _starshipEquipmentProcessor;
        private readonly IStarshipBaseProcessor<StarshipModification> _starshipModificationProcessor;
        private readonly IStarshipBaseProcessor<StarshipBaseSize> _starshipSizeProcessor;
        private readonly IStarshipBaseProcessor<StarshipVenture> _starshipVentureProcessor;
        private readonly IStarshipBaseProcessor<StarshipChapterRules> _starshipChapterRulesProcessor;
        private const string StarshipsOfTheGalaxyFile = "sotg";

        public StarshipsOfTheGalaxyManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
            _starshipDeploymentProcessor = new StarshipDeploymentProcessor();
            _starshipEquipmentProcessor = new StarshipEquipmentProcessor();
            _starshipModificationProcessor = new StarshipModificationProcessor();
            _starshipSizeProcessor = new StarshipSizeProcessor();
            _starshipVentureProcessor = new StarshipVentureProcessor();
            _starshipChapterRulesProcessor = new StarshipChapterRulesProcessor();
        }

        public async Task Parse()
        {
            var rules = await _starshipChapterRulesProcessor.Process(StarshipsOfTheGalaxyFile);
            await _tableStorage.AddBatchAsync<StarshipChapterRules>("starshipRules", rules,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var deployments = await _starshipDeploymentProcessor.Process(StarshipsOfTheGalaxyFile);
            await _tableStorage.AddBatchAsync<StarshipDeployment>("starshipDeployments", deployments,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var equipment = await _starshipEquipmentProcessor.Process(StarshipsOfTheGalaxyFile);
            await _tableStorage.AddBatchAsync<StarshipEquipment>("starshipEquipment", equipment,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var modifications = await _starshipModificationProcessor.Process(StarshipsOfTheGalaxyFile);
            await _tableStorage.AddBatchAsync<StarshipModification>("starshipModifications", modifications,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var sizes = await _starshipSizeProcessor.Process(StarshipsOfTheGalaxyFile);
            await _tableStorage.AddBatchAsync<StarshipBaseSize>("starshipBaseSizes", sizes,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var ventures = await _starshipVentureProcessor.Process(StarshipsOfTheGalaxyFile);
            await _tableStorage.AddBatchAsync<StarshipVenture>("starshipVentures", ventures,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
        }
    }
}
