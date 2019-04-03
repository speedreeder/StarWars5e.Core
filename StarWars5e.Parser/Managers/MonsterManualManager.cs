using System.Collections.Generic;
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
        private readonly List<string> _mmFileName = new List<string> { "MM.md" };

        public MonsterManualManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
            _monsterProcessor = new MonsterProcessor();
        }

        public async Task Parse()
        {
            var monsters = await _monsterProcessor.Process(_mmFileName);
            await _tableStorage.AddBatchAsync<Monster>("monsters", monsters,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
        }
    }
}
