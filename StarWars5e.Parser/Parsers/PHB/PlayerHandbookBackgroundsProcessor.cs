using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models.Background;

namespace StarWars5e.Parser.Parsers.PHB
{
    public class PlayerHandbookBackgroundsProcessor : BaseProcessor<Background>
    {
        public override Task<List<Background>> FindBlocks(List<string> lines)
        {
            throw new System.NotImplementedException();
        }
    }
}
