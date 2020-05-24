using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Monster;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class MonsterManualManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly IBaseProcessor<Monster> _monsterProcessor;
        private readonly List<string> _mmFileName = new List<string> { "SNV_Content.txt" };
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly ILocalization _localization;

        public MonsterManualManager(ITableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository, ILocalization localization)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _localization = localization;
            _monsterProcessor = new MonsterProcessor();
        }

        public async Task Parse()
        {
            try
            {
                var monsters = await _monsterProcessor.Process(_mmFileName, _localization);

                foreach (var monster in monsters)
                {
                    monster.ContentSourceEnum = ContentSource.SnV;

                    var monsterSearchTerm = _globalSearchTermRepository.CreateSearchTerm(monster.Name,
                        GlobalSearchTermType.Monster, ContentType.Core, $"/rules/snv/monsters/{monster.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(monsterSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Monster>($"monsters{_localization.Language}", monsters,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload monsters.");
            }

            foreach (var monsterChapterName in SectionNames.MonsterChapterNames)
            {
                var monsterChapterSearchTerm = _globalSearchTermRepository.CreateSearchTerm(monsterChapterName.name,
                    monsterChapterName.globalSearchTermType, ContentType.Core, monsterChapterName.pathOverride);
                _globalSearchTermRepository.SearchTerms.Add(monsterChapterSearchTerm);
            }
            
        }
    }
}
