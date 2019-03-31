using System.Threading.Tasks;
using StarWars5e.Models.Background;
using StarWars5e.Parser.Parsers;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentBackgroundsManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly BackgroundProcessor _backgroundProcessor;
        private const string EcBackgroundsFileName = "EC_Backgrounds";

        public ExpandedContentBackgroundsManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
            _backgroundProcessor = new BackgroundProcessor();
        }

        public async Task Parse()
        {
            var backgrounds = await _backgroundProcessor.Process(EcBackgroundsFileName);
            await _tableStorage.AddBatchAsync<Background>("backgrounds", backgrounds,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
        }
    }
}
