using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Processors
{
    public class ReferenceTableProcessor : BaseProcessor<ReferenceTable>
    {
        public override Task<List<ReferenceTable>> FindBlocks(List<string> lines)
        {
            var referenceTables = new List<ReferenceTable>();

            referenceTables.Add(ParseTable(lines, "##### Ability Score Point Cost", "Ability Score Point Cost", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "##### Ability Scores and Modifiers", "Ability Scores and Modifiers", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "|Experience Points|Level|Proficiency Bonus|", "XP and PB by Level", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Variant: Starting Wealth by Class", "Variant: Starting Wealth by Class", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "##### Lifestyle Expenses", "Lifestyle Expenses", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Multiclassing Prerequisites", "Multiclassing Prerequisites", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "### Multiclassing Proficiencies", "Multiclassing Proficiencies", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Stock Cost", "Starship Size Stock Cost", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Construction Workforce", "Starship Size Construction Workforce", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Base Upgrade Cost by Tier", "Base Upgrade Cost by Tier", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Upgrade Cost", "Starship Size Upgrade Cost", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Upgrade Workforce", "Starship Size Upgrade Workforce", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Modification Category Base Cost", "Modification Category Base Cost", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Modification Cost", "Starship Size Modification Cost", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Modification Workforce", "Starship Size Modification Workforce", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Modification Tier Requirement DC", "Modification Tier Requirement DC", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "### Modification Slots at  Tier 0", "Modification Slots at  Tier 0", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Equipment Cost", "Starship Size Equipment Cost", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Equipment Workforce", "Starship Size Equipment Workforce", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Cargo Capacity", "Starship Size Cargo Capacity", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Base Armor Class", "Starship Size Base Armor Class", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Fuel Cost", "Starship Size Fuel Cost", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Fuel Capacity", "Starship Size Fuel Capacity", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Food Capacity", "Starship Size Food Capacity", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Base Flying Speed", "Starship Size Base Flying Speed", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Base Turning Speed", "Starship Size Base Turning Speed", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Sample Realspace Travel Times", "Sample Realspace Travel Times", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Sample Hyperspace Travel Times", "Sample Hyperspace Travel Times", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Astrogation Time Taken", "Astrogation Time Taken", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Hyperspace Mishaps", "Hyperspace Mishaps", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Minimum Crew", "Starship Size Minimum Crew", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Repair Time", "Starship Size Repair Time", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Maintenance Time", "Starship Size Maintenance Time", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Size Categories", "Starship Size Categories", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### System Damage", "System Damage", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Maximum Suites", "Starship Size Maximum Suites", ContentType.Core, 2));
            referenceTables.Add(ParseTable(lines, "#### Starship Size Suite Capacity", "Starship Size Suite Capacity", ContentType.Core));
            referenceTables.Add(ParseTable(lines, "#### Cybernetic Augmentation Side Effects", "Cybernetic Augmentation Side Effects", ContentType.Core));

            return Task.FromResult(referenceTables);
        }

        private static ReferenceTable ParseTable(List<string> lines, string startLine, string name, ContentType contentType, int occurence = 1)
        {
            try
            {
                var referenceTableStart = lines.FindNthIndex(f => f.RemoveHtmlWhitespace().StartsWith(startLine), occurence);
                var referenceTableEnd =
                    lines.FindIndex(referenceTableStart + 3, string.IsNullOrWhiteSpace);
                var referenceTableLines = lines.Skip(referenceTableStart)
                    .Take(referenceTableEnd - referenceTableStart).Where(r => !r.StartsWith("#")).CleanListOfStrings()
                    .RemoveEmptyLines();

                return new ReferenceTable(name, string.Join("\r\n", referenceTableLines), contentType);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing reference table {name}", e);
            }
        }
    }
}
