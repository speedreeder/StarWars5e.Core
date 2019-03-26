using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Starship;

namespace StarWars5e.Parser.Parsers
{
    public class StarshipModificationProcessor : StarshipBaseProcessor<StarshipModification>
    {
        public override Task<List<StarshipModification>> FindBlocks(List<string> lines)
        {
            var modifications = new List<StarshipModification>();

            var chapter4StartIndex = lines.FindIndex(f => f == "# Chapter 4: Modifications");
            var chapter5StartIndex = lines.FindIndex(f => f == "# Chapter 5: Equipment");
            var modificationLines = lines.Skip(chapter4StartIndex).Take(chapter5StartIndex - chapter4StartIndex).ToList();

            var engineeringLinesStart = modificationLines.FindIndex(f => f.StartsWith("## Engineering Systems"));
            var operationLinesStart = modificationLines.FindIndex(f =>
                f.StartsWith("## Operation Systems", StringComparison.InvariantCultureIgnoreCase));
            var suiteLinesStart = modificationLines.FindIndex(f => f.StartsWith("## Suite Systems"));
            var universalLinesStart = modificationLines.FindIndex(f => f.StartsWith("## Universal Systems"));
            var weaponLinesStart = modificationLines.FindIndex(f => f.StartsWith("## Weapon Systems"));

            var engineeringSystemsLines = modificationLines.Skip(engineeringLinesStart)
                .Take(operationLinesStart - engineeringLinesStart).ToList();

            var operationSystemsLines = modificationLines.Skip(operationLinesStart)
                .Take(suiteLinesStart - operationLinesStart).ToList();

            var suiteSystemsLines = modificationLines.Skip(suiteLinesStart)
                .Take(universalLinesStart - suiteLinesStart).ToList();

            var universalSystemsLines = modificationLines.Skip(universalLinesStart)
                .Take(weaponLinesStart - universalLinesStart).ToList();

            var weaponSystemsLines = modificationLines.Skip(weaponLinesStart)
                .Take(modificationLines.Count - weaponLinesStart).ToList();

            modifications.AddRange(CreateModifications(engineeringSystemsLines, ModificationType.Engineering));
            modifications.AddRange(CreateModifications(operationSystemsLines, ModificationType.Operation));
            modifications.AddRange(CreateModifications(suiteSystemsLines, ModificationType.Suite));
            modifications.AddRange(CreateModifications(universalSystemsLines, ModificationType.Universal));
            modifications.AddRange(CreateModifications(weaponSystemsLines, ModificationType.Weapon));

            return Task.FromResult(modifications);
        }

        private static IEnumerable<StarshipModification> CreateModifications(List<string> systemLines, ModificationType type)
        {
            var modifications = new List<StarshipModification>();
            for (var i = 0; i < systemLines.Count; i++)
            {
                if (!systemLines[i].StartsWith("### ")) continue;

                var endIndex = systemLines.FindIndex(i + 1, x => x.StartsWith("### ", StringComparison.InvariantCultureIgnoreCase));
                var modificationLines = systemLines.Skip(i).Take((endIndex == -1 ? systemLines.Count - 1 : endIndex) - i).ToList();

                var modification = new StarshipModification
                {
                    TypeEnum = type,
                    PartitionKey = ContentType.Base.ToString(),
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
