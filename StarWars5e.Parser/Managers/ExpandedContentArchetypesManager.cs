using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models.Class;
using StarWars5e.Parser.Parsers;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentArchetypesManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly ExpandedContentArchetypeProcessor _archetypeProcessor;
        private readonly List<string> _ecArchetypesFileName = new List<string> { "ec_archetypes.txt" };

        public ExpandedContentArchetypesManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
            _archetypeProcessor = new ExpandedContentArchetypeProcessor();
        }

        public async Task Parse()
        {
            var archetypes = await _archetypeProcessor.Process(_ecArchetypesFileName);
            await _tableStorage.AddBatchAsync<Archetype>("archetypes", archetypes,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
        }
    }
}
