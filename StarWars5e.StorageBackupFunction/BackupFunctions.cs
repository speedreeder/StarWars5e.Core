using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace StarWars5e.StorageBackupFunction
{
    public static class BackupFunctions
    {
        [FunctionName("BackupTablesAndBlobs")]
        public static async Task Run([TimerTrigger("0 0 7 * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            var sourceAccountName = Environment.GetEnvironmentVariable("BackupSourceAccountName");
            var sourceKey = Environment.GetEnvironmentVariable("BackupSourceAccountKey");

            var backupAzureStorage = new Luminis.AzureStorageBackup.BackupAzureStorage(sourceAccountName, sourceKey, log, context.FunctionAppDirectory);

            var destinationAccountName = Environment.GetEnvironmentVariable("BackupDestinationAccountName");
            var destinationKey = Environment.GetEnvironmentVariable("BackupDestinationAccountKey");
            var destinationContainerName = Environment.GetEnvironmentVariable("BackupDestinationContainer");

            // Backup Tables
            //await backupAzureStorage.BackupAzureTablesToBlobStorage("table1,table2", destinationAccountName, destinationKey, destinationContainerName, "tables");

            // Backup Blobs
            await backupAzureStorage.BackupBlobStorage(new List<string>{"characters"}, destinationAccountName, destinationKey, destinationContainerName, "blobs");
        }
    }
}
