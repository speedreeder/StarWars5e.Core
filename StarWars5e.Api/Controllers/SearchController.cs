using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchManager _searchManager;

        public SearchController(ISearchManager searchManager)
        {
            _searchManager = searchManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GlobalSearchTerm>>> Get([FromQuery] string searchText, Language language = Language.en)
        {
            var searchResults = await _searchManager.RunGlobalSearch(searchText, language);
            return Ok(searchResults);
        }
    }
}
