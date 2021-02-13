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
    public class LightsaberFormController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public LightsaberFormController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LightsaberForm>>> Get(Language language = Language.en)
        {
            List<LightsaberForm> lightsaberForms;
            try
            {
                lightsaberForms = (await _tableStorage.GetAllAsync<LightsaberForm>($"lightsaberForms{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    lightsaberForms = (await _tableStorage.GetAllAsync<LightsaberForm>($"lightsaberForms{Language.en}")).ToList();
                    return Ok(lightsaberForms);
                }
                throw;
            }
            return Ok(lightsaberForms);
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
