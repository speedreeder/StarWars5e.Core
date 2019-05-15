using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Models;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceTableController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;

        public ReferenceTableController(ITableStorage tableStorage)
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
