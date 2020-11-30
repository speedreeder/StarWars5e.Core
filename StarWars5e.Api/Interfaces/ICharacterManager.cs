using StarWars5e.Models.Character;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarWars5e.Api.Interfaces
{
    public interface ICharacterManager
    {
        Task DeleteCharacterForUser(string userId, string characterId);
        Task<IEnumerable<Character>> GetCharactersForUserAsync(string userName);
        Task<Character> SaveCharacterAsync(PostCharacterRequest characterRequest, string userId);
    }
}