using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Storage;
using StarWars5e.Models.Lookup;

namespace StarWars5e.Api.Controllers
{
    [Route("api/skills")]
    [ApiController]
    public class SkillsLUController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;
        public SkillsLUController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillLU>>> Get()
        {
            var skillLUs = await _tableStorage.GetAllAsync<SkillLU>("skillsLU");
            return Ok(skillLUs);
        }
    }
}
