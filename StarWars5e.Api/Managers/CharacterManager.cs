using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using StarWars5e.Api.Interfaces;
using StarWars5e.Api.Storage;
using StarWars5e.Models.Character;
using StarWars5e.Models.Enums;

namespace StarWars5e.Api.Managers
{
    public class CharacterManager : ICharacterManager
    {
        private readonly IConfiguration _configuration;
        private readonly IAzureTableStorage _tableStorage;

        public CharacterManager(IConfiguration configuration, IAzureTableStorage tableStorage)
        {
            _configuration = configuration;
            _tableStorage = tableStorage;
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

            var ownedCharacterBlobs = await GetRawCharacterBlobsAsync(blobContainerClient, userId);

            var characters = new List<Character>();
            foreach (var characterBlob in ownedCharacterBlobs)
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

                var tableCharacter = await _tableStorage.GetAsync<Character>("characters",
                    ContentType.CommunityContent.ToString(), character.Id);

                if (tableCharacter != null)
                {
                    character.UserPermissions = tableCharacter.UserPermissions;
                }

                characters.Add(character);
            }

            return characters;
        }

        public async Task<Character> SaveCharacterAsync(PostCharacterRequest characterRequest, string userId,
            CharacterPermissionLevel permission)
        {
            var blobContainerClient =
                new BlobContainerClient(_configuration["StorageAccountConnectionString"], "characters");
            await blobContainerClient.CreateIfNotExistsAsync();

            if (string.IsNullOrWhiteSpace(characterRequest.Id))
            {
                characterRequest.Id = Guid.NewGuid().ToString();
            }

            var blobClient = blobContainerClient.GetBlobClient($"{userId}/{characterRequest.Id}");

            var content = Encoding.UTF8.GetBytes(characterRequest.JsonData);
            await using var ms = new MemoryStream(content);
            await blobClient.UploadAsync(ms, true);

            var character = new Character
            {
                RowKey = characterRequest.Id,
                PartitionKey = ContentType.CommunityContent.ToString(),
                ContentSourceEnum = ContentSource.Community,
                ContentTypeEnum = ContentType.CommunityContent,
                JsonData = characterRequest.JsonData,
                Id = characterRequest.Id
            };

            if (permission == CharacterPermissionLevel.Owner)
            {
                character.UserId = userId;
                character.UserPermissions = characterRequest.UserPermissions;
            }

            await _tableStorage.AddOrUpdateAsync("characters", character);

            return character;
        }

        public async Task DeleteCharacterForUser(string userId, string characterId)
        {
            var blobContainerClient =
                new BlobContainerClient(_configuration["StorageAccountConnectionString"], "characters");
            var blobClient = blobContainerClient.GetBlobClient($"{userId}/{characterId}");

            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<Character> GetCharacterSettingsAsync(string characterId)
        {
            var character =
                await _tableStorage.GetAsync<Character>("characters", ContentType.CommunityContent.ToString(),
                    characterId);
            return character;
        }

        public async Task<CharacterPermissionLevel> CheckCharacterPermissionLevelForUser(string characterId, string userId)
        {
            var characterSettings = await GetCharacterSettingsAsync(characterId);
            var blobContainerClient =
                new BlobContainerClient(_configuration["StorageAccountConnectionString"], "characters");

            var blobCharacterClient = blobContainerClient.GetBlobClient($"{userId}/{characterId}");

            if (await blobCharacterClient.ExistsAsync())
            {
                return CharacterPermissionLevel.Owner;
            }

            var permission = characterSettings.UserPermissions.Where(u => u.Key == userId).ToList();

            if (permission.Any(p => p.Value == CharacterPermissionLevel.Owner))
            {
                return CharacterPermissionLevel.Owner;
            }

            if (permission.Any(p => p.Value == CharacterPermissionLevel.Write))
            {
                return CharacterPermissionLevel.Write;
            }

            if (permission.Any(p => p.Value == CharacterPermissionLevel.Read))
            {
                return CharacterPermissionLevel.Read;
            }

            return CharacterPermissionLevel.None;
        }
    }
}
