using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Starship;

namespace StarWars.Starships.Parser.Processors.Modifications
{
    public class ModificationProcessor : StarshipBaseProcessor<Modification>
    {
        public override async Task<List<Modification>> FindBlocks(List<string> lines)
        {
            var modifications = new List<Modification>();

            var engineeringSystemsLines = new List<string>();
            var operationSystemsLines = new List<string>();
            var suiteSystemsLines = new List<string>();
            var universalSystemsLines = new List<string>();
            var weaponSystemsLines = new List<string>();
            for (var i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("## Engineering Systems", StringComparison.InvariantCultureIgnoreCase))
                {
                    var endIndex = lines.FindIndex(i + 1, x => x.StartsWith("## ", StringComparison.InvariantCultureIgnoreCase));
                    engineeringSystemsLines = lines.Skip(i).Take((endIndex == -1 ? lines.Count - 1 : endIndex) - i).ToList();
                }
                else if (lines[i].StartsWith("## Operation Systems", StringComparison.InvariantCultureIgnoreCase))
                {
                    var endIndex = lines.FindIndex(i + 1, x => x.StartsWith("## ", StringComparison.InvariantCultureIgnoreCase));
                    operationSystemsLines = lines.Skip(i).Take(i - endIndex == -1 ? lines.Count - 1 : endIndex).ToList();
                }
                else if (lines[i].StartsWith("## Suite Systems", StringComparison.InvariantCultureIgnoreCase))
                {
                    var endIndex = lines.FindIndex(i + 1, x => x.StartsWith("## ", StringComparison.InvariantCultureIgnoreCase));
                    suiteSystemsLines = lines.Skip(i).Take(i - endIndex == -1 ? lines.Count - 1 : endIndex).ToList();
                }
                else if (lines[i].StartsWith("## Universal Systems", StringComparison.InvariantCultureIgnoreCase))
                {
                    var endIndex = lines.FindIndex(i + 1, x => x.StartsWith("## ", StringComparison.InvariantCultureIgnoreCase));
                    universalSystemsLines = lines.Skip(i).Take(i - endIndex == -1 ? lines.Count - 1 : endIndex).ToList();
                }
                else if (lines[i].StartsWith("## Weapon Systems", StringComparison.InvariantCultureIgnoreCase))
                {
                    var endIndex = lines.FindIndex(i + 1, x => x.StartsWith("## ", StringComparison.InvariantCultureIgnoreCase));
                    weaponSystemsLines = lines.Skip(i).Take(i - endIndex == -1 ? lines.Count - 1 : endIndex).ToList();
                }
            }

            modifications.AddRange(await CreateModifications(engineeringSystemsLines, ModificationType.Engineering));
            modifications.AddRange(await CreateModifications(operationSystemsLines, ModificationType.Operation));
            modifications.AddRange(await CreateModifications(suiteSystemsLines, ModificationType.Suite));
            modifications.AddRange(await CreateModifications(universalSystemsLines, ModificationType.Universal));
            modifications.AddRange(await CreateModifications(weaponSystemsLines, ModificationType.Weapon));

            return modifications;
        }

        private static Task<List<Modification>> CreateModifications(List<string> systemLines, ModificationType type)
        {
            var modifications = new List<Modification>();
            for (var i = 0; i < systemLines.Count; i++)
            {
                var modification = new Modification(type);

                if (!systemLines[i].StartsWith("### ")) continue;

                var endIndex = systemLines.FindIndex(i + 1, x => x.StartsWith("### ", StringComparison.InvariantCultureIgnoreCase));
                var modificationLines = systemLines.Skip(i).Take((endIndex == -1 ? systemLines.Count - 1 : endIndex) - i).ToList();
                modification.Name = systemLines[i].Substring(systemLines[i].IndexOf(' ') + 1);

                modification.Prerequisites =
                    modificationLines.Where(s => s.StartsWith("_prerequisite",
                        StringComparison.InvariantCultureIgnoreCase)).Select(s => s.Substring(s.IndexOf(' ') + 1).Replace("_", string.Empty).Replace("<br>", string.Empty)).ToList();

                modification.Content =
                    string.Join("\r\n",
                        modificationLines.Where(s =>
                            !string.IsNullOrWhiteSpace(s) &&
                            !s.StartsWith('/') &&
                            !s.StartsWith('<') &&
                            !s.StartsWith('#') &&
                            !s.StartsWith("_Prerequisite", StringComparison.InvariantCultureIgnoreCase)));

                modifications.Add(modification);
            }
            return Task.FromResult(modifications);
        }
    }
}
