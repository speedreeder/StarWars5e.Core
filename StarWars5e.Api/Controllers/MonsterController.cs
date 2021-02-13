using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Api.Storage;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Monster;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonsterController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public MonsterController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Monster>>> Get(Language language = Language.en)
        {
            List<Monster> monsters;
            try
            {
                monsters = (await _tableStorage.GetAllAsync<Monster>($"monsters{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    monsters = (await _tableStorage.GetAllAsync<Monster>($"monsters{Language.en}")).ToList();
                    return Ok(monsters);
                }
                throw;
            }
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
