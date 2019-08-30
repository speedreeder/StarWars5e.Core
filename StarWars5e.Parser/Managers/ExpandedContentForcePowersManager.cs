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
    public class ExpandedContentForcePowersManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly ExpandedContentForcePowersProcessor _forcePowersProcessor;
        private readonly List<string> _ecForcePowersFileName = new List<string> { "ec_force_powers.txt" };

        public ExpandedContentForcePowersManager(ITableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _forcePowersProcessor = new ExpandedContentForcePowersProcessor();
        }

        public async Task Parse()
        {
            try
            {
                var forcePowers = await _forcePowersProcessor.Process(_ecForcePowersFileName);

                foreach (var forcePower in forcePowers)
                {
                    var forcePowerSearchTerm = _globalSearchTermRepository.CreateSearchTerm(forcePower.Name,
                        GlobalSearchTermType.ForcePower, ContentType.ExpandedContent,
                        $"/characters/forcePowers/{forcePower.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(forcePowerSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Power>("powers", forcePowers,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload EC force powers.");
            }
        }
    }
}
