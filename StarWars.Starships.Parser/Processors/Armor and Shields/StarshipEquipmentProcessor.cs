using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StarWars5e.Models.Starship;

namespace StarWars.Starships.Parser.Processors
{
    public class StarshipEquipmentProcessor : StarshipBaseProcessor<StarshipEquipment>
    {
        public override Task<List<StarshipEquipment>> FindBlocks(List<string> lines)
        {
            throw new NotImplementedException();
        }
    }
}
