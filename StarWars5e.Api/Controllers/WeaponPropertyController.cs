using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Models;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeaponPropertyController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;

        public WeaponPropertyController(ITableStorage tableStorage)
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
