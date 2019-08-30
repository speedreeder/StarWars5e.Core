using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;
using StarWars5e.Parser.Parsers.PHB;

namespace StarWars5e.Parser.Parsers
{
    public class ExpandedContentForcePowersProcessor : BaseProcessor<Power>
    {
        private readonly PlayerHandbookPowersProcessor _playerHandbookPowersProcessor;
        public ExpandedContentForcePowersProcessor()
        {
            _playerHandbookPowersProcessor = new PlayerHandbookPowersProcessor();
        }
        public override Task<List<Power>> FindBlocks(List<string> lines)
        {
            var powers = new List<Power>();
            var powerStartLines = lines.Where(f => f.StartsWith("#### ") && !Regex.IsMatch(f, @"^\#\#\#\#\s+\d+") && !Regex.IsMatch(f, @"^\#\#\#\#\s+At-Will")).ToList();
            foreach (var powerLine in powerStartLines)
            {
                var powerStartIndex = lines.IndexOf(powerLine);

                var powerEndIndex = lines.FindIndex(powerStartIndex + 1, f => f.StartsWith("#### "));
                var powerLines = lines.Skip(powerStartIndex);

                if (powerEndIndex != -1)
                {
                    powerLines = lines.Skip(powerStartIndex).Take(powerEndIndex - powerStartIndex);
                }

                powers.Add(_playerHandbookPowersProcessor.ParsePower(powerLines.CleanListOfStrings().ToList(), ContentType.Core, null));
            }

            return Task.FromResult(powers);
        }
    }
}
