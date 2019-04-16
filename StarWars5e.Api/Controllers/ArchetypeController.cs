using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Models.Class;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchetypeController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;

        public ArchetypeController(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Archetype>>> Get()
        {
            var archetypes = await _tableStorage.GetAllAsync<Archetype>("archetypes");
            return Ok(archetypes);
        }

        [HttpPost]
        public void Post([FromBody] Archetype archetype)
        {
        }

        [HttpPut("{name}")]
        public void Put(string name, [FromBody] Archetype archetype)
        {
        }

        [HttpDelete("{name}")]
        public void Delete(string name)
        {
        }
    }
}
