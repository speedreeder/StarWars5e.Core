using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Models.Lookup;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/skills")]
    [ApiController]
    public class SkillsLUController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;
        public SkillsLUController(ITableStorage tableStorage)
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
