using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Models;
using StarWars5e.Models.Class;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Lookup;
using StarWars5e.Parser.Parsers;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentArchetypesManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly ExpandedContentArchetypeProcessor _archetypeProcessor;
        private readonly List<string> _ecArchetypesFileName = new List<string> { "ec_archetypes.txt" };

        public ExpandedContentArchetypesManager(ITableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _archetypeProcessor = new ExpandedContentArchetypeProcessor();
        }

        public async Task Parse()
        {
            

            try
            {
                var archetypes = await _archetypeProcessor.Process(_ecArchetypesFileName);

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

                    var featureLevels = (await _tableStorage.GetAllAsync<FeatureDataLU>("featureDataLU")).ToList();

                    foreach (var archetypeFeature in archetypeFeatures)
                    {
                        var featureLevel = featureLevels.SingleOrDefault(f => f.FeatureRowKey == archetypeFeature.RowKey);
                        if (featureLevel != null)
                        {
                            archetypeFeature.Level = featureLevel.Level;
                        }
                    }

                    var dupes = archetypeFeatures
                        .GroupBy(i => i.RowKey)
                        .Where(g => g.Count() > 1)
                        .Select(g => g.Key);

                    await _tableStorage.AddBatchAsync<Feature>("features", archetypeFeatures,
                        new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
                }
                catch (StorageException se)
                {
                    Console.WriteLine($"Failed to upload EC archetype features: {se}");
                }

                await _tableStorage.AddBatchAsync<Archetype>("archetypes", archetypes,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            }
            catch (StorageException e)
            {
                Console.WriteLine($"Failed to upload EC archetypes: {e.Message}");
            }
        }
    }
}
