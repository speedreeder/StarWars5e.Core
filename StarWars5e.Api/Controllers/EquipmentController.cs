using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Equipment;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;
        private readonly IEquipmentManager _equipmentManager;

        public EquipmentController(ITableStorage tableStorage, IEquipmentManager equipmentManager)
        {
            _tableStorage = tableStorage;
            _equipmentManager = equipmentManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipment>>> Get()
        {
            var equipment = await _tableStorage.GetAllAsync<Equipment>("equipment");
            return Ok(equipment);
        }

        [HttpGet("/search2")]
        public async Task<ActionResult<Equipment>> Get([FromQuery] EquipmentSearch equipmentSearch)
        {
            var equipment = await _equipmentManager.SearchEquipment(equipmentSearch);
            if(!equipment.Any()) return NotFound();

            return Ok(equipment);
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
