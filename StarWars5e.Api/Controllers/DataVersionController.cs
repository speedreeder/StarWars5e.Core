using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Storage;
using StarWars5e.Models;
using StarWars5e.Models.Enums;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataVersionController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;

        public DataVersionController(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataVersion>>> Get()
        {
            var dataVersions = await _tableStorage.GetAllAsync<DataVersion>("dataVersion");
            return Ok(dataVersions);
        }

        [HttpGet]
        [Route("master")]
        public async Task<ActionResult<IEnumerable<DataVersion>>> GetMasterVersion()
        {
            var dataVersion = await _tableStorage.GetAsync<DataVersion>("dataVersion", ContentType.Core.ToString(), "MASTERVERSION");
            return Ok(dataVersion);
        }
    }
}
