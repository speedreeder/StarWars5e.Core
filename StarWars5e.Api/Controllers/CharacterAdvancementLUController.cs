using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Storage;
using StarWars5e.Models.Lookup;

namespace StarWars5e.Api.Controllers
{
    [Route("api/characterAdvancement")]
    [ApiController]
    public class CharacterAdvancementLUController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;
        public CharacterAdvancementLUController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharacterAdvancementLU>>> Get()
        {
            var characterAdvancementLUs = await _tableStorage.GetAllAsync<CharacterAdvancementLU>("characterAdvancementLU");
            return Ok(characterAdvancementLUs);
        }
    }
}
