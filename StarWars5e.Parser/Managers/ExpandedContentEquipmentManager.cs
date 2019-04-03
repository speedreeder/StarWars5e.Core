using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models.Equipment;
using StarWars5e.Parser.Parsers;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentEquipmentManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly ExpandedContentEquipmentProcessor _equipmentProcessor;
        private readonly List<string> _ecEquipmentFileName = new List<string> { "EC_Equipment.md" };

        public ExpandedContentEquipmentManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
            _equipmentProcessor = new ExpandedContentEquipmentProcessor();
        }

        public async Task Parse()
        {
            var equipment = await _equipmentProcessor.Process(_ecEquipmentFileName);
            await _tableStorage.AddBatchAsync<Equipment>("equipment", equipment,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
        }
    }
}
