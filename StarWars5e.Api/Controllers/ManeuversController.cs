using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Api.Storage;
using StarWars5e.Models;
using StarWars5e.Models.Enums;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManeuversController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public ManeuversController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Maneuver>>> Get(Language language = Language.en)
        {
            List<Maneuver> maneuvers;
            try
            {
                maneuvers = (await _tableStorage.GetAllAsync<Maneuver>($"maneuvers{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    maneuvers = (await _tableStorage.GetAllAsync<Maneuver>($"maneuvers{Language.en}")).ToList();
                    return Ok(maneuvers);
                }
                throw;
            }
            return Ok(maneuvers);
        }
    }
}
