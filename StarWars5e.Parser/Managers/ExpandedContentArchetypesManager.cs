using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.DependencyInjection;
using StarWars5e.Models;
using StarWars5e.Models.Class;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Lookup;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;
using StarWars5e.Parser.Storage;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentArchetypesManager
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly List<string> _ecArchetypesFileName = new() { "ec_03.txt" };
        private readonly ILocalization _localization;
        private readonly FeatureRepository _featureRepository;

        public ExpandedContentArchetypesManager(IServiceProvider serviceProvider, ILocalization localization)
        {
            _tableStorage = serviceProvider.GetService<IAzureTableStorage>();
            _globalSearchTermRepository = serviceProvider.GetService<GlobalSearchTermRepository>();
            _localization = localization;
            _featureRepository = serviceProvider.GetService<FeatureRepository>();
        }

        public async Task Parse()
        {
            try
            {
                var classImageLus = await _tableStorage.GetAllAsync<ClassImageLU>("classImageLU");
                var casterRatioLus = await _tableStorage.GetAllAsync<CasterRatioLU>("casterRatioLU");
                var classes = await _tableStorage.GetAllAsync<Class>($"classes{_localization.Language}");

                var archetypeProcessor = new ExpandedContentArchetypeProcessor(classImageLus.ToList(), casterRatioLus.ToList(), classes.ToList());
                var archetypes = await archetypeProcessor.Process(_ecArchetypesFileName, _localization);

                foreach (var archetype in archetypes)
                {
                    archetype.ContentSourceEnum = ContentSource.EC;

                    var archetypeSearchTerm = _globalSearchTermRepository.CreateSearchTerm(archetype.Name,
                        GlobalSearchTermType.Archetype, ContentType.ExpandedContent,
                        $"/characters/archetypes/{Uri.EscapeDataString(archetype.Name)}");
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
                    Console.WriteLine($"Failed to upload EC archetype features: {se}");
                }

                await _tableStorage.AddBatchAsync<Archetype>($"archetypes{_localization.Language}", archetypes,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            }
            catch (StorageException e)
            {
                Console.WriteLine($"Failed to upload EC archetypes: {e.Message}");
            }
        }
    }
}
