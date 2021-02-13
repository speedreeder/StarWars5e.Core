using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
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
        public async Task<ActionResult<IEnumerable<DataVersion>>> Get(Language language = Language.en)
        {
            List<DataVersion> dataVersions;
            try
            {
                dataVersions = (await _tableStorage.GetAllAsync<DataVersion>($"dataVersion{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    dataVersions = (await _tableStorage.GetAllAsync<DataVersion>($"dataVersion{Language.en}")).ToList();
                    return Ok(dataVersions);
                }
                throw;
            }
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
