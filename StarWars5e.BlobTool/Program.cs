using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace StarWars5e.BlobTool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json", true, true)
                .AddUserSecrets<Program>()
                .Build();

            var blobContainerClient = new BlobContainerClient(config["StorageAccountConnectionString"], "characters");

            GetBlobs(blobContainerClient).Wait();
        }

        private static async Task GetBlobs(BlobContainerClient blobContainerClient)
        {
            var blobs = blobContainerClient.GetBlobsAsync();

            var jsonBlobs = new List<BlobItem>();
            var nonJsonBlobs = new List<BlobItem>();

            await foreach (var blob in blobs)
            {
                if (blob.Name.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase))
                {
                    jsonBlobs.Add(blob);
                }
                else
                {
                    nonJsonBlobs.Add(blob);
                }
            }

            //get blobs that currently have a duplicate
            var duplicateBlobs = jsonBlobs.Intersect(nonJsonBlobs, new CompareBlobByNameWithoutJsonExtension()).ToList();
            await jsonBlobs.Except(duplicateBlobs).ToList().AsyncParallelForEach(async jsonBlob =>
            {
                Console.WriteLine($"Copying {jsonBlob.Name}");
                await HandleCopyAsync(jsonBlob, blobContainerClient);
            }, Environment.ProcessorCount - 1);

            //delete the existing duplicates
            await duplicateBlobs.AsyncParallelForEach(async duplicateBlob =>
            {
                Console.WriteLine($"Deleting {duplicateBlob.Name}");
                await HandleDeleteAsync(duplicateBlob, blobContainerClient);
            }, Environment.ProcessorCount - 1);
        }

        private static async Task HandleCopyAsync(BlobItem blobItem, BlobContainerClient blobContainerClient)
        {
            var existingBlobClient = blobContainerClient.GetBlobClient(blobItem.Name);
            var newBlobClient = blobContainerClient.GetBlobClient(blobItem.Name.Split(".json")[0]);

            await newBlobClient.StartCopyFromUriAsync(existingBlobClient.Uri);
            Console.WriteLine($"Starting: Copying {existingBlobClient.Name} to {newBlobClient.Name}");

            var destProperties = await newBlobClient.GetPropertiesAsync();

            await WaitUntilAsync(() => destProperties.Value.CopyStatus != CopyStatus.Pending);

            Console.WriteLine($"Complete: Copying {existingBlobClient.Name} to {newBlobClient.Name}");

            Console.WriteLine($"Starting: Deleting {existingBlobClient.Name}");
            await existingBlobClient.DeleteAsync();
            Console.WriteLine($"Complete: Deleting {existingBlobClient.Name}");
        }

        private static async Task HandleDeleteAsync(BlobItem blobItem, BlobContainerClient blobContainerClient)
        {
            Console.WriteLine($"Starting: DUPE Deleting {blobItem.Name}");
            var blobClient = blobContainerClient.GetBlobClient(blobItem.Name);
            await blobClient.DeleteAsync();
            Console.WriteLine($"Complete: DUPE Deleting {blobItem.Name}");
        }

        private static async Task WaitUntilAsync(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (!condition()) await Task.Delay(frequency);
            });

            if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)))
            {
                throw new TimeoutException();
            }
        }
    }

    public class CompareBlobByNameWithoutJsonExtension : IEqualityComparer<BlobItem>
    {
        public bool Equals(BlobItem x, BlobItem y)
        {
            return x?.Name.Split(".json")[0] == y?.Name.Split(".json")[0];
        }

        public int GetHashCode(BlobItem output)
        {
            var hashName = output.Name?.Split(".json")[0] == null ? 0 : output.Name.Split(".json")[0].GetHashCode();
            return hashName;
        }
    }
}