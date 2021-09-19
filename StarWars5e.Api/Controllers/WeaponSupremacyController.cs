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
    public class WeaponSupremacyController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public WeaponSupremacyController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeaponSupremacy>>> Get(Language language = Language.en)
        {
            List<WeaponSupremacy> weaponSupremacies;
            try
            {
                weaponSupremacies = (await _tableStorage.GetAllAsync<WeaponSupremacy>($"weaponSupremacies{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    weaponSupremacies = (await _tableStorage.GetAllAsync<WeaponSupremacy>($"weaponSupremacies{Language.en}")).ToList();
                    return Ok(weaponSupremacies);
                }
                throw;
            }
            return Ok(weaponSupremacies);
        }
    }
}
