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
        private const string EcEquipmentFileName = "EC_Equipment";

        public ExpandedContentEquipmentManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
            _equipmentProcessor = new ExpandedContentEquipmentProcessor();
        }

        public async Task Parse()
        {
            var equipment = await _equipmentProcessor.Process(EcEquipmentFileName);
            await _tableStorage.AddBatchAsync<Equipment>("equipment", equipment,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
        }
    }
}
