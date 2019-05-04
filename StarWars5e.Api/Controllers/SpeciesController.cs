using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Search;
using StarWars5e.Models.Species;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Controllers
{
    [Route("api/species")]
    [ApiController]
    public class SpeciesController : ControllerBase
    {
        private readonly ITableStorage _tableStorage;
        private readonly ISpeciesManager _speciesManager;

        public SpeciesController(ITableStorage tableStorage, ISpeciesManager speciesManager)
        {
            _tableStorage = tableStorage;
            _speciesManager = speciesManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Species>>> Get()
        {
            var species = await _tableStorage.GetAllAsync<Species>("species");
            return Ok(species);
        }

        [HttpGet("search")]
        public async Task<ActionResult<Species>> Get([FromQuery] SpeciesSearch speciesSearch)
        {
            var species = await _speciesManager.SearchSpecies(speciesSearch);

            return Ok(species);
        }

        //[HttpPost]
        //public async Task Post([FromBody] Species species)
        //{
        //    await _tableStorage.AddOrUpdateAsync("species", species);
        //}

        //[HttpDelete("{name}")]
        //public async Task Delete(string name)
        //{
        //    var query = new TableQuery<Species>();
        //    query.Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, name));

        //    var species = await _tableStorage.QueryAsync("species", query);
        //    foreach (var oneSpecies in species)
        //    {
        //        await _tableStorage.DeleteAsync("species", oneSpecies);
        //    }
        //}
    }
}
