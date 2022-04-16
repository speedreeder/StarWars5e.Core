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
    public class ExpandedContentManeuversProcessor : BaseProcessor<Maneuver>
    {
        public ExpandedContentManeuversProcessor(ILocalization localization)
        {
            Localization = localization;
        }

        public override Task<List<Maneuver>> FindBlocks(List<string> lines, ContentType contentType)
        {
            var maneuvers = new List<Maneuver>();
            lines = lines.CleanListOfStrings().ToList();
            var maneuverStartLines = lines.Where(f => f.StartsWith($"#### ") &&
                !f.StartsWith("#### General") &&
                !f.StartsWith("#### Physical") &&
                !f.StartsWith("#### Mental"));

            foreach (var maneuverStartLine in maneuverStartLines)
            {
                var maneuverStartIndex = lines.IndexOf(maneuverStartLine);

                var maneuverEndIndex = lines.FindIndex(maneuverStartIndex + 1, f => f.StartsWith("#"));
                var maneuverLines = lines.Skip(maneuverStartIndex);

                if (maneuverEndIndex != -1)
                {
                    maneuverLines = lines.Skip(maneuverStartIndex).Take(maneuverEndIndex - maneuverStartIndex);
                }

                maneuvers.Add(ParseManeuver(maneuverLines.CleanListOfStrings().ToList(), contentType));
            }

            return Task.FromResult(maneuvers);
        }

        public Maneuver ParseManeuver(List<string> maneuverLines, ContentType contentType)
        {
            var name = maneuverLines[0].Split("####")[1].Trim();

            try
            {
                var maneuver = new Maneuver
                {
                    RowKey = name.FormatKey(),
                    Name = name,
                    PartitionKey = contentType.ToString(),
                    ContentTypeEnum = contentType
                };

                var prerequisiteLine =
                    maneuverLines.FirstOrDefault(m => m.StartsWith($"_**{Localization.Prerequisite}"));

                if (prerequisiteLine != null)
                {
                    maneuver.Prerequisite = prerequisiteLine.Split(':')[1].Trim().RemoveMarkdownCharacters()
                        .RemoveUnderscores().Trim();
                }
                    
                if (maneuverLines[1].Contains("general", StringComparison.OrdinalIgnoreCase))
                {
                    maneuver.Type = "General";
                } else if(maneuverLines[1].Contains("physical", StringComparison.OrdinalIgnoreCase))
                {
                    maneuver.Type = "Physical";
                }
                else if(maneuverLines[1].Contains("mental", StringComparison.OrdinalIgnoreCase))
                {
                    maneuver.Type = "Mental";
                }

                var descriptionLines = maneuverLines.Skip(2);

                if (!string.IsNullOrWhiteSpace(prerequisiteLine))
                {
                    descriptionLines = maneuverLines.Skip(3);
                }

                maneuver.Description = string.Join("\r\n", descriptionLines);

                return maneuver;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing maneuver {name}", e);
            }
        }
    }
}
