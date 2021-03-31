using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;
using StarWars5e.Parser.Storage;

namespace StarWars5e.Parser.Managers
{
    public class ReferenceTableManager
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly IBaseProcessor<ReferenceTable> _referenceTableProcessor;
        private readonly List<string> _referenceTableFileNames = new()
        {
            "PHB.phb_-1.txt", "PHB.phb_00.txt", "PHB.phb_01.txt", "PHB.phb_02.txt", "PHB.phb_03.txt", "PHB.phb_04.txt",
            "PHB.phb_05.txt", "PHB.phb_06.txt", "PHB.phb_07.txt", "PHB.phb_08.txt", "PHB.phb_09.txt", "PHB.phb_10.txt",
            "PHB.phb_11.txt", "PHB.phb_12.txt", "PHB.phb_aa.txt", "PHB.phb_ab.txt","SOTG.sotg_00.txt", "SOTG.sotg_01.txt",
            "SOTG.sotg_02.txt", "SOTG.sotg_03.txt", "SOTG.sotg_04.txt", "SOTG.sotg_05.txt", "SOTG.sotg_06.txt",
            "SOTG.sotg_07.txt", "SOTG.sotg_08.txt", "SOTG.sotg_09.txt", "SOTG.sotg_aa.txt", "ec_05.txt"
        };
        private readonly ILocalization _localization;

        public ReferenceTableManager(IAzureTableStorage tableStorage, ILocalization localization)
        {
            _tableStorage = tableStorage;
            _localization = localization;
            _referenceTableProcessor = new ReferenceTableProcessor();
        }

        public async Task<List<ReferenceTable>> Parse()
        {
            var tables = await _referenceTableProcessor.Process(_referenceTableFileNames, _localization);

            var dupes = tables
                .GroupBy(i => i.RowKey)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);

            await _tableStorage.AddBatchAsync<ReferenceTable>($"referenceTables{_localization.Language}", tables,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            return tables;
        }
    }
}
