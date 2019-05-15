using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Equipment;
using StarWars5e.Parser.Parsers;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentEquipmentManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly ExpandedContentEquipmentProcessor _equipmentProcessor;
        private readonly WeaponPropertyProcessor _weaponPropertyProcessor;

        private readonly List<string> _ecEquipmentFileName = new List<string> { "ec_equipment.txt" };

        public ExpandedContentEquipmentManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
            _equipmentProcessor = new ExpandedContentEquipmentProcessor();

            var nameStartingLineProperties = new Dictionary<string, string>
            {
                {"Auto", "#### Auto"},
                {"Disintegrate", "#### Disintegrate" }
            };

            _weaponPropertyProcessor = new WeaponPropertyProcessor(ContentType.ExpandedContent, nameStartingLineProperties);
        }

        public async Task Parse()
        {
            var equipment = await _equipmentProcessor.Process(_ecEquipmentFileName);
            await _tableStorage.AddBatchAsync<Equipment>("equipment", equipment,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var weaponProperties = await _weaponPropertyProcessor.Process(_ecEquipmentFileName);
            await _tableStorage.AddBatchAsync<WeaponProperty>("weaponProperties", weaponProperties,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
        }
    }
}
