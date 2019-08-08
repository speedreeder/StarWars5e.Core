using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Parsers;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class ExpandedContentCustomizationOptionsManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly ExpandedContentCustomizationOptionsProcessor _expandedContentCustomizationOptionsProcessor;
        private readonly List<string> _ecCustomizationOptionsFileName = new List<string> { "ec_customization_options.txt" };
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;

        public ExpandedContentCustomizationOptionsManager(ITableStorage tableStorage,
            GlobalSearchTermRepository globalSearchTermRepository)
        {
            _tableStorage = tableStorage;
            _expandedContentCustomizationOptionsProcessor = new ExpandedContentCustomizationOptionsProcessor();
            _globalSearchTermRepository = globalSearchTermRepository;
        }

        public async Task Parse()
        {
            try
            {
                var ecFeats = await _expandedContentCustomizationOptionsProcessor.Process(_ecCustomizationOptionsFileName);

                foreach (var feat in ecFeats)
                {
                    var featSearchTerm = _globalSearchTermRepository.CreateSearchTerm(feat.Name, GlobalSearchTermType.Feat, ContentType.ExpandedContent,
                        $"/characters/feats/?search={feat.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(featSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Feat>("feats", ecFeats,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload EC feats.");
            }
        }
    }
}
