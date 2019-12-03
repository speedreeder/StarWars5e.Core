using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Models.Lookup;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/characterAdvancement")]
    [ApiController]
    public class CharacterAdvancementLUController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;
        public CharacterAdvancementLUController(ITableStorage tableStorage)
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
