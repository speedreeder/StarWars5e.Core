using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Models.Species;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeciesController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;

        public SpeciesController(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Species>>> Get()
        {
            var species = await _tableStorage.GetAllAsync<Species>("species");
            return Ok(species);
        }

        [HttpPost]
        public void Post([FromBody] Species species)
        {
        }

        [HttpPut("{name}")]
        public void Put(string name, [FromBody] Species species)
        {
        }

        [HttpDelete("{name}")]
        public void Delete(string name)
        {
        }
    }
}
