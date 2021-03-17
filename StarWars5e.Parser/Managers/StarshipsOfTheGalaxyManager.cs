using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Starship;
using StarWars5e.Parser.Localization;
using StarWars5e.Parser.Processors;
using StarWars5e.Parser.Processors.SOTG;
using StarWars5e.Parser.Storage;

namespace StarWars5e.Parser.Managers
{
    public class StarshipsOfTheGalaxyManager
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly IBaseProcessor<StarshipDeployment> _starshipDeploymentProcessor;
        private readonly IBaseProcessor<StarshipEquipment> _starshipEquipmentProcessor;
        private readonly IBaseProcessor<StarshipModification> _starshipModificationProcessor;
        private readonly IBaseProcessor<StarshipBaseSize> _starshipSizeProcessor;
        private readonly IBaseProcessor<StarshipVenture> _starshipVentureProcessor;
        private readonly IBaseProcessor<ChapterRules> _starshipChapterRulesProcessor;
        private readonly GlobalSearchTermRepository _globalSearchTermRepository;
        private readonly BlobContainerClient _blobContainerClient;

        private readonly ILocalization _localization;

        private readonly List<string> _sotgFilesName = new List<string>
        {
            "SOTG.sotg_00.txt", "SOTG.sotg_01.txt", "SOTG.sotg_02.txt", "SOTG.sotg_03.txt", "SOTG.sotg_04.txt", "SOTG.sotg_05.txt", "SOTG.sotg_06.txt",
            "SOTG.sotg_07.txt", "SOTG.sotg_08.txt", "SOTG.sotg_09.txt", "SOTG.sotg_10.txt", "SOTG.sotg_aa.txt", "SOTG.sotg_changelog.txt"
        };

        public StarshipsOfTheGalaxyManager(IServiceProvider serviceProvider, ILocalization localization)
        {
            _localization = localization;
            _tableStorage = serviceProvider.GetService<IAzureTableStorage>();
            _globalSearchTermRepository = serviceProvider.GetService<GlobalSearchTermRepository>();
            _starshipDeploymentProcessor = new StarshipDeploymentProcessor();
            _starshipEquipmentProcessor = new StarshipEquipmentProcessor();
            _starshipModificationProcessor = new StarshipModificationProcessor();
            _starshipSizeProcessor = new StarshipSizeProcessor();
            _starshipVentureProcessor = new StarshipVentureProcessor();
            _starshipChapterRulesProcessor = new StarshipChapterRulesProcessor(_globalSearchTermRepository);

            var blobServiceClient = serviceProvider.GetService<BlobServiceClient>();
            _blobContainerClient = blobServiceClient.GetBlobContainerClient($"starships-rules-{_localization.Language}");
        }

