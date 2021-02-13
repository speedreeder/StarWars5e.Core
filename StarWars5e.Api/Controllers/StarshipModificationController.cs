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
    public class StarshipModificationController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public StarshipModificationController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StarshipModification>>> Get(Language language = Language.en)
        {
            List<StarshipModification> starshipModifications;
            try
            {
                starshipModifications = (await _tableStorage.GetAllAsync<StarshipModification>($"starshipModifications{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    starshipModifications = (await _tableStorage.GetAllAsync<StarshipModification>($"starshipModifications{Language.en}")).ToList();
                    return Ok(starshipModifications);
                }
                throw;
            }
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
