using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using StarWars5e.Models;
using StarWars5e.Models.Equipment;
using StarWars5e.Models.Interfaces;
using StarWars5e.Models.Monster;
using StarWars5e.Models.ViewModels;

namespace StarWars.Storage.Clients
{
    public interface ITableStorageConnection
    {
        Task AddMonsters(List<Monster> monsters);

        /// <summary>
        /// Given a list of powers this will bulk insert them into the backing database
        /// </summary>
        /// <param name="powers"></param>
        /// <returns></returns>
        Task UpsertPowers(List<Power> powers);

        /// <summary>
        /// Given a single species this will insert it into the backing database
        /// </summary>
        /// <param name="species"></param>
        /// <returns></returns>
        Task AddSpecies(Species species);

        Task AddBackground(BackgroundViewModel background);

        Task<List<Power>> GetAllPowers();

        Task AddItem(IEquipment item);
    }

    /// <summary>
    /// Connection to the Azure Table Storage
    /// </summary>
    public class TableStorageConnection : ITableStorageConnection
    {
        private CloudTable monsterTable;
        private CloudTable powerTable;
        private CloudTable backgroundTable;
        private CloudTable speciesTable;
        private CloudTable equipmentTable;

        public TableStorageConnection()
        {
            this.InitConnection();
        }

        private void InitConnection()
        {
            var storageAccount = CloudStorageAccount.Parse("removed");
            var tableClient = storageAccount.CreateCloudTableClient();
            this.monsterTable = tableClient.GetTableReference("monsters");
            this.powerTable = tableClient.GetTableReference("powers");
            this.speciesTable = tableClient.GetTableReference("species");
            this.backgroundTable = tableClient.GetTableReference("backgrounds");
            this.equipmentTable = tableClient.GetTableReference("equipment");
            this.powerTable.CreateIfNotExistsAsync().Wait();
            this.monsterTable.CreateIfNotExistsAsync().Wait();
            this.speciesTable.CreateIfNotExistsAsync().Wait();
            this.backgroundTable.CreateIfNotExistsAsync().Wait();
            this.equipmentTable.CreateIfNotExistsAsync().Wait();
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

        public async Task UpsertPowers(List<Power> powers)
        {
            foreach (var power in powers)
            {
                try
                {
                    if (string.IsNullOrEmpty(power.PowerType))
                    {
                        throw new ArgumentException("All powers must have a type");
                    }

                    power.PartitionKey = power.PowerType;
                    if (string.IsNullOrEmpty(power.RowKey))
                    {
                        power.RowKey = Guid.NewGuid().ToString();
                    }
                    var insertOperation = TableOperation.InsertOrMerge(power);
                    await this.powerTable.ExecuteAsync(insertOperation);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public async Task<List<Power>> GetAllPowers()
        {
            var q = new TableQuery<Power>();
            TableContinuationToken token = null;
            var entities = new List<Power>();
            do
            {
                var queryResult = await this.powerTable.ExecuteQuerySegmentedAsync(new TableQuery<Power>(), token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return entities;
        }

        public async Task AddSpecies(Species species)
        {
            var insertOperation = TableOperation.Insert(species);
            await this.speciesTable.ExecuteAsync(insertOperation);
        }

        public async Task AddBackground(BackgroundViewModel background)
        {
            var insertOperation = TableOperation.Insert(background);
            await this.backgroundTable.ExecuteAsync(insertOperation);
        }

        public async Task AddItem(IEquipment item)
        {
            var bob = item as ITableEntity;
            var insertOperation = TableOperation.Insert(bob);
            await this.equipmentTable.ExecuteAsync(insertOperation);
        }
        public async Task UpsertPowers(List<Power> powers)
        {
            foreach (var power in powers)
            {
                try
                {
                    if (string.IsNullOrEmpty(power.PowerType))
                    {
                        throw new ArgumentException("All powers must have a type");
                    }

                    power.PartitionKey = power.PowerType;
                    if (string.IsNullOrEmpty(power.RowKey))
                    {
                        power.RowKey = Guid.NewGuid().ToString();
                    }
                    var insertOperation = TableOperation.Insert(power);
                    await this.powerTable.ExecuteAsync(insertOperation);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public async Task AddSpecies(Species species)
        {
            var insertOperation = TableOperation.Insert(species);
            await this.speciesTable.ExecuteAsync(insertOperation);
        }

        public async Task AddBackground(BackgroundViewModel background)
        {
            var insertOperation = TableOperation.Insert(background);
            await this.backgroundTable.ExecuteAsync(insertOperation);
        }
    }
}