using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.EnhancedItems;
using StarWars5e.Models.Search;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/enhancedItem")]
    [ApiController]
    public class EnhancedItemController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;
        private readonly IEnhancedItemManager _enhancedItemManager;

        public EnhancedItemController(ITableStorage tableStorage, IEnhancedItemManager enhancedItemManager)
        {
            _tableStorage = tableStorage;
            _enhancedItemManager = enhancedItemManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnhancedItem>>> Get()
        {
            var enhancedItems = await _tableStorage.GetAllAsync<EnhancedItem>("enhancedItems");
            return Ok(enhancedItems);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<PagedSearchResult<EnhancedItem>>>> Get([FromQuery] EnhancedItemSearch enhancedItemSearch)
        {
            var enhancedItems = await _enhancedItemManager.SearchEnhancedItems(enhancedItemSearch);

            return Ok(enhancedItems);
        }
    }
}
