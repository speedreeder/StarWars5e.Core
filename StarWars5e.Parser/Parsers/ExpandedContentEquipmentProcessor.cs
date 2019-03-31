using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StarWars5e.Models.Equipment;

namespace StarWars5e.Parser.Parsers
{
    public class ExpandedContentEquipmentProcessor : BaseProcessor<Equipment>
    {
        public override Task<List<Equipment>> FindBlocks(List<string> lines)
        {
            throw new NotImplementedException();
        }
    }
}
