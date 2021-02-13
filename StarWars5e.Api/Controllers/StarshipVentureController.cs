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
    public class StarshipVentureController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public StarshipVentureController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StarshipVenture>>> Get(Language language = Language.en)
        {
            List<StarshipVenture> starshipVentures;
            try
            {
                starshipVentures = (await _tableStorage.GetAllAsync<StarshipVenture>($"starshipVentures{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    starshipVentures = (await _tableStorage.GetAllAsync<StarshipVenture>($"starshipVentures{Language.en}")).ToList();
                    return Ok(starshipVentures);
                }
                throw;
            }
            return Ok(starshipVentures);
        }

        //[HttpPost]
        //public void Post([FromBody] Class starshipVenture)
        //{
        //}

        //[HttpPut("{name}")]
        //public void Put(string name, [FromBody] Class starshipVenture)
        //{
        //}

        //[HttpDelete("{name}")]
        //public void Delete(string name)
        //{
        //}
    }
}
