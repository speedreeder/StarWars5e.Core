using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Class;
using StarWars5e.Models.Search;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchetypeController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;
        private readonly IArchetypeManager _archetypeManager;

        public ArchetypeController(ITableStorage tableStorage, IArchetypeManager archetypeManager)
        {
            _tableStorage = tableStorage;
            _archetypeManager = archetypeManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Archetype>>> Get()
        {
            var archetypes = await _tableStorage.GetAllAsync<Archetype>("archetypes");
            return Ok(archetypes);
        }

        [HttpGet("/search1")]
        public async Task<ActionResult<List<Archetype>>> Get([FromQuery] ArchetypeSearch archetypeSearch)
        {
            var archetypes = await _archetypeManager.SearchArchetypes(archetypeSearch);
            if (!archetypes.Any()) return NotFound();

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
