using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Storage;
using StarWars5e.Models.Starship;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StarshipDeploymentController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public StarshipDeploymentController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        [AllowAnonymous]
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
