using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Api.Storage;
using StarWars5e.Models.EnhancedItems;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Controllers
{
    [Route("api/enhancedItem")]
    [ApiController]
    public class EnhancedItemController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly IEnhancedItemManager _enhancedItemManager;

        public EnhancedItemController(IAzureTableStorage tableStorage, IEnhancedItemManager enhancedItemManager)
        {
            _tableStorage = tableStorage;
            _enhancedItemManager = enhancedItemManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnhancedItem>>> Get(Language language = Language.en)
        {
            List<EnhancedItem> enhancedItems;
            try
            {
                enhancedItems = (await _tableStorage.GetAllAsync<EnhancedItem>($"enhancedItems{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    enhancedItems = (await _tableStorage.GetAllAsync<EnhancedItem>($"enhancedItems{Language.en}")).ToList();
                    return Ok(enhancedItems);
                }
                throw;
            }
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
