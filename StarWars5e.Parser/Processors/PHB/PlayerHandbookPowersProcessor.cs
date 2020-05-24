using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Processors.PHB
{
    public class PlayerHandbookPowersProcessor : BaseProcessor<Power>
    {
        public override Task<List<Power>> FindBlocks(List<string> lines)
        {
            var powers = new List<Power>();
            var powerStartLines = lines.Where(f =>
                f.StartsWith("#### ") && !Regex.IsMatch(f, @"^\#\#\#\#\s+\d+") &&
                !Regex.IsMatch(f, Localization.PHBPowerStartLinesPattern)).ToList();

            var stunIndex = 1;
            foreach (var powerLine in powerStartLines)
            {
                var rowKeyOverride = "";
                var powerStartIndex = lines.IndexOf(powerLine);
                if (powerLine == $"#### {Localization.Stun}" && stunIndex == 2)
                {
                    powerStartIndex = lines.FindNthIndex(f => f == powerLine, 2);
                    rowKeyOverride = $"{Localization.Stun} 2";
                }
                
                var powerEndIndex = lines.FindIndex(powerStartIndex + 1, f => f.StartsWith("#### ") || f.StartsWith("# "));
                var powerLines = lines.Skip(powerStartIndex);

                if (powerEndIndex != -1)
                {
                    powerLines = lines.Skip(powerStartIndex).Take(powerEndIndex - powerStartIndex);
                }

                powers.Add(ParsePower(powerLines.CleanListOfStrings().ToList(), ContentType.Core, rowKeyOverride));

                if (powerLine == $"#### {Localization.Stun}")
                {
                    stunIndex++;
                }
            }

            return Task.FromResult(powers);
        }

        public Power ParsePower(List<string> powerLines, ContentType contentType, string rowKeyOverride)
        {
            var name = powerLines[0].Split("####")[1].Trim();

            try
            {
                var power = new Power
                {
                    RowKey = !string.IsNullOrWhiteSpace(rowKeyOverride) ? rowKeyOverride : name.FormatKey(),
                    Name = name,
                    PartitionKey = contentType.ToString(),
                    ContentTypeEnum = contentType
                };

                var levelMatch = Regex.Match(powerLines[1], @"\d+");
                power.Level = levelMatch.Success ? int.Parse(levelMatch.Value) : 0;

                if (powerLines[1].Contains(Localization.universal, StringComparison.InvariantCultureIgnoreCase))
                {
                    power.ForceAlignmentEnum = ForceAlignment.Universal;
                    power.PowerTypeEnum = PowerType.Force;
                }
                else if (powerLines[1].Contains(Localization.dark, StringComparison.InvariantCultureIgnoreCase))
                {
                    power.ForceAlignmentEnum = ForceAlignment.Dark;
                    power.PowerTypeEnum = PowerType.Force;
                }
                else if (powerLines[1].Contains(Localization.light, StringComparison.InvariantCultureIgnoreCase))
                {
                    power.ForceAlignmentEnum = ForceAlignment.Light;
                    power.PowerTypeEnum = PowerType.Force;
                }

                if (powerLines[1].Contains(Localization.tech, StringComparison.InvariantCultureIgnoreCase))
                {
                    power.ForceAlignmentEnum = ForceAlignment.None;
                    power.PowerTypeEnum = PowerType.Tech;
                }

                power.Prerequisite = powerLines.Find(f => f.Contains($"**{Localization.Prerequisite}"))?.Split("**").ElementAtOrDefault(2)?.Trim();
                power.Range = powerLines.Find(f => f.Contains($"**{Localization.Range}"))?.Split("**").ElementAtOrDefault(2)?.Trim();

                var durationSplit = powerLines.Find(f => f.Contains($"**{Localization.Duration}"))?.Split("**").ElementAtOrDefault(2)?.Trim().Split(',');
                if (durationSplit != null)
                {
                    power.Duration = durationSplit.ElementAtOrDefault(1)?.Trim() ?? durationSplit.ElementAtOrDefault(0)?.Trim();
                    power.Concentration = durationSplit[0].Trim()
                        .Contains(Localization.Concentration, StringComparison.InvariantCultureIgnoreCase);
                }

                var castingTime = powerLines.Find(f => f.Contains($"**{Localization.CastingTime}", StringComparison.InvariantCultureIgnoreCase));
                if (castingTime != null)
                {
                    power.CastingPeriodText = castingTime.Split("**").ElementAtOrDefault(2)?.Trim();
                }

                if (castingTime != null && castingTime.Contains(Localization.bonus, StringComparison.InvariantCultureIgnoreCase))
                {
                    power.CastingPeriodEnum = CastingPeriod.BonusAction;
                }
                else if (castingTime != null &&
                         castingTime.Contains(Localization.reaction, StringComparison.InvariantCultureIgnoreCase))
                {
                    power.CastingPeriodEnum = CastingPeriod.Reaction;
                }
                else if (castingTime != null &&
                         castingTime.Contains(Localization.action, StringComparison.InvariantCultureIgnoreCase))
                {
                    power.CastingPeriodEnum = CastingPeriod.Action;
                }
                else if (castingTime != null &&
                         castingTime.Contains(Localization.minute, StringComparison.InvariantCultureIgnoreCase))
                {
                    power.CastingPeriodEnum = CastingPeriod.Minute;
                }
                else if (castingTime != null &&
                         castingTime.Contains(Localization.hour, StringComparison.InvariantCultureIgnoreCase))
                {
                    power.CastingPeriodEnum = CastingPeriod.Hour;
                }

                var powerDescriptionStart = powerLines.FindIndex(f => Regex.IsMatch(f, @"^\s$") || string.IsNullOrWhiteSpace(f));

                var descriptionLines = powerLines.Skip(powerDescriptionStart + 1);
                
                power.Description = string.Join("\r\n", descriptionLines);

                return power;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing {name}", e);
            }
        }
    }
}
