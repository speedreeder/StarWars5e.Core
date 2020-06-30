using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Processors
{
    public class ExpandedContentCustomizationOptionsFightingStyleProcessor : BaseProcessor<FightingStyle>
    {
        public override Task<List<FightingStyle>> FindBlocks(List<string> lines)
        {
            var fightingStyles = new List<FightingStyle>();
            lines = lines.CleanListOfStrings().ToList();


            var fightingStylesStart = lines.FindIndex(f => f.StartsWith("## Fighting Styles"));
            var fightingStylesEndIndex = lines.FindIndex(fightingStylesStart + 1, f => f.StartsWith("## "));

            for (var i = fightingStylesStart; i < (fightingStylesEndIndex != -1 ? fightingStylesEndIndex : lines.Count); i++)
            {
                if (!lines[i].StartsWith("#### ")) continue;

                var fightingStyleStartIndex = i;
                var fightingStyleEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("#### "));

                var fightingStyleLines = lines.Skip(fightingStyleStartIndex);
                if (fightingStyleEndIndex != -1)
                {
                    fightingStyleLines = lines.Skip(fightingStyleStartIndex).Take(fightingStyleEndIndex - fightingStyleStartIndex);
                }

                var fightingStyle = ParseFightingStyle(fightingStyleLines.ToList(), ContentType.ExpandedContent);
                fightingStyles.Add(fightingStyle);
            }

            return Task.FromResult(fightingStyles);
        }

        public FightingStyle ParseFightingStyle(List<string> fightingStyleLines, ContentType contentType)
        {
            var name = fightingStyleLines[0].Split("####")[1].Trim();
            try
            {
                var fightingStyle = new FightingStyle
                {
                    ContentTypeEnum = contentType,
                    Name = name,
                    PartitionKey = contentType.ToString(),
                    RowKey = name,
                    Description = string.Join("\r\n", fightingStyleLines.Skip(1).ToList())
                };

                return fightingStyle;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing fighting style {name}", e);
            }
        }
    }
}
