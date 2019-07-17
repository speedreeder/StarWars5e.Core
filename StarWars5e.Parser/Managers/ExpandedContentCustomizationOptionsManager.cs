using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StarWars5e.Models.Class;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentCustomizationOptionsManager
    {
        private readonly ITableStorage _tableStorage;
        //private readonly ExpandedContentArchetypeProcessor _archetypeProcessor;
        private readonly List<string> _ecArchetypesFileName = new List<string> { "ec_archetypes.txt" };

        public ExpandedContentCustomizationOptionsManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
            //_archetypeProcessor = new ExpandedContentArchetypeProcessor();
        }

        public async Task Parse()
        {
            //var archetypes = await _archetypeProcessor.Process(_ecArchetypesFileName);
            //await _tableStorage.AddBatchAsync<Archetype>("archetypes", archetypes,
            //    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
        }
    }
}
