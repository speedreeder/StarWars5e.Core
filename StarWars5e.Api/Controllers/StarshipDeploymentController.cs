using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Models.Starship;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StarshipDeploymentController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;

        public StarshipDeploymentController(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StarshipDeployment>>> Get()
        {
            var starshipDeployments = await _tableStorage.GetAllAsync<StarshipDeployment>("starshipDeployments");
            return Ok(starshipDeployments);
        }

        //[HttpPost]
        //public void Post([FromBody] StarshipDeployment starshipDeployment)
        //{
        //}

        //[HttpPut("{name}")]
        //public void Put(string name, [FromBody] StarshipDeployment starshipDeployment)
        //{
        //}

        //[HttpDelete("{name}")]
        //public void Delete(string name)
        //{
        //}
    }
}
