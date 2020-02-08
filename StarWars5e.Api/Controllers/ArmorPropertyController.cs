using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Storage;
using StarWars5e.Models;

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
        public async Task<ActionResult<IEnumerable<ArmorProperty>>> Get()
        {
            var armorProperties = await _tableStorage.GetAllAsync<ArmorProperty>("armorProperties");
            return Ok(armorProperties);
        }
    }
}
