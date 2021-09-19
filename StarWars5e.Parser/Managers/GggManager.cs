using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.DependencyInjection;
using StarWars5e.Models;
using StarWars5e.Models.CustomizationOptions;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;
using StarWars5e.Parser.Processors.GGG;
using StarWars5e.Parser.Storage;

namespace StarWars5e.Parser.Managers
{
    public class GggManager
    {
        private readonly IAzureTableStorage _tableStorage;

        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly BlobContainerClient _blobContainerClient;

        private readonly ILocalization _localization;

        private readonly List<string> _gggFileNames = new List<string>
        {
            "GGG.ggg_06.txt"
        };

        public GggManager(IServiceProvider serviceProvider, ILocalization localization)
        {
            _tableStorage = serviceProvider.GetService<IAzureTableStorage>();
            _globalSearchTermRepository = serviceProvider.GetService<GlobalSearchTermRepository>();
            _localization = localization;

            var blobServiceClient = serviceProvider.GetService<BlobServiceClient>();
            _blobContainerClient = blobServiceClient.GetBlobContainerClient($"ggg-rules-{_localization.Language}");

        }

        public async Task Parse()
        {
            try
            {
                var featProcessor = new GggFeatProcessor();
                var gggFeats = await featProcessor.Process(_gggFileNames, _localization);

                foreach (var feat in gggFeats)
                {
                    feat.ContentSourceEnum = ContentSource.EC;

                    var featSearchTerm = _globalSearchTermRepository.CreateSearchTerm(feat.Name, GlobalSearchTermType.Feat, ContentType.ExpandedContent,
                        $"/characters/feats/?search={feat.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(featSearchTerm);
                }

                await _tableStorage.AddBatchAsync<Feat>($"feats{_localization.Language}", gggFeats,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload GGG feats.");
            }

            try
            {
                var classImprovementProcessor = new ClassImprovementProcessor();

                var classImprovements =
                    await classImprovementProcessor.Process(
                        _gggFileNames, _localization, ContentType.ExpandedContent);

                foreach (var classImprovement in classImprovements)
                {
                    classImprovement.ContentSourceEnum = ContentSource.EC;

                    var classImprovementSearchTerm = _globalSearchTermRepository.CreateSearchTerm(classImprovement.Name, GlobalSearchTermType.ClassImprovement, ContentType.ExpandedContent,
                        $"/characters/customizationOptions/classImprovements/?search={classImprovement.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(classImprovementSearchTerm);
                }

                await _tableStorage.AddBatchAsync<ClassImprovement>($"classImprovements{_localization.Language}", classImprovements,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload GGG class improvements.");
            }

            try
            {
                var multiclassImprovementProcessor = new MulticlassImprovementProcessor();

                var multiclassImprovements =
                    await multiclassImprovementProcessor.Process(
                        _gggFileNames, _localization, ContentType.ExpandedContent);

                foreach (var multiclassImprovement in multiclassImprovements)
                {
                    multiclassImprovement.ContentSourceEnum = ContentSource.EC;

                    var multiclassImprovementSearchTerm = _globalSearchTermRepository.CreateSearchTerm(multiclassImprovement.Name, GlobalSearchTermType.MulticlassImprovement, ContentType.ExpandedContent,
                        $"/characters/customizationOptions/multiclassImprovements/?search={multiclassImprovement.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(multiclassImprovementSearchTerm);
                }

                await _tableStorage.AddBatchAsync<MulticlassImprovement>($"multiclassImprovements{_localization.Language}", multiclassImprovements,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload GGG multiclass improvements.");
            }

            try
            {
                var splashclassImprovementProcessor = new SplashclassImprovementProcessor();

                var splashclassImprovements =
                    await splashclassImprovementProcessor.Process(
                        _gggFileNames, _localization, ContentType.ExpandedContent);

                foreach (var splashclassImprovement in splashclassImprovements)
                {
                    splashclassImprovement.ContentSourceEnum = ContentSource.EC;

                    var splashclassImprovementSearchTerm = _globalSearchTermRepository.CreateSearchTerm(splashclassImprovement.Name, GlobalSearchTermType.SplashclassImprovement, ContentType.ExpandedContent,
                        $"/characters/customizationOptions/splashclassImprovements/?search={splashclassImprovement.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(splashclassImprovementSearchTerm);
                }

                await _tableStorage.AddBatchAsync<SplashclassImprovement>($"splashclassImprovements{_localization.Language}", splashclassImprovements,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload GGG splashclass improvements.");
            }
        }
    }
}
