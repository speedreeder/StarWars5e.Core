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
    public class StarshipEquipmentController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public StarshipEquipmentController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StarshipEquipment>>> Get(Language language = Language.en)
        {
            List<StarshipEquipment> starshipEquipment;
            try
            {
                starshipEquipment = (await _tableStorage.GetAllAsync<StarshipEquipment>($"starshipEquipment{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    starshipEquipment = (await _tableStorage.GetAllAsync<StarshipEquipment>($"starshipEquipment{Language.en}")).ToList();
                    return Ok(starshipEquipment);
                }
                throw;
            }
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
