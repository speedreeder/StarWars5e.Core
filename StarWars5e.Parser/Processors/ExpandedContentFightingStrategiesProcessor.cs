using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;
using StarWars5e.Parser.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars5e.Parser.Processors
{
    public class ExpandedContentFightingStrategiesProcessor : BaseProcessor<FightingStrategy>
    {
        public ExpandedContentFightingStrategiesProcessor(ILocalization localization)
        {
            Localization = localization;
        }

        public override Task<List<FightingStrategy>> FindBlocks(List<string> lines, ContentType contentType)
        {
            var strategies = new List<FightingStrategy>();
            lines = lines.CleanListOfStrings().ToList();
            var strategyStartLines = lines.Where(f => f.StartsWith($"#### ") && f.EndsWith($"Strategist"));

            foreach (var strategyStartLine in strategyStartLines)
            {
                var strategyStartIndex = lines.IndexOf(strategyStartLine);

                var strategyEndIndex = lines.FindIndex(strategyStartIndex + 1, f => f.StartsWith("#"));
                var strategyLines = lines.Skip(strategyStartIndex);

                if (strategyEndIndex != -1)
                {
                    strategyLines = lines.Skip(strategyStartIndex).Take(strategyEndIndex - strategyStartIndex);
                }

                strategies.Add(ParseStrategy(strategyLines.CleanListOfStrings().ToList(), contentType));
            }

            return Task.FromResult(strategies);
        }

        public FightingStrategy ParseStrategy(List<string> strategyLines, ContentType contentType)
        {
            var name = strategyLines[0].Split("####")[1].Trim();

            try
            {
                var strategy = new FightingStrategy
                {
                    RowKey = name.FormatKey(),
                    Name = name,
                    PartitionKey = contentType.ToString(),
                    ContentTypeEnum = contentType
                };

                strategy.Text = string.Join(Environment.NewLine, strategyLines.Skip(1));

                return strategy;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing fighting strategy {name}", e);
            }
        }
    }
}
