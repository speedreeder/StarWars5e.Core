using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models.Starship;
using StarWars5e.Parser.Parsers;
using StarWars5e.Parser.Parsers.SOTG;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class StarshipsOfTheGalaxyManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly IBaseProcessor<StarshipDeployment> _starshipDeploymentProcessor;
        private readonly IBaseProcessor<StarshipEquipment> _starshipEquipmentProcessor;
        private readonly IBaseProcessor<StarshipModification> _starshipModificationProcessor;
        private readonly IBaseProcessor<StarshipBaseSize> _starshipSizeProcessor;
        private readonly IBaseProcessor<StarshipVenture> _starshipVentureProcessor;
        private readonly IBaseProcessor<StarshipChapterRules> _starshipChapterRulesProcessor;
        private readonly List<string> _ecSpeciesFileNames = new List<string>
        {
            "sotg_00.txt", "sotg_01.txt", "sotg_02.txt", "sotg_03.txt", "sotg_04.txt", "sotg_05.txt", "sotg_06.txt",
            "sotg_07.txt", "sotg_08.txt", "sotg_09.txt", "sotg_aa.txt"
        };

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
            var rules = await _starshipChapterRulesProcessor.Process(_ecSpeciesFileNames);
            await _tableStorage.AddBatchAsync<StarshipChapterRules>("starshipRules", rules,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var deployments = await _starshipDeploymentProcessor.Process(_ecSpeciesFileNames);
            await _tableStorage.AddBatchAsync<StarshipDeployment>("starshipDeployments", deployments,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var equipment = await _starshipEquipmentProcessor.Process(_ecSpeciesFileNames);
            await _tableStorage.AddBatchAsync<StarshipEquipment>("starshipEquipment", equipment,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var modifications = await _starshipModificationProcessor.Process(_ecSpeciesFileNames);
            await _tableStorage.AddBatchAsync<StarshipModification>("starshipModifications", modifications,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var sizes = await _starshipSizeProcessor.Process(_ecSpeciesFileNames);
            await _tableStorage.AddBatchAsync<StarshipBaseSize>("starshipBaseSizes", sizes,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var ventures = await _starshipVentureProcessor.Process(_ecSpeciesFileNames);
            await _tableStorage.AddBatchAsync<StarshipVenture>("starshipVentures", ventures,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
        }
    }
}
