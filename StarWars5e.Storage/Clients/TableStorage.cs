using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Models;
using StarWars5e.Models.Interfaces;
using StarWars5e.Models.Monster;
using StarWars5e.Models.ViewModels;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Storage.Clients
{
    public interface ITableStorageConnection
    {

        CloudTable MonsterTable { get; }
        CloudTable PowerTable { get; }
        CloudTable BackgroundTable { get; }
        CloudTable SpeciesTable { get; }
        CloudTable EquipmentTable { get; }
        CloudTable ModificationsTable { get; }
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
        Task AddModifications(TableBatchOperation modificationOperation);
    }

    /// <summary>
    /// Connection to the Azure Table Storage
    /// </summary>
    public class TableStorageConnection : ITableStorageConnection
    {
        public CloudTable MonsterTable { get; private set; }
        public CloudTable PowerTable { get; private set; }
        public CloudTable BackgroundTable { get; private set; }
        public CloudTable SpeciesTable { get; private set; }
        public CloudTable EquipmentTable { get; private set; }
        public CloudTable ModificationsTable { get; private set; }

        public TableStorageConnection()
        {
            InitConnection();
        }

        private void InitConnection()
        {
            var storageAccount = CloudStorageAccount.Parse("removed");
            var tableClient = storageAccount.CreateCloudTableClient();

            MonsterTable = tableClient.GetTableReference("monsters");
            PowerTable = tableClient.GetTableReference("powers");
            SpeciesTable = tableClient.GetTableReference("species");
            BackgroundTable = tableClient.GetTableReference("backgrounds");
            EquipmentTable = tableClient.GetTableReference("equipment");
            ModificationsTable = tableClient.GetTableReference("modifications");
            PowerTable.CreateIfNotExistsAsync().Wait();
            MonsterTable.CreateIfNotExistsAsync().Wait();
            SpeciesTable.CreateIfNotExistsAsync().Wait();
            BackgroundTable.CreateIfNotExistsAsync().Wait();
            EquipmentTable.CreateIfNotExistsAsync().Wait();
            ModificationsTable.CreateIfNotExistsAsync().Wait();
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
                    await this.MonsterTable.ExecuteAsync(insertOperation);
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
                    await this.PowerTable.ExecuteAsync(insertOperation);
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
                var queryResult = await PowerTable.ExecuteQuerySegmentedAsync(new TableQuery<Power>(), token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return entities;
        }

        public async Task AddSpecies(Species species)
        {
            var insertOperation = TableOperation.Insert(species);
            await this.SpeciesTable.ExecuteAsync(insertOperation);
        }

        public async Task AddBackground(BackgroundViewModel background)
        {
            var insertOperation = TableOperation.Insert(background);
            await this.BackgroundTable.ExecuteAsync(insertOperation);
        }

        public async Task AddItem(IEquipment item)
        {
            var bob = item as ITableEntity;
            var insertOperation = TableOperation.Insert(bob);
            await this.EquipmentTable.ExecuteAsync(insertOperation);
        }

        public async Task AddModifications(TableBatchOperation modificationOperation)
        {
            await ModificationsTable.ExecuteBatchAsync(modificationOperation);
        }
    }
}