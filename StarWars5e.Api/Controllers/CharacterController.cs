using System.Collections.Generic;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Character>>> Get()
        {
            var owner = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(owner))
            {
                return BadRequest();
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
                return BadRequest();
            }

            var newCharacter = await _characterManager.SaveCharacterAsync(characterRequest, userId);

            return Ok(newCharacter);
        }
    }
}
