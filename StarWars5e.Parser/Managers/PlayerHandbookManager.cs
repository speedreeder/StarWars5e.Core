using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using StarWars5e.Models;
using StarWars5e.Models.Background;
using StarWars5e.Models.Class;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Equipment;
using StarWars5e.Models.Search;
using StarWars5e.Models.Species;
using StarWars5e.Parser.Parsers;
using StarWars5e.Parser.Parsers.PHB;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class PlayerHandbookManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly CloudBlobContainer _cloudBlobContainer;
        private readonly PlayerHandbookEquipmentProcessor _playerHandbookEquipmentProcessor;
        private readonly PlayerHandbookBackgroundsProcessor _playerHandbookBackgroundsProcessor;
        private readonly PlayerHandbookSpeciesProcessor _playerHandbookSpeciesProcessor;
        private readonly PlayerHandbookClassProcessor _playerHandbookClassProcessor;
        private readonly PlayerHandbookPowersProcessor _playerHandbookPowersProcessor;
        private readonly PlayerHandbookChapterRulesProcessor _playerHandbookChapterRulesProcessor;
        private readonly PlayerHandbookFeatProcessor _playerHandbookFeatProcessor;
        private readonly WeaponPropertyProcessor _weaponPropertyProcessor;


        private readonly List<string> _phbFilesNames = new List<string>
        {
            "PHB.phb_-1.txt", "PHB.phb_00.txt", "PHB.phb_01.txt", "PHB.phb_02.txt", "PHB.phb_03.txt", "PHB.phb_04.txt",
            "PHB.phb_05.txt", "PHB.phb_06.txt", "PHB.phb_07.txt", "PHB.phb_08.txt", "PHB.phb_09.txt", "PHB.phb_10.txt",
            "PHB.phb_11.txt", "PHB.phb_12.txt", "PHB.phb_aa.txt", "PHB.phb_ab.txt", "PHB.phb_changelog.txt"
        };

        public PlayerHandbookManager(ITableStorage tableStorage, CloudStorageAccount cloudStorageAccount, GlobalSearchTermRepository globalSearchTermRepository)
        {
            _tableStorage = tableStorage;
            _playerHandbookEquipmentProcessor = new PlayerHandbookEquipmentProcessor();
            _playerHandbookBackgroundsProcessor = new PlayerHandbookBackgroundsProcessor();
            _playerHandbookSpeciesProcessor = new PlayerHandbookSpeciesProcessor();
            _playerHandbookClassProcessor = new PlayerHandbookClassProcessor();
            _playerHandbookPowersProcessor = new PlayerHandbookPowersProcessor();
            _playerHandbookChapterRulesProcessor = new PlayerHandbookChapterRulesProcessor(globalSearchTermRepository);
            _playerHandbookFeatProcessor = new PlayerHandbookFeatProcessor();

            var nameStartingLineProperties = new Dictionary<string, string>
            {
                {"Ammunition", "#### Ammunition"},
                {"Burst", "#### Burst" },
                {"Double", "#### Double" },
                {"Finesse", "#### Finesse" },
                {"Heavy", "#### Heavy" },
                {"Hidden", "#### Hidden" },
                {"Light", "#### Burst" },
                {"Luminous", "#### Luminous" },
                {"Range", "#### Range" },
                {"Reach", "#### Reach" },
                {"Reload", "#### Reload" },
                {"Special", "#### Special" },
                {"Strength", "#### Strength" },
                {"Thrown", "#### Thrown" },
                {"Two-Handed", "#### Two-Handed" },
                {"Versatile", "#### Versatile" },
            };

            _weaponPropertyProcessor = new WeaponPropertyProcessor(ContentType.Core, nameStartingLineProperties);

            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            _cloudBlobContainer = cloudBlobClient.GetContainerReference("player-handbook-rules");
        }

        public async Task Parse()
        {
            var equipment =
                await _playerHandbookEquipmentProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_05.txt"))
                    .ToList());
            await _tableStorage.AddBatchAsync<Equipment>("equipment", equipment,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var backgrounds =
                await _playerHandbookBackgroundsProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_04.txt"))
                    .ToList());
            await _tableStorage.AddBatchAsync<Background>("backgrounds", backgrounds,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var species =
                await _playerHandbookSpeciesProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_02.txt"))
                    .ToList());
            await _tableStorage.AddBatchAsync<Species>("species", species,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var classes =
                await _playerHandbookClassProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_03.txt"))
                    .ToList());

            var archetypes = classes.SelectMany(s => s.Archetypes);

            await _tableStorage.AddBatchAsync<Archetype>("archetypes", archetypes,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            await _tableStorage.AddBatchAsync<Class>("classes", classes,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var powers =
                await _playerHandbookPowersProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_11.txt") || p.Equals("PHB.phb_12.txt"))
                    .ToList());

            await _tableStorage.AddBatchAsync<Power>("powers", powers,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var feats = await _playerHandbookFeatProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_06.txt"))
                .ToList());

            await _tableStorage.AddBatchAsync<Feat>("feats", feats,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var rules =
                await _playerHandbookChapterRulesProcessor.Process(_phbFilesNames);

            await _cloudBlobContainer.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Off, null, null);
            foreach (var chapterRules in rules)
            {
                var json = JsonConvert.SerializeObject(chapterRules);
                var blob = _cloudBlobContainer.GetBlockBlobReference($"{chapterRules.ChapterName}.json");

                await blob.UploadTextAsync(json);
            }

            var weaponProperties =
                await _weaponPropertyProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_05.txt")).ToList());

            var specialProperty = weaponProperties.SingleOrDefault(w => w.Name == "Special");
            if (specialProperty != null)
            {
                specialProperty.Content =
                    "#### Special\r\nA weapon with the special property has unusual rules governing its use, explained in the weapon's description.";
            }

            await _tableStorage.AddBatchAsync<WeaponProperty>("weaponProperties", weaponProperties,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
        }
    }
}
