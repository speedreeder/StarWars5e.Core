using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Api.Storage;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Controllers
{
    [Route("api/power")]
    [ApiController]
    public class PowerController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly IPowerManager _powerManager;

        public PowerController(IAzureTableStorage tableStorage, IPowerManager powerManager)
        {
            _tableStorage = tableStorage;
            _powerManager = powerManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Power>>> Get(Language language = Language.en)
        {
            List<Power> powers;
            try
            {
                powers = (await _tableStorage.GetAllAsync<Power>($"powers{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    powers = (await _tableStorage.GetAllAsync<Power>($"powers{Language.en}")).ToList();
                    return Ok(powers);
                }
                throw;
            }
            return Ok(powers);
        }

        [HttpGet("search")]
        public async Task<ActionResult<Power>> Get([FromQuery] PowerSearch powerSearch)
        {
            var powers = await _powerManager.SearchPowers(powerSearch);

            return Ok(powers);
        }

        //[HttpPost]
        //public async Task Post([FromBody] Power power)
        //{
        //    await _tableStorage.AddOrUpdateAsync("powers", power);
        //}

        //[HttpDelete("{name}")]
        //public async Task Delete(string name)
        //{
        //    var query = new TableQuery<Power>();
        //    query.Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, name));

        //    var powers = await _tableStorage.QueryAsync("powers", query);
        //    foreach (var power in powers)
        //    {
        //        await _tableStorage.DeleteAsync("powers", power);
        //    }
        //}
    }
}
