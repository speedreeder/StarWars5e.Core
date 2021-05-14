using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web.Resource;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Character;
using StarWars5e.Models.Enums;

namespace StarWars5e.Api.Controllers
{
    [Authorize]
    [Route("api/character")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterManager _characterManager;
        private readonly IConfiguration _configuration;

        private readonly int _characterLimitPerUser;

        public CharacterController(ICharacterManager characterManager, IConfiguration configuration)
        {
            _characterManager = characterManager;
            _configuration = configuration;

            _characterLimitPerUser =
                int.TryParse(configuration["CharacterLimitPerUser"], out var intValue) ? intValue : 50;
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

            var blobContainerClient = new BlobContainerClient(_configuration["StorageAccountConnectionString"], "characters");

            var permission = CharacterPermissionLevel.Owner;
            if (!string.IsNullOrWhiteSpace(characterRequest.Id))
            {
                permission =
                    await _characterManager.CheckCharacterPermissionLevelForUser(characterRequest.Id, userId);

                if (permission != CharacterPermissionLevel.Owner && permission != CharacterPermissionLevel.Write)
                {
                    return BadRequest("User does not have permission to change this character.");
                }
            }

            var currentCharactersForUser = await _characterManager.GetRawCharacterBlobsAsync(blobContainerClient, userId);
            if (currentCharactersForUser.Count >= _characterLimitPerUser)
            {
                return BadRequest($"User already has {_characterLimitPerUser} characters saved.");
            }

            var newCharacter = await _characterManager.SaveCharacterAsync(characterRequest, userId, permission);

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

            var blobContainerClient = new BlobContainerClient(_configuration["StorageAccountConnectionString"], "characters");
            var currentCharactersForUser = await _characterManager.GetRawCharacterBlobsAsync(blobContainerClient, userId);

            if (currentCharactersForUser.Count >= _characterLimitPerUser)
            {
                return BadRequest($"User already has {_characterLimitPerUser} characters saved.");
            }

            var characters = new List<Character>();

            var numberOfCharactersToAdd =
                _characterLimitPerUser - currentCharactersForUser.Count < postCharactersRequest.CharacterRequests.Count
                    ? _characterLimitPerUser - currentCharactersForUser.Count
                    : postCharactersRequest.CharacterRequests.Count;

            foreach (var postCharacterRequest in postCharactersRequest.CharacterRequests.Take(numberOfCharactersToAdd))
            {
                if (!string.IsNullOrWhiteSpace(postCharacterRequest.Id) &&
                    !Guid.TryParse(postCharacterRequest.Id, out _))
                {
                    return BadRequest("Invalid character Id.");
                }

                var permission = CharacterPermissionLevel.Owner;
                if (!string.IsNullOrWhiteSpace(postCharacterRequest.Id))
                {
                    permission =
                        await _characterManager.CheckCharacterPermissionLevelForUser(postCharacterRequest.Id, userId);

                    if (permission != CharacterPermissionLevel.Owner && permission != CharacterPermissionLevel.Write)
                    {
                        return BadRequest($"User does not have permission to change character {postCharacterRequest.Id}.");
                    }
                }

                var newCharacter =
                    await _characterManager.SaveCharacterAsync(postCharacterRequest, userId, permission);

                characters.Add(newCharacter);
            }

            return Ok(characters);
        }
    }
}
