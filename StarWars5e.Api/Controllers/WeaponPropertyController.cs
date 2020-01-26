using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Storage;
using StarWars5e.Models;

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
        public async Task<ActionResult<IEnumerable<WeaponProperty>>> Get()
        {
            var weaponProperties = await _tableStorage.GetAllAsync<WeaponProperty>("weaponProperties");
            return Ok(weaponProperties);
        }
    }
}
