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
    public class StarshipBaseSizeController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public StarshipBaseSizeController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StarshipBaseSize>>> Get(Language language = Language.en)
        {
            List<StarshipBaseSize> starshipBaseSizes;
            try
            {
                starshipBaseSizes = (await _tableStorage.GetAllAsync<StarshipBaseSize>($"starshipBaseSizes{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    starshipBaseSizes = (await _tableStorage.GetAllAsync<StarshipBaseSize>($"starshipBaseSizes{Language.en}")).ToList();
                    return Ok(starshipBaseSizes);
                }
                throw;
            }
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
