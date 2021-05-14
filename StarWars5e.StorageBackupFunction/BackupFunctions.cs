using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using TheByteStuff.AzureTableUtilities;

namespace StarWars5e.StorageBackupFunction
{
    public static class BackupFunctions
    {
        [FunctionName("BackupTablesAndBlobs")]
        public static void Run([TimerTrigger("0 0 7 * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            var backupCs = Environment.GetEnvironmentVariable("BackupStorageConnectionString");
            var targetCs = Environment.GetEnvironmentVariable("TargetStorageConnectionString");

            var backup = new BackupAzureTables(backupCs, targetCs);

            backup.BackupTableToBlobDirect("characters", "backups", true);
            backup.BackupTableToBlobDirect("users", "backups", true);
        }
    }
}
