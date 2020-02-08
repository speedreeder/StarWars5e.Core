using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
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
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly ExpandedContentEquipmentProcessor _equipmentProcessor;
        private readonly WeaponPropertyProcessor _weaponPropertyProcessor;

        private readonly List<string> _ecEquipmentFileName = new List<string> { "ec_equipment.txt" };

        public ExpandedContentEquipmentManager(ITableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _equipmentProcessor = new ExpandedContentEquipmentProcessor();

            var nameStartingLineProperties = new List<(string name, string startLine, int occurence)>
            {
                ("Auto", "#### Auto", 1),
                ("Defensive", "#### Defensive", 1),
                ("Disguised", "#### Disguised", 1),
                ("Disintegrate", "#### Disintegrate", 1),
                ("Shocking", "#### Shocking", 1)
            };

            _weaponPropertyProcessor = new WeaponPropertyProcessor(ContentType.ExpandedContent, nameStartingLineProperties);
        }

        public async Task Parse()
        {
            try
            {
                var equipments = await _equipmentProcessor.Process(_ecEquipmentFileName);

                foreach (var equipment in equipments)
                {
                    equipment.ContentSourceEnum = ContentSource.EC;

                    switch (equipment.EquipmentCategoryEnum)
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
                            var equipmentSearchTerm = _globalSearchTermRepository.CreateSearchTerm(equipment.Name,
                                GlobalSearchTermType.AdventuringGear, ContentType.ExpandedContent,
                                $"/loot/adventuringGear/?search={equipment.Name}");
                            _globalSearchTermRepository.SearchTerms.Add(equipmentSearchTerm);
                            break;
                        case EquipmentCategory.Weapon:
                            var weaponSearchTerm = _globalSearchTermRepository.CreateSearchTerm(equipment.Name,
                                GlobalSearchTermType.Weapon, ContentType.ExpandedContent,
                                $"/loot/weapons/?search={equipment.Name}");
                            _globalSearchTermRepository.SearchTerms.Add(weaponSearchTerm);
                            break;
                        case EquipmentCategory.Armor:
                            var searchTermType = GlobalSearchTermType.Armor;
                            if (equipment.ArmorClassificationEnum == ArmorClassification.Shield)
                            {
                                searchTermType = GlobalSearchTermType.Shield;
                            }

                            var armorSearchTerm = _globalSearchTermRepository.CreateSearchTerm(equipment.Name,
                                searchTermType, ContentType.ExpandedContent,
                                $"/loot/armor/?search={equipment.Name}");
                            _globalSearchTermRepository.SearchTerms.Add(armorSearchTerm);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                await _tableStorage.AddBatchAsync<Equipment>("equipment", equipments,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload EC equipment.");
            }

            try
            {
                var weaponProperties = await _weaponPropertyProcessor.Process(_ecEquipmentFileName);

                await _tableStorage.AddBatchAsync<WeaponProperty>("weaponProperties", weaponProperties,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload EC weapon properties.");
            }
        }
    }
}
