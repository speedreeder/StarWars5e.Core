using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Models.Background;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackgroundController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;

        public BackgroundController(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Background>>> Get()
        {
            var backgrounds = await _tableStorage.GetAllAsync<Background>("backgrounds");
            return Ok(backgrounds);
        }

        [HttpPost]
        public void Post([FromBody] Background background)
        {
        }

        [HttpPut("{name}")]
        public void Put(string name, [FromBody] Background background)
        {
        }

        [HttpDelete("{name}")]
        public void Delete(string name)
        {
        }
    }
}
