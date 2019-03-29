using System.Threading.Tasks;
using StarWars5e.Models.Monster;
using StarWars5e.Parser.Parsers;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class MonsterManualManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly IBaseProcessor<Monster> _monsterProcessor;
        private const string MonsterManualFileName = "MM";

        public MonsterManualManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
            _monsterProcessor = new MonsterProcessor();
        }

        public async Task Parse()
        {
            var monsters = await _monsterProcessor.Process(MonsterManualFileName);
            await _tableStorage.AddBatchAsync<Monster>("monsters", monsters,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
        }
    }
}
