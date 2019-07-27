using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Background;
using StarWars5e.Models.Search;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/background")]
    [AllowAnonymous]
    [ApiController]
    public class BackgroundController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;
        private readonly IBackgroundManager _backgroundManager;

        public BackgroundController(ITableStorage tableStorage, IBackgroundManager backgroundManager)
        {
            _tableStorage = tableStorage;
            _backgroundManager = backgroundManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Background>>> Get()
        {
            var backgrounds = await _tableStorage.GetAllAsync<Background>("backgrounds");
            return Ok(backgrounds);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Background>>> Get([FromQuery] BackgroundSearch backgroundSearch)
        {
            var backgrounds = await _backgroundManager.SearchBackgrounds(backgroundSearch);
            return Ok(backgrounds);
        }

        //[HttpPost]
        //public async Task Post([FromBody] Background background)
        //{
        //    await _tableStorage.AddOrUpdateAsync("backgrounds", background);
        //}

        //[HttpDelete("{name}")]
        //public async Task Delete(string name)
        //{
        //    var query = new TableQuery<Background>();
        //    query.Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, name));

        //    var backgrounds = await _tableStorage.QueryAsync("backgrounds", query);
        //    foreach (var background in backgrounds)
        //    {
        //        await _tableStorage.DeleteAsync("backgrounds", background);
        //    }
        //}
    }
}
