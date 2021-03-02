using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StarWars5e.Models;
using StarWars5e.Models.Background;
using StarWars5e.Models.Class;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Equipment;
using StarWars5e.Models.Lookup;
using StarWars5e.Models.Species;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;
using StarWars5e.Parser.Processors.PHB;
using StarWars5e.Parser.Storage;

namespace StarWars5e.Parser.Managers
{
    public class PlayerHandbookManager
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly PlayerHandbookEquipmentProcessor _playerHandbookEquipmentProcessor;
        private readonly PlayerHandbookBackgroundsProcessor _playerHandbookBackgroundsProcessor;
        private readonly PlayerHandbookChapterRulesProcessor _playerHandbookChapterRulesProcessor;
        private readonly PlayerHandbookFeatProcessor _playerHandbookFeatProcessor;
        private readonly WeaponPropertyProcessor _weaponPropertyProcessor;
        private readonly ArmorPropertyProcessor _armorPropertyProcessor;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly ILocalization _localization;
        private readonly BlobContainerClient _blobContainerClient;
        private readonly FeatureRepository _featureRepository;

        private readonly List<string> _phbFilesNames = new()
        {
            "PHB.phb_-1.txt", "PHB.phb_00.txt", "PHB.phb_01.txt", "PHB.phb_02.txt", "PHB.phb_03.txt", "PHB.phb_04.txt",
            "PHB.phb_05.txt", "PHB.phb_06.txt", "PHB.phb_07.txt", "PHB.phb_08.txt", "PHB.phb_09.txt", "PHB.phb_10.txt",
            "PHB.phb_11.txt", "PHB.phb_12.txt", "PHB.phb_aa.txt", "PHB.phb_ab.txt", "PHB.phb_changelog.txt"
        };

        public List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> ReferenceNames;

        public PlayerHandbookManager(IServiceProvider serviceProvider,
            ILocalization localization)
        {
            _tableStorage = serviceProvider.GetService<IAzureTableStorage>();
            _globalSearchTermRepository = serviceProvider.GetService<GlobalSearchTermRepository>();

            _playerHandbookEquipmentProcessor = new PlayerHandbookEquipmentProcessor();
            _playerHandbookBackgroundsProcessor = new PlayerHandbookBackgroundsProcessor();
            _playerHandbookChapterRulesProcessor = new PlayerHandbookChapterRulesProcessor(_globalSearchTermRepository);
            _playerHandbookFeatProcessor = new PlayerHandbookFeatProcessor(localization);
            _localization = localization;

            var blobServiceClient = serviceProvider.GetService<BlobServiceClient>();
            _blobContainerClient = blobServiceClient.GetBlobContainerClient($"player-handbook-rules-{_localization.Language}");

            _weaponPropertyProcessor = new WeaponPropertyProcessor(ContentType.Core, _localization.PlayerHandbookWeaponProperties);

            _armorPropertyProcessor = new ArmorPropertyProcessor(ContentType.Core, _localization.PlayerHandbookArmorProperties);

            ReferenceNames = new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( localization.Monsters, GlobalSearchTermType.Reference, "/rules/snv/monsters"),
                ( localization.Classes, GlobalSearchTermType.Reference, "/characters/classes"),
                ( localization.Species, GlobalSearchTermType.Reference, "/characters/species"),
                ( localization.Archetypes, GlobalSearchTermType.Reference, "/characters/archetypes"),
                ( localization.Backgrounds, GlobalSearchTermType.Reference, "/characters/backgrounds"),
                ( localization.Armor, GlobalSearchTermType.Reference, "/loot/armor"),
                ( localization.Weapons, GlobalSearchTermType.Reference, "/loot/weapons"),
                ( localization.AdventuringGear, GlobalSearchTermType.Reference, "/loot/adventuringGear"),
                ( localization.EnhancedItems, GlobalSearchTermType.Reference, "/loot/enhancedItems"),
                ( localization.Feats, GlobalSearchTermType.Reference, "/characters/feats"),
                ( localization.ForcePowers, GlobalSearchTermType.Reference, "/characters/forcePowers"),
                ( localization.TechPowers, GlobalSearchTermType.Reference, "/characters/techPowers"),
                ( localization.StarshipModifications, GlobalSearchTermType.Reference, "/starships/modifications"),
                ( localization.StarshipEquipment, GlobalSearchTermType.Reference, "/starships/equipment"),
                ( localization.StarshipWeapons, GlobalSearchTermType.Reference, "/starships/weapons"),
                ( localization.Ventures, GlobalSearchTermType.Reference, "/starships/ventures"),
                ( localization.AdditionalVariantRules, GlobalSearchTermType.Reference, "/rules/variantRules"),
            };

