using System;
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
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;

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
            _globalSearchTermRepository = globalSearchTermRepository;

            var nameStartingLineProperties = new Dictionary<string, string>
            {
                {"Ammunition", "#### Ammunition"},
                {"Burst", "#### Burst" },
                {"Double", "#### Double" },
                {"Finesse", "#### Finesse" },
                {"Fixed", "#### Fixed" },
                {"Heavy", "#### Heavy" },
                {"Hidden", "#### Hidden" },
                {"Light", "#### Light" },
                {"Luminous", "#### Luminous" },
                {"Range", "#### Range" },
                {"Reach", "#### Reach" },
                {"Reload", "#### Reload" },
                {"Returning", "#### Returning" },
                {"Special", "#### Special" },
                {"Strength", "#### Strength" },
                {"Thrown", "#### Thrown" },
                {"Two-Handed", "#### Two-Handed" },
                {"Versatile", "#### Versatile" }
            };

            _weaponPropertyProcessor = new WeaponPropertyProcessor(ContentType.Core, nameStartingLineProperties);

            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            _cloudBlobContainer = cloudBlobClient.GetContainerReference("player-handbook-rules");
        }

        public async Task Parse()
        {
            try
            {
                var equipment =
                    await _playerHandbookEquipmentProcessor.Process(_phbFilesNames
                        .Where(p => p.Equals("PHB.phb_05.txt")).ToList());

                foreach (var equipment1 in equipment)
                {
                    equipment1.ContentSourceEnum = ContentSource.PHB;

                    switch (equipment1.EquipmentCategoryEnum)
                    {
                        case EquipmentCategory.Unknown:
                        case EquipmentCategory.Ammunition:
                        case EquipmentCategory.Explosive:
                        case EquipmentCategory.Storage:
                        case EquipmentCategory.AdventurePack:
                        case EquipmentCategory.Communications:
                        case EquipmentCategory.DataRecordingAndStorage:
                        case EquipmentCategory.LifeSupport:
                        case EquipmentCategory.Medical:
                        case EquipmentCategory.WeaponOrArmorAccessory:
                        case EquipmentCategory.Tool:
                        case EquipmentCategory.Mount:
                        case EquipmentCategory.Vehicle:
                        case EquipmentCategory.TradeGood:
                        case EquipmentCategory.Utility:
                        case EquipmentCategory.GamingSet:
                        case EquipmentCategory.MusicalInstrument:
                        case EquipmentCategory.Droid:
                        case EquipmentCategory.Clothing:
                        case EquipmentCategory.Kit:
                            var equipmentSearchTerm = _globalSearchTermRepository.CreateSearchTerm(equipment1.Name,
                                GlobalSearchTermType.AdventuringGear, ContentType.Core,
                                $"/loot/adventuringGear/?search={equipment1.Name}");
                            _globalSearchTermRepository.SearchTerms.Add(equipmentSearchTerm);
                            break;
                        case EquipmentCategory.Weapon:
                            var weaponSearchTerm = _globalSearchTermRepository.CreateSearchTerm(equipment1.Name,
                                GlobalSearchTermType.Weapon, ContentType.Core,
                                $"/loot/weapons/?search={equipment1.Name}");
                            _globalSearchTermRepository.SearchTerms.Add(weaponSearchTerm);
                            break;
                        case EquipmentCategory.Armor:
                            var searchTermType = GlobalSearchTermType.Armor;
                            if (equipment1.ArmorClassificationEnum == ArmorClassification.Shield)
                            {
                                searchTermType = GlobalSearchTermType.Shield;
                            }

                            var armorSearchTerm = _globalSearchTermRepository.CreateSearchTerm(equipment1.Name,
                                searchTermType, ContentType.Core,
                                $"/loot/armor/?search={equipment1.Name}");
                            _globalSearchTermRepository.SearchTerms.Add(armorSearchTerm);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                await _tableStorage.AddBatchAsync<Equipment>("equipment", equipment,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB equipment.");
            }

            try
            {
                var backgrounds =
                    await _playerHandbookBackgroundsProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_04.txt"))
                        .ToList());

                foreach (var background in backgrounds)
                {
                    background.ContentSourceEnum = ContentSource.PHB;

                    var backgroundSearchTerm = _globalSearchTermRepository.CreateSearchTerm(background.Name, GlobalSearchTermType.Background, ContentType.Core,
                        $"/characters/backgrounds/{background.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(backgroundSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Background>("backgrounds", backgrounds,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB backgrounds.");
            }

            try
            {
                var species =
                    await _playerHandbookSpeciesProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_02.txt"))
                        .ToList());

                foreach (var specie in species)
                {
                    specie.ContentSourceEnum = ContentSource.PHB;

                    var specieSearchTerm = _globalSearchTermRepository.CreateSearchTerm(specie.Name, GlobalSearchTermType.Species, ContentType.Core,
                        $"/characters/species/{specie.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(specieSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Species>("species", species,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB species.");
            }

            try
            {
                var classes =
                    await _playerHandbookClassProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_03.txt"))
                        .ToList());

                foreach (var swClass in classes)
                {
                    swClass.ContentSourceEnum = ContentSource.PHB;

                    var classSearchTerm = _globalSearchTermRepository.CreateSearchTerm(swClass.Name, GlobalSearchTermType.Class, ContentType.Core,
                        $"/characters/classes/{swClass.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(classSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Class>("classes", classes,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
                try
                {
                    var archetypes = classes.SelectMany(s => s.Archetypes).ToList();

                    foreach (var archetype in archetypes)
                    {
                        archetype.ContentSourceEnum = ContentSource.PHB;

                        var archetypeSearchTerm = _globalSearchTermRepository.CreateSearchTerm(archetype.Name, GlobalSearchTermType.Archetype, ContentType.Core,
                            $"/characters/archetypes/{archetype.Name}");
                        _globalSearchTermRepository.SearchTerms.Add(archetypeSearchTerm);
                    }

                    await _tableStorage.AddBatchAsync<Archetype>("archetypes", archetypes,
                        new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
                }
                catch (StorageException)
                {
                    Console.WriteLine("Failed to upload PHB archetypes.");
                }
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB classes.");
            }

            try
            {
                var powers =
                    await _playerHandbookPowersProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_11.txt") || p.Equals("PHB.phb_12.txt"))
                        .ToList());
                
                foreach (var power in powers)
                {
                    power.ContentSourceEnum = ContentSource.PHB;

                    switch (power.PowerTypeEnum)
                    {
                        case PowerType.None:
                            break;
                        case PowerType.Force:
                            var forcePowerSearchTerm = _globalSearchTermRepository.CreateSearchTerm(power.Name, GlobalSearchTermType.ForcePower, ContentType.Core,
                                $"/characters/forcePowers/?search={power.Name}");
                            _globalSearchTermRepository.SearchTerms.Add(forcePowerSearchTerm);
                            break;
                        case PowerType.Tech:
                            var techPowerSearchTerm = _globalSearchTermRepository.CreateSearchTerm(power.Name, GlobalSearchTermType.TechPower, ContentType.Core,
                                $"/characters/techPowers/?search={power.Name}");
                            _globalSearchTermRepository.SearchTerms.Add(techPowerSearchTerm);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                await _tableStorage.AddBatchAsync<Power>("powers", powers,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB powers.");
            }

            try
            {
                var feats = await _playerHandbookFeatProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_06.txt"))
                    .ToList());

                foreach (var feat in feats)
                {
                    feat.ContentSourceEnum = ContentSource.PHB;

                    var featSearchTerm = _globalSearchTermRepository.CreateSearchTerm(feat.Name, GlobalSearchTermType.Feat, ContentType.Core,
                        $"/characters/feats/?search={feat.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(featSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Feat>("feats", feats,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB feats.");
            }

            try
            {
                var rules =
                    await _playerHandbookChapterRulesProcessor.Process(_phbFilesNames);

                await _cloudBlobContainer.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Off, null, null);
                foreach (var chapterRules in rules)
                {
                    var json = JsonConvert.SerializeObject(chapterRules);
                    var blob = _cloudBlobContainer.GetBlockBlobReference($"{chapterRules.ChapterName}.json");

                    await blob.UploadTextAsync(json);
                }
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB rules.");
            }

            try
            {
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
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB weapon properties.");
            }

            foreach (var referenceName in SectionNames.ReferenceNames)
            {
                var referenceSearchTerm = _globalSearchTermRepository.CreateSearchTerm(referenceName.name, referenceName.globalSearchTermType, ContentType.Core,
                    referenceName.pathOverride);
                _globalSearchTermRepository.SearchTerms.Add(referenceSearchTerm);
            }

            foreach (var variantRuleName in SectionNames.VariantRuleNames)
            {
                var variantRuleSearchTerm = _globalSearchTermRepository.CreateSearchTerm(variantRuleName.name, variantRuleName.globalSearchTermType, ContentType.Core,
                    variantRuleName.pathOverride);
                _globalSearchTermRepository.SearchTerms.Add(variantRuleSearchTerm);
            }
        }
    }
}
