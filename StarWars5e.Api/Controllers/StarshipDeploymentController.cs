using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Api.Storage;
using StarWars5e.Models.Enums;
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
        public async Task<ActionResult<IEnumerable<StarshipDeployment>>> Get(Language language = Language.en)
        {
            List<StarshipDeployment> starshipDeployments;
            try
            {
                starshipDeployments = (await _tableStorage.GetAllAsync<StarshipDeployment>($"starshipDeployments{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    starshipDeployments = (await _tableStorage.GetAllAsync<StarshipDeployment>($"starshipDeployments{Language.en}")).ToList();
                    return Ok(starshipDeployments);
                }
                throw;
            }
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
