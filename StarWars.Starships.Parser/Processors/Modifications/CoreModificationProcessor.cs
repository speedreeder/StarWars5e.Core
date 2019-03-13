using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.ViewModels;

namespace StarWars.Starships.Parser.Processors.Modifications
{
    public class CoreModificationProcessor : StarshipBaseProcessor<Modification>
    {
        public CoreModificationProcessor()
        {
            
        }


        public override List<Modification> FindBlocks(List<string> lines)
        {
            var modifications = new List<Modification>();

            return modifications;
        }
    }
}
