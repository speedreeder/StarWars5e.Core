using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Processors
{
    public class ExpandedContentCustomizationOptionsFightingMasteryProcessor : BaseProcessor<FightingMastery>
    {
        public override Task<List<FightingMastery>> FindBlocks(List<string> lines, ContentType contentType)
        {
            var fightingMasteries = new List<FightingMastery>();
            lines = lines.CleanListOfStrings().ToList();

            var fightingMasteriesStart = lines.FindIndex(f => f.StartsWith("## Fighting Masteries"));
            var fightingMasteriesTempEndIndex = lines.FindIndex(fightingMasteriesStart + 1, f => f.StartsWith("## "));
            var fightingMasteriesEndIndex = fightingMasteriesTempEndIndex != -1 ? fightingMasteriesTempEndIndex : lines.Count;

            for (var i = fightingMasteriesStart; i < fightingMasteriesEndIndex; i++)
            {
                if (!lines[i].StartsWith("#### ")) continue;

                var fightingMasteryStartIndex = i;
                var fightingMasteryEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("#### "));

                var fightingMasteryLines = lines.Skip(fightingMasteryStartIndex).Take(fightingMasteriesEndIndex - fightingMasteryStartIndex);
                if (fightingMasteryEndIndex != -1 && fightingMasteryEndIndex < fightingMasteriesEndIndex)
                {
                    fightingMasteryLines = lines.Skip(fightingMasteryStartIndex).Take(fightingMasteryEndIndex - fightingMasteryStartIndex);
                }

                var fightingMastery = ParseFightingMastery(fightingMasteryLines.ToList(), contentType);
                fightingMasteries.Add(fightingMastery);
            }

            return Task.FromResult(fightingMasteries);
        }

        public FightingMastery ParseFightingMastery(List<string> fightingMasteryLines, ContentType contentType)
        {
            var name = fightingMasteryLines[0].Split("####")[1].Trim();
            try
            {
                var fightingMastery = new FightingMastery
                {
                    ContentTypeEnum = contentType,
                    Name = name,
                    PartitionKey = contentType.ToString(),
                    RowKey = name,
                    Text = string.Join("\r\n", fightingMasteryLines.Skip(1).ToList())
                };

                return fightingMastery;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing fighting mastery {name}", e);
            }
        } 
    }
}
