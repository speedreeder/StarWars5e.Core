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
    public class ClassImprovementController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public ClassImprovementController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassImprovement>>> Get(Language language = Language.en)
        {
            List<ClassImprovement> classImprovements;
            try
            {
                classImprovements = (await _tableStorage.GetAllAsync<ClassImprovement>($"classImprovements{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    classImprovements = (await _tableStorage.GetAllAsync<ClassImprovement>($"classImprovements{Language.en}")).ToList();
                    return Ok(classImprovements);
                }
                throw;
            }
            return Ok(classImprovements);
        }
    }
}
