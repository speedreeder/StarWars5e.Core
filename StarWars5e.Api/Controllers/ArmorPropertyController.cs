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
    public class ArmorPropertyController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public ArmorPropertyController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArmorProperty>>> Get(Language language = Language.en)
        {
            List<ArmorProperty> armorProperties;
            try
            {
                armorProperties = (await _tableStorage.GetAllAsync<ArmorProperty>($"armorProperties{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    armorProperties = (await _tableStorage.GetAllAsync<ArmorProperty>($"armorProperties{Language.en}")).ToList();
                    return Ok(armorProperties);
                }
                throw;
            }
            return Ok(armorProperties);
        }
    }
}
