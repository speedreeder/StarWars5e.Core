using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Models.Starship;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StarshipModificationController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;

        public StarshipModificationController(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StarshipModification>>> Get()
        {
            var starshipModifications = await _tableStorage.GetAllAsync<StarshipModification>("starshipModifications");
            return Ok(starshipModifications);
        }

        //[HttpPost]
        //public void Post([FromBody] StarshipModification starshipModification)
        //{
        //}

        //[HttpPut("{name}")]
        //public void Put(string name, [FromBody] StarshipModification starshipModification)
        //{
        //}

        //[HttpDelete("{name}")]
        //public void Delete(string name)
        //{
        //}
    }
}
