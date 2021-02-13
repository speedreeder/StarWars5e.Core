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
    public class WeaponPropertyController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public WeaponPropertyController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeaponProperty>>> Get(Language language = Language.en)
        {
            List<WeaponProperty> weaponProperties;
            try
            {
                weaponProperties = (await _tableStorage.GetAllAsync<WeaponProperty>($"weaponProperties{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    weaponProperties = (await _tableStorage.GetAllAsync<WeaponProperty>($"weaponProperties{Language.en}")).ToList();
                    return Ok(weaponProperties);
                }
                throw;
            }
            return Ok(weaponProperties);
        }
    }
}
