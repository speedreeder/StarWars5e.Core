using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Models.Class;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;

        public ClassController(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> Get()
        {
            var starWarsClasses = await _tableStorage.GetAllAsync<Class>("classes");
            return Ok(starWarsClasses);
        }

        [HttpPost]
        public async Task Post([FromBody] Class starWarsClass)
        {
            await _tableStorage.AddOrUpdateAsync("classes", starWarsClass);
        }

        [HttpDelete("{name}")]
        public async Task Delete(string name)
        {
            var query = new TableQuery<Class>();
            query.Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, name));

            var starWarsClass = await _tableStorage.QueryAsync("classes", query);
            foreach (var @class in starWarsClass)
            {
                await _tableStorage.DeleteAsync("classes", @class);
            }
        }
    }
}
