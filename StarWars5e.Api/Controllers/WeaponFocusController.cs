using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Api.Storage;
using StarWars5e.Models.CustomizationOptions;
using StarWars5e.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeaponFocusController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public WeaponFocusController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeaponFocus>>> Get(Language language = Language.en)
        {
            List<WeaponFocus> weaponFocuses;
            try
            {
                weaponFocuses = (await _tableStorage.GetAllAsync<WeaponFocus>($"weaponFocuses{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    weaponFocuses = (await _tableStorage.GetAllAsync<WeaponFocus>($"weaponFocuses{Language.en}")).ToList();
                    return Ok(weaponFocuses);
                }
                throw;
            }
            return Ok(weaponFocuses);
        }
    }
}
