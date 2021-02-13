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
    public class FeatController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public FeatController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feat>>> Get(Language language = Language.en)
        {
            List<Feat> feats;
            try
            {
                feats = (await _tableStorage.GetAllAsync<Feat>($"feats{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    feats = (await _tableStorage.GetAllAsync<Feat>($"feats{Language.en}")).ToList();
                    return Ok(feats);
                }
                throw;
            }
            return Ok(feats);
        }

        //[HttpPost]
        //public void Post([FromBody] Feat feat)
        //{
        //}

        //[HttpDelete("{name}")]
        //public void Delete(string name)
        //{
        //}
    }
}
