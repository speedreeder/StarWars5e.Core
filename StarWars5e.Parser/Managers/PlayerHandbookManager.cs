﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models.Equipment;
using StarWars5e.Parser.Parsers.PHB;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class PlayerHandbookManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly PlayerHandbookEquipmentProcessor _playerHandbookEquipmentProcessor;
        private readonly List<string> _phbFilesNames = new List<string>
        {
            "PHB.phb_01.txt", "PHB.phb_02.txt", "PHB.phb_03.txt", "PHB.phb_04.txt", "PHB.phb_05.txt", "PHB.phb_06.txt", "PHB.phb_07.txt",
            "PHB.phb_08.txt", "PHB.phb_09.txt", "PHB.phb_10.txt", "PHB.phb_11.txt", "PHB.phb_12.txt", "PHB.phb_aa.txt", "PHB.phb_ab.txt",
        };

        public PlayerHandbookManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
            _playerHandbookEquipmentProcessor = new PlayerHandbookEquipmentProcessor();
        }

        public async Task Parse()
        {
            var equipment =
                await _playerHandbookEquipmentProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_05.txt"))
                    .ToList());
            await _tableStorage.AddBatchAsync<Equipment>("equipment", equipment,
                new BatchOperationOptions {BatchInsertMethod = BatchInsertMethod.InsertOrReplace});
        }
    }
}
