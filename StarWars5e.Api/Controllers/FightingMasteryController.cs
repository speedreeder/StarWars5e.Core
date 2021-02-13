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
    public class FightingMasteryController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public FightingMasteryController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FightingMastery>>> Get(Language language = Language.en)
        {
            List<FightingMastery> fightingMasteries;
            try
            {
                fightingMasteries = (await _tableStorage.GetAllAsync<FightingMastery>($"fightingMasteries{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    fightingMasteries = (await _tableStorage.GetAllAsync<FightingMastery>($"fightingMasteries{Language.en}")).ToList();
                    return Ok(fightingMasteries);
                }
                throw;
            }
            return Ok(fightingMasteries);
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
