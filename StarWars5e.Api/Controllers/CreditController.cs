using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Models;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;

        public CreditController(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BaseEntity>>> Get()
        {
            var credits = await _tableStorage.GetAllAsync<BaseEntity>("credits");
            return Ok(credits);
        }
    }
}
