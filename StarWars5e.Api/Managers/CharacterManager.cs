using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Character;

namespace StarWars5e.Api.Managers
{
    public class CharacterManager : ICharacterManager
    {
        private readonly CloudBlobClient _cloudBlobClient;

        public CharacterManager(CloudBlobClient cloudBlobClient)
        {
            _cloudBlobClient = cloudBlobClient;
        }

        public async Task<IEnumerable<Character>> GetCharactersForUserAsync(string userId)
        {
            var blobContainer = _cloudBlobClient.GetContainerReference("characters");
            var directory = blobContainer.GetDirectoryReference(userId);
            var characters = new List<Character>();

            var characterBlobs = await ListBlobsAsync(directory);

            foreach (var characterBlob in characterBlobs)
            {
                var blob = new CloudBlockBlob(characterBlob.Uri, directory.ServiceClient);

                var character = new Character
                {
                    JsonData = await blob.DownloadTextAsync(),
                    Id = blob.Name.Split('/')[1].Split('.')[0],
                    UserId = userId
                };
                characters.Add(character);
            }

            return characters;
        }

        public async Task<Character> SaveCharacterAsync(PostCharacterRequest characterRequest, string userId)
        {
            var blobContainer = _cloudBlobClient.GetContainerReference("characters");

            if (string.IsNullOrWhiteSpace(characterRequest.Id))
            {
                characterRequest.Id = Guid.NewGuid().ToString();
            }

            var blob = blobContainer.GetBlockBlobReference($"{userId}/{characterRequest.Id}.json");

            await blob.UploadTextAsync(characterRequest.JsonData);

            return new Character
            {
                JsonData = characterRequest.JsonData,
                Id = characterRequest.Id,
                UserId = userId
            };
        }

        public async Task DeleteCharacterForUser(string userId, string characterId)
        {
            var blobContainer = _cloudBlobClient.GetContainerReference("characters");
            var blob = blobContainer.GetBlockBlobReference($"{userId}/{characterId}.json");

            await blob.DeleteIfExistsAsync();
        }

        private async Task<List<IListBlobItem>> ListBlobsAsync(CloudBlobDirectory directory)
        {
            BlobContinuationToken continuationToken = null;
            var results = new List<IListBlobItem>();
            do
            {
                var response = await directory.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                results.AddRange(response.Results);
            }
            while (continuationToken != null);
            return results;
        }
    }
}
