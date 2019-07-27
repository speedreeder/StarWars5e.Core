using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Models.Starship;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StarshipEquipmentController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;

        public StarshipEquipmentController(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<StarshipEquipment>>> Get()
        {
            var starshipEquipment = await _tableStorage.GetAllAsync<StarshipEquipment>("starshipEquipment");
            return Ok(starshipEquipment);
        }

        //[HttpPost]
        //public void Post([FromBody] StarshipEquipment starshipEquipment)
        //{
        //}

        //[HttpPut("{name}")]
        //public void Put(string name, [FromBody] StarshipEquipment starshipEquipment)
        //{
        //}

        //[HttpDelete("{name}")]
        //public void Delete(string name)
        //{
        //}
    }
}
