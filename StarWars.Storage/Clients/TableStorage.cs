using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using StarWars5e.Models.Monster;

namespace StarWars.Storage.Clients
{
    public interface ITableStorageConnection
    {
        Task AddMonsters(List<Monster> monsters);
    }

    /// <summary>
    /// Connection to the Azure Table Storage
    /// </summary>
    public class TableStorageConnection : ITableStorageConnection
    {
        private CloudTable monsterTable;

        public TableStorageConnection()
        {
            this.InitConnection();
        }

        private void InitConnection()
        {
            var storageAccount = CloudStorageAccount.Parse("lawl-no-dice");
            var tableClient = storageAccount.CreateCloudTableClient();
            this.monsterTable = tableClient.GetTableReference("monsters");
            this.monsterTable.CreateIfNotExistsAsync().Wait();
        }

        public async Task AddMonsters(List<Monster> monsters)
        {
            foreach (var input in monsters)
            {
                try
                {
                    // potential to make this a common guard check across all types of inserts
                    if (string.IsNullOrEmpty(input.PartitionKey))
                    {
                        if (string.IsNullOrEmpty(input.MonsterType))
                        {
                            continue;
                        }
                        input.PartitionKey = input.MonsterType;
                    }

                    if (string.IsNullOrEmpty(input.RowKey))
                    {
                        input.RowKey = Guid.NewGuid().ToString();
                    }
                    var insertOperation = TableOperation.Insert(input);
                    await this.monsterTable.ExecuteAsync(insertOperation);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            }
            
        }
    }
}