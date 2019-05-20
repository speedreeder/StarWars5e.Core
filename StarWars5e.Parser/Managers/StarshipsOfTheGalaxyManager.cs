using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using StarWars5e.Models;
using StarWars5e.Models.Starship;
using StarWars5e.Parser.Parsers;
using StarWars5e.Parser.Parsers.SOTG;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Parser.Managers
{
    public class StarshipsOfTheGalaxyManager
    {
        private readonly ITableStorage _tableStorage;
        private readonly CloudBlobContainer _cloudBlobContainer;

        private readonly IBaseProcessor<StarshipDeployment> _starshipDeploymentProcessor;
        private readonly IBaseProcessor<StarshipEquipment> _starshipEquipmentProcessor;
        private readonly IBaseProcessor<StarshipModification> _starshipModificationProcessor;
        private readonly IBaseProcessor<StarshipBaseSize> _starshipSizeProcessor;
        private readonly IBaseProcessor<StarshipVenture> _starshipVentureProcessor;
        private readonly IBaseProcessor<ChapterRules> _starshipChapterRulesProcessor;
        private readonly List<string> _sotgFilesName = new List<string>
        {
            "SOTG.sotg_00.txt", "SOTG.sotg_01.txt", "SOTG.sotg_02.txt", "SOTG.sotg_03.txt", "SOTG.sotg_04.txt", "SOTG.sotg_05.txt", "SOTG.sotg_06.txt",
            "SOTG.sotg_07.txt", "SOTG.sotg_08.txt", "SOTG.sotg_09.txt", "SOTG.sotg_10.txt", "SOTG.sotg_aa.txt"
        };

        public StarshipsOfTheGalaxyManager(ITableStorage tableStorage, CloudStorageAccount cloudStorageAccount)
        {
            _tableStorage = tableStorage;
            _starshipDeploymentProcessor = new StarshipDeploymentProcessor();
            _starshipEquipmentProcessor = new StarshipEquipmentProcessor();
            _starshipModificationProcessor = new StarshipModificationProcessor();
            _starshipSizeProcessor = new StarshipSizeProcessor();
            _starshipVentureProcessor = new StarshipVentureProcessor();
            _starshipChapterRulesProcessor = new StarshipChapterRulesProcessor();

            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            _cloudBlobContainer = cloudBlobClient.GetContainerReference("starships-rules");
        }

        public async Task Parse(List<ReferenceTable> referenceTables = null)
        {
            var rules = await _starshipChapterRulesProcessor.Process(_sotgFilesName);

            if (referenceTables != null)
            {
                foreach (var chapterRule in rules)
                {
                    foreach (var referenceTable in referenceTables)
                    {
                        chapterRule.ContentMarkdown = Regex.Replace(chapterRule.ContentMarkdown,
                            $@"(?<!#\s*){referenceTable.Name}", $"[{referenceTable.Name}]", RegexOptions.IgnoreCase);
                    }
                }
            }

            await _cloudBlobContainer.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Off, null, null);
            foreach (var chapterRules in rules)
            {
                var json = JsonConvert.SerializeObject(chapterRules);
                var blob = _cloudBlobContainer.GetBlockBlobReference($"{chapterRules.ChapterName}.json");

                await blob.UploadTextAsync(json);
            }

            var deployments =
                await _starshipDeploymentProcessor.Process(_sotgFilesName.Where(f => f.Equals("SOTG.sotg_02.txt")).ToList());

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
                                        $@"(?<!#\s*){referenceTable.Name}", $"[{referenceTable.Name}]",
                                        RegexOptions.IgnoreCase);
                                }
                            }   
                        }
                    }
                }
            }

            await _tableStorage.AddBatchAsync<StarshipDeployment>("starshipDeployments", deployments,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });


            var equipment = await _starshipEquipmentProcessor.Process(_sotgFilesName.Where(f => f.Equals("SOTG.sotg_05.txt")).ToList());

            if (referenceTables != null)
            {
                foreach (var starshipEquipment in equipment)
                {
                    if (starshipEquipment.Description != null)
                    {
                        foreach (var referenceTable in referenceTables)
                        {
                            starshipEquipment.Description = Regex.Replace(starshipEquipment.Description,
                                $@"(?<!#\s*){referenceTable.Name}", $"[{referenceTable.Name}]", RegexOptions.IgnoreCase);
                        }
                    }
                }
            }

            await _tableStorage.AddBatchAsync<StarshipEquipment>("starshipEquipment", equipment,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var modifications = await _starshipModificationProcessor.Process(_sotgFilesName.Where(f => f.Equals("SOTG.sotg_04.txt")).ToList());

            if (referenceTables != null)
            {
                foreach (var modification in modifications)
                {
                    if (modification.Content != null)
                    {
                        foreach (var referenceTable in referenceTables)
                        {
                            modification.Content = Regex.Replace(modification.Content,
                                $@"(?<!#\s*){referenceTable.Name}", $"[{referenceTable.Name}]", RegexOptions.IgnoreCase);
                        }
                    }
                }
            }

            await _tableStorage.AddBatchAsync<StarshipModification>("starshipModifications", modifications,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var sizes = await _starshipSizeProcessor.Process(_sotgFilesName.Where(f => f.Equals("SOTG.sotg_03.txt")).ToList());
            await _tableStorage.AddBatchAsync<StarshipBaseSize>("starshipBaseSizes", sizes,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });

            var ventures = await _starshipVentureProcessor.Process(_sotgFilesName.Where(f => f.Equals("SOTG.sotg_06.txt")).ToList());
            await _tableStorage.AddBatchAsync<StarshipVenture>("starshipVentures", ventures,
                new BatchOperationOptions { BatchInsertMethod = BatchInsertMethod.InsertOrReplace });
        }
    }
}
