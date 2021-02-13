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
    public class FeatureController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public FeatureController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feature>>> Get(Language language = Language.en)
        {
            List<Feature> features;
            try
            {
                features = (await _tableStorage.GetAllAsync<Feature>($"features{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    features = (await _tableStorage.GetAllAsync<Feature>($"features{Language.en}")).ToList();
                    return Ok(features);
                }
                throw;
            }
            return Ok(features);
        }
    }
}
