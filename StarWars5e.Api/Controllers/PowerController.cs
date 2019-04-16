using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Models;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PowerController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;

        public PowerController(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Power>>> Get()
        {
            var powers = await _tableStorage.GetAllAsync<Power>("powers");
            return Ok(powers);
        }

        [HttpPost]
        public void Post([FromBody] Power power)
        {
        }

        [HttpPut("{name}")]
        public void Put(string name, [FromBody] Power power)
        {
        }

        [HttpDelete("{name}")]
        public void Delete(string name)
        {
        }
    }
}
