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
    public class ReferenceTableController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public ReferenceTableController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReferenceTable>>> Get(Language language = Language.en)
        {
            List<ReferenceTable> referenceTables;
            try
            {
                referenceTables = (await _tableStorage.GetAllAsync<ReferenceTable>($"referenceTables{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    referenceTables = (await _tableStorage.GetAllAsync<ReferenceTable>($"referenceTables{Language.en}")).ToList();
                    return Ok(referenceTables);
                }
                throw;
            }
            return Ok(referenceTables);
        }
    }
}
