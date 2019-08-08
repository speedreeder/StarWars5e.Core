using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Models.Class;
using StarWars5e.Models.Enums;
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
                    var archetypeSearchTerm = _globalSearchTermRepository.CreateSearchTerm(archetype.Name,
                        GlobalSearchTermType.Archetype, ContentType.ExpandedContent,
                        $"/characters/archetypes/{archetype.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(archetypeSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Archetype>("archetypes", archetypes,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload EC archetypes.");
            }
        }
    }
}
