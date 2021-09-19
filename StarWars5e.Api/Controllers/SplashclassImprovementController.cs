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
    public class SplashclassImprovementController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public SplashclassImprovementController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SplashclassImprovement>>> Get(Language language = Language.en)
        {
            List<SplashclassImprovement> splashclassImprovements;
            try
            {
                splashclassImprovements = (await _tableStorage.GetAllAsync<SplashclassImprovement>($"splashclassImprovements{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    splashclassImprovements = (await _tableStorage.GetAllAsync<SplashclassImprovement>($"splashclassImprovements{Language.en}")).ToList();
                    return Ok(splashclassImprovements);
                }
                throw;
            }
            return Ok(splashclassImprovements);
        }
    }
}
