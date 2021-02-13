using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Equipment;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;
using StarWars5e.Parser.Storage;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentEquipmentManager
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;

        private readonly List<string> _ecEquipmentFileName = new List<string> { "ec_05.txt" };
        private readonly ILocalization _localization;

        public ExpandedContentEquipmentManager(IAzureTableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository, ILocalization localization)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _localization = localization;
        }

        public async Task Parse()
        {
            try
            {
                var equipmentProcessor = new ExpandedContentEquipmentProcessor(_localization);
                var equipments = await equipmentProcessor.Process(_ecEquipmentFileName, _localization);

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

                await _tableStorage.AddBatchAsync<Equipment>($"equipment{_localization.Language}", equipments,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload EC equipment.");
            }
        }
    }
}
