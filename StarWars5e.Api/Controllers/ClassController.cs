using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Class;
using StarWars5e.Models.Search;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/class")]
    [AllowAnonymous]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;
        private readonly IClassManager _classManager;

        public ClassController(ITableStorage tableStorage, IClassManager classManager)
        {
            _tableStorage = tableStorage;
            _classManager = classManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> Get()
        {
            var starWarsClasses = await _tableStorage.GetAllAsync<Class>("classes");
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
