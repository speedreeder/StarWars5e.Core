using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;
using StarWars5e.Parser.Storage;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentCustomizationOptionsManager
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly ExpandedContentCustomizationOptionsFeatProcessor _expandedContentCustomizationOptionsFeatProcessor;
        private readonly ExpandedContentCustomizationOptionsFightingStyleProcessor _expandedContentCustomizationOptionsFightingStyleProcessor;
        private readonly ExpandedContentCustomizationOptionsFightingMasteryProcessor _expandedContentCustomizationOptionsFightingMasteryProcessor;
        private readonly List<string> _ecCustomizationOptionsFileName = new List<string> { "ec_06.txt" };
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly ILocalization _localization;

        public ExpandedContentCustomizationOptionsManager(IAzureTableStorage tableStorage,
            GlobalSearchTermRepository globalSearchTermRepository, ILocalization localization)
        {
            _tableStorage = tableStorage;
            _expandedContentCustomizationOptionsFeatProcessor = new ExpandedContentCustomizationOptionsFeatProcessor();
            _expandedContentCustomizationOptionsFightingStyleProcessor = new ExpandedContentCustomizationOptionsFightingStyleProcessor();
            _expandedContentCustomizationOptionsFightingMasteryProcessor = new ExpandedContentCustomizationOptionsFightingMasteryProcessor();
            _globalSearchTermRepository = globalSearchTermRepository;
            _localization = localization;
        }

        public async Task Parse()
        {
            try
            {
                var ecFeats = await _expandedContentCustomizationOptionsFeatProcessor.Process(_ecCustomizationOptionsFileName, _localization);

                foreach (var feat in ecFeats)
                {
                    feat.ContentSourceEnum = ContentSource.EC;

                    var featSearchTerm = _globalSearchTermRepository.CreateSearchTerm(feat.Name, GlobalSearchTermType.Feat, ContentType.ExpandedContent,
                        $"/characters/feats/?search={feat.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(featSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Feat>($"feats{_localization.Language}", ecFeats,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload EC feats.");
            }

            try
            {
                var expandedContentCustomizationOptionsLightsaberFormsProcessor = new ExpandedContentCustomizationOptionsLightsaberFormsProcessor();

                var ecLightsaberForms =
                    await expandedContentCustomizationOptionsLightsaberFormsProcessor.Process(
                        _ecCustomizationOptionsFileName, _localization, ContentType.ExpandedContent);

                foreach (var lightsaberForm in ecLightsaberForms)
                {
                    lightsaberForm.ContentSourceEnum = ContentSource.EC;

                    var lightsaberFormSearchTerm = _globalSearchTermRepository.CreateSearchTerm(lightsaberForm.Name, GlobalSearchTermType.LightsaberForm, ContentType.ExpandedContent,
                        $"/characters/lightsaberForms/?search={lightsaberForm.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(lightsaberFormSearchTerm);
                }

                await _tableStorage.AddBatchAsync<LightsaberForm>($"lightsaberForms{_localization.Language}", ecLightsaberForms,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload EC fighting styles.");
            }

            //try
            //{
            //    var ecFightingStyles =
            //        await _expandedContentCustomizationOptionsFightingStyleProcessor.Process(
            //            _ecCustomizationOptionsFileName, _localization, ContentType.ExpandedContent);

            //    foreach (var fightingStyle in ecFightingStyles)
            //    {
            //        fightingStyle.ContentSourceEnum = ContentSource.EC;

            //        var fightingStyleSearchTerm = _globalSearchTermRepository.CreateSearchTerm(fightingStyle.Name, GlobalSearchTermType.FightingStyle, ContentType.ExpandedContent,
            //            $"/characters/fightingStyles/?search={fightingStyle.Name}");
            //        _globalSearchTermRepository.SearchTerms.Add(fightingStyleSearchTerm);
            //    }

            //    await _tableStorage.AddBatchAsync<FightingStyle>($"fightingStyles{_localization.Language}", ecFightingStyles,
            //        new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            //}
            //catch (StorageException)
            //{
            //    Console.WriteLine("Failed to upload EC fighting styles.");
            //}

            //try
            //{
            //    var ecFightingMasteries =
            //        await _expandedContentCustomizationOptionsFightingMasteryProcessor.Process(
            //            _ecCustomizationOptionsFileName, _localization, ContentType.ExpandedContent);

            //    foreach (var fightingMastery in ecFightingMasteries)
            //    {
            //        fightingMastery.ContentSourceEnum = ContentSource.EC;

            //        var fightingMasterySearchTerm = _globalSearchTermRepository.CreateSearchTerm(fightingMastery.Name, GlobalSearchTermType.FightingMastery, ContentType.ExpandedContent,
            //            $"/characters/fightingMasteries/?search={fightingMastery.Name}");
            //        _globalSearchTermRepository.SearchTerms.Add(fightingMasterySearchTerm);
            //    }

            //    await _tableStorage.AddBatchAsync<FightingStyle>($"fightingMasteries{_localization.Language}", ecFightingMasteries,
            //        new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            //}
            //catch (StorageException)
            //{
            //    Console.WriteLine("Failed to upload EC fighting masteries.");
            //}
        }
    }
}
