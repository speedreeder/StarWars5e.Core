using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Character;

namespace StarWars5e.Api.Controllers
{
    [Authorize]
    [Route("api/character")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterManager _characterManager;

        public CharacterController(ICharacterManager characterManager)
        {
            _characterManager = characterManager;
        }

        [HttpDelete("{characterId}")]
        public async Task<ActionResult<IEnumerable<Character>>> Delete(string characterId)
        {
            var owner = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(owner))
            {
                return BadRequest("User not found.");
            }

            if (string.IsNullOrWhiteSpace(characterId) || !Guid.TryParse(characterId, out _))
            {
                return BadRequest("Invalid character Id.");
            }

            await _characterManager.DeleteCharacterForUser(owner, characterId);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Character>>> Get()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope("api.readCharacterData");

            var owner = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(owner))
            {
                return BadRequest("User not found.");
            }

            var characters = await _characterManager.GetCharactersForUserAsync(owner);

            return Ok(characters);
        }

        [HttpPost]
        public async Task<ActionResult<Character>> Post(PostCharacterRequest characterRequest)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope("api.writeCharacterData");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("No userId found.");
            }

            if (!string.IsNullOrWhiteSpace(characterRequest.Id) && !Guid.TryParse(characterRequest.Id, out _))
            {
                return BadRequest("Invalid character Id.");
            }

            var currentCharactersForUser = await _characterManager.GetRawCharacterBlobsAsync(userId);

            if (currentCharactersForUser.Count >= 20)
            {
                return BadRequest("User already has 20 characters saved.");
            }

            var newCharacter = await _characterManager.SaveCharacterAsync(characterRequest, userId);

            return Ok(newCharacter);
        }

        [HttpPost("multiple")]
        public async Task<ActionResult<Character>> PostMultipleCharacters(PostCharactersRequest postCharactersRequest)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope("api.writeCharacterData");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("No userId found.");
            }

            var currentCharactersForUser = await _characterManager.GetRawCharacterBlobsAsync(userId);

            if (currentCharactersForUser.Count >= 20)
            {
                return BadRequest("User already has 20 characters saved.");
            }

            var characters = new List<Character>();

            var numberOfCharactersToAdd =
                20 - currentCharactersForUser.Count < postCharactersRequest.CharacterRequests.Count
                    ? 20 - currentCharactersForUser.Count
                    : postCharactersRequest.CharacterRequests.Count;

            foreach (var postCharacterRequest in postCharactersRequest.CharacterRequests.Take(numberOfCharactersToAdd))
            {
                if (!string.IsNullOrWhiteSpace(postCharacterRequest.Id) &&
                    !Guid.TryParse(postCharacterRequest.Id, out _))
                {
                    return BadRequest("Invalid character Id.");
                }

                var newCharacter =
                    await _characterManager.SaveCharacterAsync(postCharacterRequest, userId);

                characters.Add(newCharacter);
            }

            return Ok(characters);
        }
    }
}
