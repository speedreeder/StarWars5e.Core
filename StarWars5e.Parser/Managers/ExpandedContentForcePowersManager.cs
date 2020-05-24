using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Globalization;
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
        private readonly IGlobalization _globalization;

        public ExpandedContentForcePowersManager(ITableStorage tableStorage, GlobalSearchTermRepository globalSearchTermRepository, IGlobalization globalization)
        {
            _tableStorage = tableStorage;
            _globalSearchTermRepository = globalSearchTermRepository;
            _globalization = globalization;
            _forcePowersProcessor = new ExpandedContentForcePowersProcessor();
        }

        public async Task Parse()
        {
            try
            {
                var forcePowers = await _forcePowersProcessor.Process(_ecForcePowersFileName, _globalization);

                foreach (var forcePower in forcePowers)
                {
                    forcePower.ContentSourceEnum = ContentSource.EC;

                    var forcePowerSearchTerm = _globalSearchTermRepository.CreateSearchTerm(forcePower.Name,
                        GlobalSearchTermType.ForcePower, ContentType.ExpandedContent,
                        $"/characters/forcePowers/?search={forcePower.Name}");
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
