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
    public class MulticlassImprovementController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public MulticlassImprovementController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MulticlassImprovement>>> Get(Language language = Language.en)
        {
            List<MulticlassImprovement> multiclassImprovements;
            try
            {
                multiclassImprovements = (await _tableStorage.GetAllAsync<MulticlassImprovement>($"multiclassImprovements{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    multiclassImprovements = (await _tableStorage.GetAllAsync<MulticlassImprovement>($"multiclassImprovements{Language.en}")).ToList();
                    return Ok(multiclassImprovements);
                }
                throw;
            }
            return Ok(multiclassImprovements);
        }
    }
}
