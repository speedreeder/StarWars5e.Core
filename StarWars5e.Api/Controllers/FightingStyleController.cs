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
    public class FightingStyleController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public FightingStyleController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FightingStyle>>> Get(Language language = Language.en)
        {
            List<FightingStyle> fightingStyles;
            try
            {
                fightingStyles = (await _tableStorage.GetAllAsync<FightingStyle>($"fightingStyles{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    fightingStyles = (await _tableStorage.GetAllAsync<FightingStyle>($"fightingStyles{Language.en}")).ToList();
                    return Ok(fightingStyles);
                }
                throw;
            }
            return Ok(fightingStyles);
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
