using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Models.Monster;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class MonsterController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;

        public MonsterController(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Monster>>> Get()
        {
            var monsters = await _tableStorage.GetAllAsync<Monster>("monsters");
            return Ok(monsters);
        }

        //[HttpPost]
        //public void Post([FromBody] Monster monster)
        //{
        //}

        //[HttpPut("{name}")]
        //public void Put(string name, [FromBody] Monster monster)
        //{
        //}

        //[HttpDelete("{name}")]
        //public void Delete(string name)
        //{
        //}
    }
}
