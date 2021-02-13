using StarWars5e.Models.Character;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace StarWars5e.Api.Interfaces
{
    public interface ICharacterManager
    {
        Task DeleteCharacterForUser(string userId, string characterId);
        Task<IEnumerable<Character>> GetCharactersForUserAsync(string userName);
        Task<List<BlobItem>> GetRawCharacterBlobsAsync(BlobContainerClient blobContainerClient, string userId);
        Task<Character> SaveCharacterAsync(PostCharacterRequest characterRequest, string userId);
    }
}