        public async Task Parse(List<ReferenceTable> referenceTables = null)
        {
            try
            {
                var rules = await _starshipChapterRulesProcessor.Process(_sotgFilesName, _localization);

                if (referenceTables != null)
                {
                    foreach (var chapterRule in rules)
                    {
                        foreach (var referenceTable in referenceTables)
                        {
                            chapterRule.ContentMarkdown = Regex.Replace(chapterRule.ContentMarkdown,
                                $@"(?<!#\s*){referenceTable.Name}", $"[{referenceTable.Name}](#{Uri.EscapeUriString(referenceTable.Name)})", RegexOptions.IgnoreCase);
                        }
                    }
                }

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
                Console.WriteLine("Failed to upload SOTG rules.");
            }

            try
            {
                var deployments =
                    await _starshipDeploymentProcessor.Process(_sotgFilesName.Where(f => f.Equals("SOTG.sotg_02.txt")).ToList(), _localization);

                foreach (var deployment in deployments)
                {
                    deployment.ContentSourceEnum = ContentSource.SotG;

                    var deploymentSearchTerm = _globalSearchTermRepository.CreateSearchTerm(deployment.Name, GlobalSearchTermType.Deployment, ContentType.Core,
                        $"/starships/deployments/{deployment.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(deploymentSearchTerm);
                }

                if (referenceTables != null)
                {
                    foreach (var deployment in deployments)
                    {
                        foreach (var referenceTable in referenceTables)
                        {
                            if (deployment.Features != null)
                            {
                                foreach (var deploymentFeature in deployment.Features)
                                {
                                    if (deploymentFeature.Content != null)
                                    {
                                        deploymentFeature.Content = Regex.Replace(deploymentFeature.Content,
                                            $@"(?<!#\s*){referenceTable.Name}", $"[{referenceTable.Name}](#{Uri.EscapeUriString(referenceTable.Name)})",
                                            RegexOptions.IgnoreCase);
                                    }
                                }
                            }
                        }
                    }
                }

                await _tableStorage.AddBatchAsync<StarshipDeployment>($"starshipDeployments{_localization.Language}", deployments,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload SOTG deployments.");
            }

            try
            {
                var equipment = await _starshipEquipmentProcessor.Process(_sotgFilesName.Where(f => f.Equals("SOTG.sotg_05.txt")).ToList(), _localization);

                if (referenceTables != null)
                {
                    foreach (var starshipEquipment in equipment)
                    {
                        if (starshipEquipment.Description != null)
                        {
                            foreach (var referenceTable in referenceTables)
                            {
                                starshipEquipment.Description = Regex.Replace(starshipEquipment.Description,
                                    $@"(?<!#\s*){referenceTable.Name}", $"[{referenceTable.Name}](#{Uri.EscapeUriString(referenceTable.Name)})", RegexOptions.IgnoreCase);
                            }
                        }
                    }
                }

                var dupes = equipment
                    .GroupBy(i => i.RowKey)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key);

                foreach (var starshipEquipment in equipment)
                {
                    starshipEquipment.ContentSourceEnum = ContentSource.SotG;

                    switch (starshipEquipment.TypeEnum)
                    {
                        case StarshipEquipmentType.Armor:
                        case StarshipEquipmentType.Shield:
                        case StarshipEquipmentType.Ammunition:
                        case StarshipEquipmentType.Hyperdrive:
                        case StarshipEquipmentType.Navcomputer:
                        case StarshipEquipmentType.PowerCoupling:
                        case StarshipEquipmentType.Reactor:
                            var equipmentSearchTerm = _globalSearchTermRepository.CreateSearchTerm(starshipEquipment.Name, GlobalSearchTermType.StarshipEquipment, ContentType.Core,
                                $"/starships/equipment?search={starshipEquipment.Name}");
                            _globalSearchTermRepository.SearchTerms.Add(equipmentSearchTerm);
                            break;
                        case StarshipEquipmentType.Weapon:
                            var weaponSearchTerm = _globalSearchTermRepository.CreateSearchTerm(starshipEquipment.Name, GlobalSearchTermType.StarshipWeapon, ContentType.Core,
                                $"/starships/weapons?search={starshipEquipment.Name}");
                            _globalSearchTermRepository.SearchTerms.Add(weaponSearchTerm);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                await _tableStorage.AddBatchAsync<StarshipEquipment>($"starshipEquipment{_localization.Language}", equipment,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException e)
            {
                Console.WriteLine("Failed to upload SOTG equipment.");
            }

            try
            {
                var modifications = await _starshipModificationProcessor.Process(_sotgFilesName.Where(f => f.Equals("SOTG.sotg_04.txt")).ToList(), _localization);

                if (referenceTables != null)
                {
                    foreach (var modification in modifications)
                    {
                        if (modification.Content != null)
                        {
                            foreach (var referenceTable in referenceTables)
                            {
                                modification.Content = Regex.Replace(modification.Content,
                                    $@"(?<!#\s*){referenceTable.Name}", $"[{referenceTable.Name}](#{Uri.EscapeUriString(referenceTable.Name)})", RegexOptions.IgnoreCase);
                            }
                        }
                    }
                }

                foreach (var modification in modifications)
                {
                    modification.ContentSourceEnum = ContentSource.SotG;

                    var modificationSearchTerm = _globalSearchTermRepository.CreateSearchTerm(modification.Name, GlobalSearchTermType.StarshipModification, ContentType.Core,
                        $"/starships/modifications?search={modification.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(modificationSearchTerm);
                }

                await _tableStorage.AddBatchAsync<StarshipModification>($"starshipModifications{_localization.Language}", modifications,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException e)
            {
                Console.WriteLine("Failed to upload SOTG modifications.");
            }

            try
            {
                var sizes = await _starshipSizeProcessor.Process(_sotgFilesName.Where(f => f.Equals("SOTG.sotg_03.txt")).ToList(), _localization);

                foreach (var size in sizes)
                {
                    size.ContentSourceEnum = ContentSource.SotG;

                    var sizeSearchTerm = _globalSearchTermRepository.CreateSearchTerm(size.Name, GlobalSearchTermType.StarshipSize, ContentType.Core,
                        $"/rules/sotg/starshipSizes/{size.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(sizeSearchTerm);
                }

                await _tableStorage.AddBatchAsync<StarshipBaseSize>($"starshipBaseSizes{_localization.Language}", sizes,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload SOTG sizes.");
            }
            
            try
            {
                var ventures = await _starshipVentureProcessor.Process(_sotgFilesName.Where(f => f.Equals("SOTG.sotg_06.txt")).ToList(), _localization);

                foreach (var venture in ventures)
                {
                    venture.ContentSourceEnum = ContentSource.SotG;

                    var sizeSearchTerm = _globalSearchTermRepository.CreateSearchTerm(venture.Name, GlobalSearchTermType.Venture, ContentType.Core,
                        $"/starships/ventures?search={venture.Name}");
                    _globalSearchTermRepository.SearchTerms.Add(sizeSearchTerm);
                }
                await _tableStorage.AddBatchAsync<StarshipVenture>($"starshipVentures{_localization.Language}", ventures,
                    new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
            }
            catch (StorageException)
            {
                Console.WriteLine("Failed to upload SOTG ventures.");
            }
        }
    }
}
