using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Starship;

namespace StarWars5e.Starships.Parser.Processors
{
    public class StarshipModificationProcessor : StarshipBaseProcessor<StarshipModification>
    {
        public override async Task<List<StarshipModification>> FindBlocks(List<string> lines)
        {
            var modifications = new List<StarshipModification>();

            var chapter4StartIndex = lines.FindIndex(f => f == "# Chapter 4: Modifications");
            var chapter5StartIndex = lines.FindIndex(f => f == "# Chapter 5: Equipment");
            var modificationLines = lines.Skip(chapter4StartIndex).Take(chapter5StartIndex - chapter4StartIndex).ToList();

            var engineeringSystemsLines = new List<string>();
            var operationSystemsLines = new List<string>();
            var suiteSystemsLines = new List<string>();
            var universalSystemsLines = new List<string>();
            var weaponSystemsLines = new List<string>();
            for (var i = 0; i < modificationLines.Count; i++)
            {
                if (modificationLines[i].StartsWith("## Engineering Systems", StringComparison.InvariantCultureIgnoreCase))
                {
                    var endIndex = modificationLines.FindIndex(i + 1, x => x.StartsWith("## ", StringComparison.InvariantCultureIgnoreCase));
                    engineeringSystemsLines = modificationLines.Skip(i).Take((endIndex == -1 ? modificationLines.Count - 1 : endIndex) - i).ToList();
                }
                else if (modificationLines[i].StartsWith("## Operation Systems", StringComparison.InvariantCultureIgnoreCase))
                {
                    var endIndex = modificationLines.FindIndex(i + 1, x => x.StartsWith("## ", StringComparison.InvariantCultureIgnoreCase));
                    operationSystemsLines = modificationLines.Skip(i).Take(i - endIndex == -1 ? modificationLines.Count - 1 : endIndex).ToList();
                }
                else if (modificationLines[i].StartsWith("## Suite Systems", StringComparison.InvariantCultureIgnoreCase))
                {
                    var endIndex = modificationLines.FindIndex(i + 1, x => x.StartsWith("## ", StringComparison.InvariantCultureIgnoreCase));
                    suiteSystemsLines = modificationLines.Skip(i).Take(i - endIndex == -1 ? modificationLines.Count - 1 : endIndex).ToList();
                }
                else if (modificationLines[i].StartsWith("## Universal Systems", StringComparison.InvariantCultureIgnoreCase))
                {
                    var endIndex = modificationLines.FindIndex(i + 1, x => x.StartsWith("## ", StringComparison.InvariantCultureIgnoreCase));
                    universalSystemsLines = modificationLines.Skip(i).Take(i - endIndex == -1 ? modificationLines.Count - 1 : endIndex).ToList();
                }
                else if (modificationLines[i].StartsWith("## Weapon Systems", StringComparison.InvariantCultureIgnoreCase))
                {
                    var endIndex = modificationLines.FindIndex(i + 1, x => x.StartsWith("## ", StringComparison.InvariantCultureIgnoreCase));
                    weaponSystemsLines = modificationLines.Skip(i).Take(i - endIndex == -1 ? modificationLines.Count - 1 : endIndex).ToList();
                }
            }

            modifications.AddRange(await CreateModifications(engineeringSystemsLines, ModificationType.Engineering));
            modifications.AddRange(await CreateModifications(operationSystemsLines, ModificationType.Operation));
            modifications.AddRange(await CreateModifications(suiteSystemsLines, ModificationType.Suite));
            modifications.AddRange(await CreateModifications(universalSystemsLines, ModificationType.Universal));
            modifications.AddRange(await CreateModifications(weaponSystemsLines, ModificationType.Weapon));

            return modifications;
        }

        private static Task<List<StarshipModification>> CreateModifications(List<string> systemLines, ModificationType type)
        {
            var modifications = new List<StarshipModification>();
            for (var i = 0; i < systemLines.Count; i++)
            {
                var modification = new StarshipModification(type);

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