            _featureRepository = serviceProvider.GetService<FeatureRepository>();
        }

        public async Task<List<Power>> Parse()
        {
            try
            {
                var equipment =
                    await _playerHandbookEquipmentProcessor.Process(_phbFilesNames
                        .Where(p => p.Equals("PHB.phb_05.txt")).ToList(), _localization);

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

                await _tableStorage.AddBatchAsync<Equipment>($"equipment{_localization.Language}", equipment,
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
                        .ToList(), _localization);

                foreach (var background in backgrounds)
                {
                    background.ContentSourceEnum = ContentSource.PHB;

                    var backgroundSearchTerm = _globalSearchTermRepository.CreateSearchTerm(background.Name, GlobalSearchTermType.Background, ContentType.Core,
                        $"/characters/backgrounds/{background.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(backgroundSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Background>($"backgrounds{_localization.Language}", backgrounds,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB backgrounds.");
            }

            try
            {
                var playerHandbookFeatureOptionsProcessor = new PlayerHandbookFeatureOptionsProcessor();

                var featureOptions = await playerHandbookFeatureOptionsProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_03.txt"))
                        .ToList(), _localization);

                await _tableStorage.AddBatchAsync<FeatureOption>($"featureOptions{_localization.Language}", featureOptions,
                 new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            }
            catch (StorageException se)
            {
                Console.WriteLine($"Failed to upload feature options. {se}");
            }

            var speciesImageUrlsLus = await _tableStorage.GetAllAsync<SpeciesImageUrlLU>("speciesImageUrlsLU");
            var playerHandbookSpeciesProcessor = new PlayerHandbookSpeciesProcessor(speciesImageUrlsLus.ToList());

            var species =
                await playerHandbookSpeciesProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_02.txt"))
                    .ToList(), _localization);
            try
            {
                foreach (var specie in species)
                {
                    specie.ContentSourceEnum = ContentSource.PHB;

                    var specieSearchTerm = _globalSearchTermRepository.CreateSearchTerm(specie.Name, GlobalSearchTermType.Species, ContentType.Core,
                        $"/characters/species/{specie.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(specieSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Species>($"species{_localization.Language}", species,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB species.");
            }

            try
            {
                var specieFeatures = species.SelectMany(f => f.Features).ToList();

                await _tableStorage.AddBatchAsync<Feature>($"features{_localization.Language}", specieFeatures,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

                _featureRepository.Features.AddRange(specieFeatures);
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload EC species.");
            }

            try
            {
                var classImageLus = await _tableStorage.GetAllAsync<ClassImageLU>("classImageLU");
                var casterRatioLus = await _tableStorage.GetAllAsync<CasterRatioLU>("casterRatioLU");
                var multiclassProficiencyLus =
                    await _tableStorage.GetAllAsync<MulticlassProficiencyLU>("multiclassProficiencyLU");

                var playerHandbookClassProcessor = new PlayerHandbookClassProcessor(classImageLus.ToList(), casterRatioLus.ToList(), multiclassProficiencyLus.ToList());

                var classes =
                    await playerHandbookClassProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_03.txt"))
                        .ToList(), _localization);

                foreach (var swClass in classes)
                {
                    swClass.ContentSourceEnum = ContentSource.PHB;

                    var classSearchTerm = _globalSearchTermRepository.CreateSearchTerm(swClass.Name, GlobalSearchTermType.Class, ContentType.Core,
                        $"/characters/classes/{swClass.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(classSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Class>($"classes{_localization.Language}", classes,
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

                    try
                    {
                        var archetypeFeatures = archetypes.SelectMany(f => f.Features).ToList();

                        var dupes = archetypeFeatures
                            .GroupBy(i => i.RowKey)
                            .Where(g => g.Count() > 1)
                            .Select(g => g.Key);

                        await _tableStorage.AddBatchAsync<Feature>($"features{_localization.Language}", archetypeFeatures,
                            new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
                        _featureRepository.Features.AddRange(archetypeFeatures);
                    }
                    catch (StorageException se)
                    {
                        Console.WriteLine($"Failed to upload PHB archetype features: {se}");
                    }

                    await _tableStorage.AddBatchAsync<Archetype>($"archetypes{_localization.Language}", archetypes,
                        new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
                }
                catch (StorageException se)
                {
                    Console.WriteLine($"Failed to upload PHB archetypes: {se}");
                }

                try
                {
                    var classFeatures = classes.SelectMany(f => f.Features).ToList();

                    var dupes = classFeatures
                        .GroupBy(i => i.RowKey)
                        .Where(g => g.Count() > 1)
                        .Select(g => g.Key);

                    await _tableStorage.AddBatchAsync<Feature>($"features{_localization.Language}", classFeatures,
                        new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
                    _featureRepository.Features.AddRange(classFeatures);
                }
                catch (StorageException se)
                {
                    Console.WriteLine($"Failed to upload PHB class features: {se}");
                }
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB classes.");
            }

            var powers = new List<Power>();
            try
            {
                var playerHandbookPowersProcessor = new PlayerHandbookPowersProcessor(_localization);

                powers =
                    await playerHandbookPowersProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_11.txt") || p.Equals("PHB.phb_12.txt"))
                        .ToList(), _localization);
                
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

                await _tableStorage.AddBatchAsync<Power>($"powers{_localization.Language}", powers,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB powers.");
            }

            try
            {
                var feats = await _playerHandbookFeatProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_06.txt"))
                    .ToList(), _localization);

                foreach (var feat in feats)
                {
                    feat.ContentSourceEnum = ContentSource.PHB;

                    var featSearchTerm = _globalSearchTermRepository.CreateSearchTerm(feat.Name, GlobalSearchTermType.Feat, ContentType.Core,
                        $"/characters/feats/?search={feat.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(featSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Feat>($"feats{_localization.Language}", feats,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB feats.");
            }

            try
            {
                var lightsaberFormsProcessor = new PlayerHandbookCustomizationOptionsLightsaberFormsProcessor();
                var lightsaberForms = await lightsaberFormsProcessor.Process(new List<string> { "PHB.phb_06.txt" }, _localization, ContentType.Core);

                foreach (var lightsaberForm in lightsaberForms)
                {
                    lightsaberForm.ContentSourceEnum = ContentSource.PHB;

                    var lightsaberFormSearchTerm = _globalSearchTermRepository.CreateSearchTerm(lightsaberForm.Name, GlobalSearchTermType.LightsaberForm, ContentType.Core,
                        $"/characters/lightsaberForms/?search={lightsaberForm.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(lightsaberFormSearchTerm);
                }

                await _tableStorage.AddBatchAsync<LightsaberForm>($"lightsaberForms{_localization.Language}", lightsaberForms,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException se)
            {
                Console.WriteLine($"Failed to upload PHB lightsaber forms: {se}");
            }

            try
            {
                var fightingStylesProcessor = new ExpandedContentCustomizationOptionsFightingStyleProcessor();
                var fightingStyles = await fightingStylesProcessor.Process(new List<string> { "PHB.phb_06.txt" }, _localization, ContentType.Core);

                foreach (var fightingStyle in fightingStyles)
                {
                    fightingStyle.ContentSourceEnum = ContentSource.PHB;

                    var fightingStyleSearchTerm = _globalSearchTermRepository.CreateSearchTerm(fightingStyle.Name, GlobalSearchTermType.FightingStyle, ContentType.Core,
                        $"/characters/fightingStyles/?search={fightingStyle.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(fightingStyleSearchTerm);
                }

                await _tableStorage.AddBatchAsync<FightingStyle>($"fightingStyles{_localization.Language}", fightingStyles,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB fighting styles.");
            }

            try
            {
                var fightingMasteriesProcessor = new ExpandedContentCustomizationOptionsFightingMasteryProcessor();
                var fightingMasteries = await fightingMasteriesProcessor.Process(new List<string> { "PHB.phb_06.txt" }, _localization, ContentType.Core);

                foreach (var fightingMastery in fightingMasteries)
                {
                    fightingMastery.ContentSourceEnum = ContentSource.PHB;

                    var fightingMasterySearchTerm = _globalSearchTermRepository.CreateSearchTerm(fightingMastery.Name, GlobalSearchTermType.FightingMastery, ContentType.Core,
                        $"/characters/fightingMasteries/?search={fightingMastery.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(fightingMasterySearchTerm);
                }

                await _tableStorage.AddBatchAsync<FightingMastery>($"fightingMasteries{_localization.Language}", fightingMasteries,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB fighting masteries.");
            }

            try
            {
                var rules =
                    await _playerHandbookChapterRulesProcessor.Process(_phbFilesNames, _localization);

                await _blobContainerClient.CreateIfNotExistsAsync();
                foreach (var chapterRules in rules)
                {
                    var json = JsonConvert.SerializeObject(chapterRules);
                    var blobClient = _blobContainerClient.GetBlobClient($"{chapterRules.ChapterName}.json");

                    var content = Encoding.UTF8.GetBytes(json);
                    using (var ms = new MemoryStream(content))
                    {
                        await blobClient.UploadAsync(ms, true);
                    }
                }
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB rules.");
            }

            try
            {
                var weaponProperties =
                    await _weaponPropertyProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_05.txt")).ToList(), _localization);

                var specialProperty = weaponProperties.SingleOrDefault(w => w.Name == "Special");
                if (specialProperty != null)
                {
                    specialProperty.Content =
                        "#### Special\r\nA weapon with the special property has unusual rules governing its use, explained in the weapon's description.";
                }

                await _tableStorage.AddBatchAsync<WeaponProperty>($"weaponProperties{_localization.Language}", weaponProperties,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB weapon properties.");
            }

            try
            {
                var armorProperties =
                    await _armorPropertyProcessor.Process(_phbFilesNames.Where(p => p.Equals("PHB.phb_05.txt")).ToList(), _localization);

                await _tableStorage.AddBatchAsync<ArmorProperty>($"armorProperties{_localization.Language}", armorProperties,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload PHB armor properties.");
            }

            foreach (var referenceName in ReferenceNames)
            {
                var referenceSearchTerm = _globalSearchTermRepository.CreateSearchTerm(referenceName.name, referenceName.globalSearchTermType, ContentType.Core,
                    referenceName.pathOverride);
                _globalSearchTermRepository.SearchTerms.Add(referenceSearchTerm);
            }

            //foreach (var variantRuleName in SectionNames.VariantRuleNames)
            //{
            //    var variantRuleSearchTerm = _globalSearchTermRepository.CreateSearchTerm(variantRuleName.name, variantRuleName.globalSearchTermType, ContentType.Core,
            //        variantRuleName.pathOverride);
            //    _globalSearchTermRepository.SearchTerms.Add(variantRuleSearchTerm);
            //}

            return powers;
        }
    }
}
