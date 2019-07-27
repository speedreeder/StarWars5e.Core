using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models;
using StarWars5e.Models.Search;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/power")]
    [ApiController]
    public class PowerController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;
        private readonly IPowerManager _powerManager;

        public PowerController(ITableStorage tableStorage, IPowerManager powerManager)
        {
            _tableStorage = tableStorage;
            _powerManager = powerManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Power>>> Get()
        {
            var powers = await _tableStorage.GetAllAsync<Power>("powers");
            return Ok(powers);
        }

        [HttpGet("search")]
        [AllowAnonymous]
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
