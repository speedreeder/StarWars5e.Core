using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models.Background;
using StarWars5e.Parser.Parsers;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentBackgroundsManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly ExpandedContentBackgroundProcessor _backgroundProcessor;
        private readonly List<string> _ecBackgroundsFileName = new List<string>{"ec_backgrounds.txt"};

        public ExpandedContentBackgroundsManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
            _backgroundProcessor = new ExpandedContentBackgroundProcessor();
        }

        public async Task Parse()
        {
            var backgrounds = await _backgroundProcessor.Process(_ecBackgroundsFileName);
            await _tableStorage.AddBatchAsync<Background>("backgrounds", backgrounds,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
        }
    }
}
