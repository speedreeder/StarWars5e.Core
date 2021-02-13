using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Monster;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;
using StarWars5e.Parser.Storage;

namespace StarWars5e.Parser.Managers
{
    public class MonsterManualManager
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly IBaseProcessor<Monster> _monsterProcessor;
        private readonly List<string> _mmFileName = new List<string> { "SNV_Content.txt" };
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly ILocalization _localization;

        public List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> MonsterChapterNames;

        public MonsterManualManager(IAzureTableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository, ILocalization localization)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _localization = localization;
            _monsterProcessor = new MonsterProcessor();

            MonsterChapterNames = new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( localization.MonsterManual, GlobalSearchTermType.Book, "/rules/snv")
            };
        }

        public async Task Parse(List<Power> powers)
        {
            try
            {
                var monsters = await _monsterProcessor.Process(_mmFileName, _localization);
                var powerNames = powers.Select(p => p.Name).OrderBy(p => p);

                foreach (var monster in monsters)
                {
                    monster.ContentSourceEnum = ContentSource.SnV;

                    var monsterSearchTerm = _globalSearchTermRepository.CreateSearchTerm(monster.Name,
                        GlobalSearchTermType.Monster, ContentType.Core, $"/rules/snv/monsters/{monster.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(monsterSearchTerm);

                    //foreach (var behavior in monster.Behaviors.Where(b =>
                    //    b.Name.Contains("casting", StringComparison.OrdinalIgnoreCase)))
                    //{
                    //    foreach (var power in powers.Where(p => behavior.DescriptionWithLinks.Contains(p.Name, StringComparison.InvariantCultureIgnoreCase)))
                    //    {
                    //        behavior.DescriptionWithLinks = Regex.Replace(behavior.DescriptionWithLinks,
                    //            $@"([ :*,]+)({power.Name})([\s:*,]*)", $"$1[{power.Name}](#{Uri.EscapeUriString(power.Name)})$3",
                    //            RegexOptions.IgnoreCase);
                    //    }
                    //}
                }

                await _tableStorage.AddBatchAsync<Monster>($"monsters{_localization.Language}", monsters,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload monsters.");
            }

            foreach (var monsterChapterName in MonsterChapterNames)
            {
                var monsterChapterSearchTerm = _globalSearchTermRepository.CreateSearchTerm(monsterChapterName.name,
                    monsterChapterName.globalSearchTermType, ContentType.Core, monsterChapterName.pathOverride);
                _globalSearchTermRepository.SearchTerms.Add(monsterChapterSearchTerm);
            }
            
        }
    }
}
