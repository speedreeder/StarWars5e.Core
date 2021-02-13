using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Api.Storage;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Equipment;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Controllers
{
    [Route("api/equipment")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly IEquipmentManager _equipmentManager;

        public EquipmentController(IAzureTableStorage tableStorage, IEquipmentManager equipmentManager)
        {
            _tableStorage = tableStorage;
            _equipmentManager = equipmentManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipment>>> Get(Language language = Language.en)
        {
            List<Equipment> equipment;
            try
            {
                equipment = (await _tableStorage.GetAllAsync<Equipment>($"equipment{language}")).ToList();
            }
            catch (StorageException e)
            {
                if (e.Message == "Not Found")
                {
                    equipment = (await _tableStorage.GetAllAsync<Equipment>($"equipment{Language.en}")).ToList();
                    return Ok(equipment);
                }
                throw;
            }
            return Ok(equipment);
        }

        [HttpGet("search")]
        public async Task<ActionResult<Equipment>> Get([FromQuery] EquipmentSearch equipmentSearch)
        {
            var equipment = await _equipmentManager.SearchEquipment(equipmentSearch);

            return Ok(equipment);
        }

        //[HttpPost]
        //public async Task Post([FromBody] Equipment equipment)
        //{
        //    await _tableStorage.AddOrUpdateAsync("equipment", equipment);
        //}

        //[HttpDelete("{name}")]
        //public async Task Delete(string name)
        //{
        //    var query = new TableQuery<Equipment>();
        //    query.Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, name));

        //    var equipment = await _tableStorage.QueryAsync("equipment", query);
        //    foreach (var equipmentPiece in equipment)
        //    {
        //        await _tableStorage.DeleteAsync("equipment", equipmentPiece);
        //    }
        //}
    }
}
