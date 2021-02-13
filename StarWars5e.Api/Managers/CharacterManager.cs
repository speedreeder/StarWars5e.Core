using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Character;

namespace StarWars5e.Api.Managers
{
    public class CharacterManager : ICharacterManager
    {
        private readonly IConfiguration _configuration;

        public CharacterManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<BlobItem>> GetRawCharacterBlobsAsync(BlobContainerClient blobContainerClient, string userId)
        {
            var characterBlobs = new List<BlobItem>();
            try
            {
                var blobs = blobContainerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None, userId);

                await foreach (var blob in blobs)
                {
                    characterBlobs.Add(blob);
                }
            }
            catch (Exception e)
            {
                var x = e;
            }
            
            return characterBlobs;
        }

        public async Task<IEnumerable<Character>> GetCharactersForUserAsync(string userId)
        {
            var blobContainerClient = new BlobContainerClient(_configuration["StorageAccountConnectionString"], "characters");

            var characterBlobs = await GetRawCharacterBlobsAsync(blobContainerClient, userId);

            var characters = new List<Character>();
            foreach (var characterBlob in characterBlobs)
            {
                var blobClient = blobContainerClient.GetBlobClient($"{characterBlob.Name}");

                var stream = new MemoryStream();
                await blobClient.DownloadToAsync(stream);

                stream.Seek(0, SeekOrigin.Begin);

                var character = new Character
                {
                    JsonData = await new StreamReader(stream).ReadToEndAsync(),
                    Id = characterBlob.Name.Split('/')[1].Split('.')[0],
                    UserId = userId
                };
                characters.Add(character);
            }

            return characters;
        }

        public async Task<Character> SaveCharacterAsync(PostCharacterRequest characterRequest, string userId)
        {
            var blobContainerClient = new BlobContainerClient(_configuration["StorageAccountConnectionString"], "characters");
            await blobContainerClient.CreateIfNotExistsAsync();

            if (string.IsNullOrWhiteSpace(characterRequest.Id))
            {
                characterRequest.Id = Guid.NewGuid().ToString();
            }

            var blobClient = blobContainerClient.GetBlobClient($"{userId}/{characterRequest.Id}");

            var content = Encoding.UTF8.GetBytes(characterRequest.JsonData);
            await using var ms = new MemoryStream(content);
            await blobClient.UploadAsync(ms);

            return new Character
            {
                JsonData = characterRequest.JsonData,
                Id = characterRequest.Id,
                UserId = userId
            };
        }

        public async Task DeleteCharacterForUser(string userId, string characterId)
        {
            var blobContainerClient = new BlobContainerClient(_configuration["StorageAccountConnectionString"], "characters");
            var blobClient = blobContainerClient.GetBlobClient($"{userId}/{characterId}");

            await blobClient.DeleteIfExistsAsync();
        }
    }
}
