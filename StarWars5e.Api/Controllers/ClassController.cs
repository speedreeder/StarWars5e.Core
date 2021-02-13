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
    [Route("api/class")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly IClassManager _classManager;

        public ClassController(IAzureTableStorage tableStorage, IClassManager classManager)
        {
            _tableStorage = tableStorage;
            _classManager = classManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> Get(Language language = Language.en)
        {
            List<Class> starWarsClasses;
            try
            {
                starWarsClasses = (await _tableStorage.GetAllAsync<Class>($"classes{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    starWarsClasses = (await _tableStorage.GetAllAsync<Class>($"classes{Language.en}")).ToList();
                    return Ok(starWarsClasses);
                }
                throw;
            }
            return Ok(starWarsClasses);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<PagedSearchResult<Class>>>> Get([FromQuery] ClassSearch classSearch)
        {
            var classes = await _classManager.SearchClasses(classSearch);
            return Ok(classes);
        }

        //[HttpPost]
        //public async Task Post([FromBody] Class starWarsClass)
        //{
        //    await _tableStorage.AddOrUpdateAsync("classes", starWarsClass);
        //}

        //[HttpDelete("{name}")]
        //public async Task Delete(string name)
        //{
        //    var query = new TableQuery<Class>();
        //    query.Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, name));

        //    var starWarsClass = await _tableStorage.QueryAsync("classes", query);
        //    foreach (var @class in starWarsClass)
        //    {
        //        await _tableStorage.DeleteAsync("classes", @class);
        //    }
        //}
    }
}
