using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Starship;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Parsers.SOTG
{
    public class StarshipModificationProcessor : BaseProcessor<StarshipModification>
    {
        public override Task<List<StarshipModification>> FindBlocks(List<string> lines)
        {
            var modifications = new List<StarshipModification>();

            var engineeringLinesStart = lines.FindIndex(f => f.StartsWith("## Engineering Systems"));
            var operationLinesStart = lines.FindIndex(f =>
                f.StartsWith("## Operation Systems", StringComparison.InvariantCultureIgnoreCase));
            var suiteLinesStart = lines.FindIndex(f => f.StartsWith("## Suite Systems"));
            var universalLinesStart = lines.FindIndex(f => f.StartsWith("## Universal Systems"));
            var weaponLinesStart = lines.FindIndex(f => f.StartsWith("## Weapon Systems"));

            var engineeringSystemsLines = lines.Skip(engineeringLinesStart)
                .Take(operationLinesStart - engineeringLinesStart).ToList();

            var operationSystemsLines = lines.Skip(operationLinesStart)
                .Take(suiteLinesStart - operationLinesStart).ToList();

            var suiteSystemsLines = lines.Skip(suiteLinesStart)
                .Take(universalLinesStart - suiteLinesStart).ToList();

            var universalSystemsLines = lines.Skip(universalLinesStart)
                .Take(weaponLinesStart - universalLinesStart).ToList();

            var weaponSystemsLines = lines.Skip(weaponLinesStart)
                .Take(lines.Count - weaponLinesStart).ToList();

            modifications.AddRange(CreateModifications(engineeringSystemsLines, StarshipModificationType.Engineering));
            modifications.AddRange(CreateModifications(operationSystemsLines, StarshipModificationType.Operation));
            modifications.AddRange(CreateModifications(suiteSystemsLines, StarshipModificationType.Suite));
            modifications.AddRange(CreateModifications(universalSystemsLines, StarshipModificationType.Universal));
            modifications.AddRange(CreateModifications(weaponSystemsLines, StarshipModificationType.Weapon));

            return Task.FromResult(modifications);
        }

        private static IEnumerable<StarshipModification> CreateModifications(List<string> systemLines, StarshipModificationType type)
        {
            var modifications = new List<StarshipModification>();
            for (var i = 0; i < systemLines.Count; i++)
            {
                if (!systemLines[i].StartsWith("### ")) continue;

                var endIndex = systemLines.FindIndex(i + 1, x => x.StartsWith("### ", StringComparison.InvariantCultureIgnoreCase));
                var modificationLines = systemLines.Skip(i).Take((endIndex == -1 ? systemLines.Count - 1 : endIndex) - i).ToList().CleanListOfStrings();

                var modification = new StarshipModification
                {
                    TypeEnum = type,
                    PartitionKey = ContentType.Core.ToString(),
                    RowKey = systemLines[i].Substring(systemLines[i].IndexOf(' ') + 1),
                    Name = systemLines[i].Substring(systemLines[i].IndexOf(' ') + 1),
                    Prerequisites = modificationLines.Where(s => s.StartsWith("_prerequisite",
                            StringComparison.InvariantCultureIgnoreCase)).Select(s =>
                            s.Substring(s.IndexOf(' ') + 1).Replace("_", string.Empty).Replace("<br>", string.Empty))
                        .ToList(),
                    Content = string.Join("\r\n",
                        modificationLines.Where(s =>
                            !string.IsNullOrWhiteSpace(s) &&
                            !s.StartsWith('/') &&
                            !s.StartsWith('<') &&
                            !s.StartsWith('#') &&
                            !s.StartsWith("_Prerequisite", StringComparison.InvariantCultureIgnoreCase)))
                };

                modifications.Add(modification);
            }
            return modifications;
        }
    }
}
