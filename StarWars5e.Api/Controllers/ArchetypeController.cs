using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Api.Storage;
using StarWars5e.Models.Class;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Controllers
{
    [Route("api/archetype")]
    [ApiController]
    public class ArchetypeController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly IArchetypeManager _archetypeManager;

        public ArchetypeController(IAzureTableStorage tableStorage, IArchetypeManager archetypeManager)
        {
            _tableStorage = tableStorage;
            _archetypeManager = archetypeManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Archetype>>> Get(Language language = Language.en)
        {
            List<Archetype> archetypes;
            try
            {
                archetypes = (await _tableStorage.GetAllAsync<Archetype>($"archetypes{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    archetypes = (await _tableStorage.GetAllAsync<Archetype>($"archetypes{Language.en}")).ToList();
                    return Ok(archetypes);
                }
                throw;
            }
            return Ok(archetypes);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<PagedSearchResult<Archetype>>>> Get(ArchetypeSearch archetypeSearch, Language language = Language.en)
        {
            var archetypes = await _archetypeManager.SearchArchetypes(archetypeSearch, language);

            return Ok(archetypes);
        }

        //[HttpPost]
        //public async Task Post([FromBody] Archetype archetype)
        //{
        //    await _tableStorage.AddOrUpdateAsync("archetypes", archetype);
        //}

        //[HttpDelete("{name}")]
        //public async Task Delete(string name)
        //{
        //    var query = new TableQuery<Archetype>();
        //    query.Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, name));

        //    var archetypes = await _tableStorage.QueryAsync("archetypes", query);
        //    foreach (var archetype in archetypes)
        //    {
        //        await _tableStorage.DeleteAsync("archetypes", archetype);
        //    }
        //}
    }
}
