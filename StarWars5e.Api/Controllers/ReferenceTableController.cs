using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Storage;
using StarWars5e.Models;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceTableController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public ReferenceTableController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReferenceTable>>> Get()
        {
            var referenceTables = await _tableStorage.GetAllAsync<ReferenceTable>("referenceTables");
            return Ok(referenceTables);
        }
    }
}
