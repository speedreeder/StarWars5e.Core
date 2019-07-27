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
    public class StarshipBaseSizeController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;

        public StarshipBaseSizeController(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<StarshipBaseSize>>> Get()
        {
            var starshipBaseSizes = await _tableStorage.GetAllAsync<StarshipBaseSize>("starshipBaseSizes");
            return Ok(starshipBaseSizes);
        }

        //[HttpPost]
        //public void Post([FromBody] StarshipBaseSize starshipBaseSize)
        //{
        //}

        //[HttpPut("{name}")]
        //public void Put(string name, [FromBody] StarshipBaseSize starshipBaseSize)
        //{
        //}

        //[HttpDelete("{name}")]
        //public void Delete(string name)
        //{
        //}
    }
}
