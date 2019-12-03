using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Models.Lookup;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/conditions")]
    [ApiController]
    public class ConditionsLUController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;
        public ConditionsLUController(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConditionLU>>> Get()
        {
            var conditionLus = await _tableStorage.GetAllAsync<ConditionLU>("conditionsLU");
            return Ok(conditionLus);
        }
    }
}
