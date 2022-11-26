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
    public class FightingStrategyController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public FightingStrategyController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FightingStrategy>>> Get(Language language = Language.en)
        {
            List<FightingStrategy> FightingStrategy;
            try
            {
                FightingStrategy = (await _tableStorage.GetAllAsync<FightingStrategy>($"fightingStrategies{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    FightingStrategy = (await _tableStorage.GetAllAsync<FightingStrategy>($"fightingStrategies{Language.en}")).ToList();
                    return Ok(FightingStrategy);
                }
                throw;
            }
            return Ok(FightingStrategy);
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
