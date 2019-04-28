using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Models;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;

        public FeatController(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feat>>> Get()
        {
            var feats = await _tableStorage.GetAllAsync<Feat>("feats");
            return Ok(feats);
        }

        [HttpPost]
        public void Post([FromBody] Feat feat)
        {
        }

        [HttpDelete("{name}")]
        public void Delete(string name)
        {
        }
    }
}
