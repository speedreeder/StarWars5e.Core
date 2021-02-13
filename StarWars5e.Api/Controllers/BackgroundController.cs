using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Api.Storage;
using StarWars5e.Models.Background;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Controllers
{
    [Route("api/background")]
    [ApiController]
    public class BackgroundController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly IBackgroundManager _backgroundManager;

        public BackgroundController(IAzureTableStorage tableStorage, IBackgroundManager backgroundManager)
        {
            _tableStorage = tableStorage;
            _backgroundManager = backgroundManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Background>>> Get(Language language = Language.en)
        {
            List<Background> backgrounds;
            try
            {
                backgrounds = (await _tableStorage.GetAllAsync<Background>($"backgrounds{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    backgrounds = (await _tableStorage.GetAllAsync<Background>($"backgrounds{Language.en}")).ToList();
                    return Ok(backgrounds);
                }
                throw;
            }
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
