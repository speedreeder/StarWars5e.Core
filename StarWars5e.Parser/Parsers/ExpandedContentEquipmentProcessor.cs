using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Equipment;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Parsers
{
    public class ExpandedContentEquipmentProcessor : BaseProcessor<Equipment>
    {
        public override async Task<List<Equipment>> FindBlocks(List<string> lines)
        {
            var equipment = new List<Equipment>();

            equipment.AddRange(await ParseWeapons(lines.CleanListOfStrings().ToList()));
            equipment.AddRange(await ParseAmmunition(lines.CleanListOfStrings().ToList()));
            equipment.AddRange(await ParseOtherEquipment(lines.CleanListOfStrings().ToList()));

            return equipment;
        }

        private Task<List<Weapon>> ParseWeapons(List<string> lines)
        {
            var equipmentList = new List<Weapon>();

            var tableStart = lines.FindIndex(f => f.Contains("##### Blasters - Expanded"));
            var tableEnd = lines.FindIndex(tableStart + 3, f => f == string.Empty);
            var tableLines = lines.Skip(tableStart + 3).Take(tableEnd - (tableStart + 3)).ToList();

            var weaponClassification = WeaponClassification.Unknown;
            foreach (var tableLine in tableLines)
            {
                var tableLineSplit = tableLine.Split('|');

                var costMatch = Regex.Match(tableLineSplit[2], @"(?<!\S)(\d*\.?\d+|\d{1,3}(,\d{3})*(\.\d+)?)(?!\S)");
                if (!tableLineSplit[1].Contains("_"))
                {
                    var weapon = new Weapon
                    {
                        ContentTypeEnum = ContentType.ExpandedContent,
                        PartitionKey = ContentType.ExpandedContent.ToString()
                    };

                    var weightMatch = Regex.Match(tableLineSplit[4], @"\d+");
                    weapon.Name = tableLineSplit[1].RemoveHtmlWhitespace().Trim();
                    weapon.RowKey = weapon.Name;
                    try
                    {
                        weapon.Weight = weightMatch.Success ? int.Parse(weightMatch.Value) : 0;
                        weapon.Properties = tableLineSplit[5].Split(',').Select(s => s.Trim().RemoveHtmlWhitespace())
                            .ToList();

                        var damageSplit = tableLineSplit[3].Replace("�", string.Empty).Trim().RemoveHtmlWhitespace().Split(' ');
                        var damageNumberMatches =
                            Regex.Matches(tableLineSplit[3].Trim().RemoveHtmlWhitespace(), @"\d+");
                        if (damageNumberMatches.Any())
                        {
                            weapon.DamageNumberOfDice = int.Parse(damageNumberMatches[0].Value);
                            weapon.DamageDiceDieTypeEnum = (DiceType)int.Parse(damageNumberMatches[1].Value);
                            weapon.DamageTypeEnum = Enum.Parse<DamageType>(damageSplit[1], true);
                        }

                        if (costMatch.Success)
                        {
                            weapon.Cost = int.Parse(costMatch.Value, NumberStyles.AllowThousands);
                            weapon.ClassificationEnum = weaponClassification;
                            weapon.EquipmentCategoryEnum = EquipmentCategory.Weapon;

                            var weaponDescriptionStartLine =
                                lines.FindIndex(f => Regex.IsMatch(f, $@"####\s+{weapon.Name}", RegexOptions.IgnoreCase) ||
                                                     (weapon.Name.Equals("IWS", StringComparison.InvariantCultureIgnoreCase) && Regex.IsMatch(f, @"####\s+Interchangeable\s+Weapons\s+System", RegexOptions.IgnoreCase)) ||
                                                     (weapon.Name.Equals("E-web blaster", StringComparison.InvariantCultureIgnoreCase) && Regex.IsMatch(f, @"####\s+E-Web", RegexOptions.IgnoreCase)));
                            if (weaponDescriptionStartLine != -1)
                            {
                                var weaponDescriptionEndLine = lines.FindIndex(weaponDescriptionStartLine + 1, f => f.StartsWith("#") || f.StartsWith('|'));
                                weapon.Description = string.Join("\r\n",
                                    lines.Skip(weaponDescriptionStartLine + 1)
                                        .Take(weaponDescriptionEndLine - (weaponDescriptionStartLine + 1)));
                            }

                            equipmentList.Add(weapon);
                        }
                        else
                        {
                            var parentEquipment = equipmentList.Last();
                            parentEquipment.Modes.Add(weapon);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Failed while parsing {weapon.Name}", e);
                    }
                }
                else
                {
                    weaponClassification = DetermineWeaponClassification(tableLineSplit[1]);
                }
            }

            return Task.FromResult(equipmentList);
        }

        private Task<List<Equipment>> ParseAmmunition(List<string> lines)
        {
            var equipmentList = new List<Equipment>();

            var tableStart = lines.FindIndex(f => f.Contains("_Ammunition_"));
            var tableEnd = lines.FindIndex(tableStart + 1, f => f == string.Empty);
            var tableLines = lines.Skip(tableStart + 1).Take(tableEnd - (tableStart + 1)).Where(f => !f.Contains('_')).ToList();

            foreach (var tableLine in tableLines)
            {
                var ammunitionTableLineSplit = tableLine.Split('|');
                var ammunition = new Equipment
                {
                    ContentTypeEnum = ContentType.ExpandedContent,
                    PartitionKey = ContentType.ExpandedContent.ToString()
                };

                var costMatch = Regex.Match(ammunitionTableLineSplit[2], @"(?<!\S)(\d*\.?\d+|\d{1,3}(,\d{3})*(\.\d+)?)(?!\S)");
                ammunition.Name = ammunitionTableLineSplit[1].RemoveHtmlWhitespace().Trim();
                ammunition.RowKey = ammunition.Name;
                try
                {
                    ammunition.Cost = costMatch.Success ? int.Parse(costMatch.Value, NumberStyles.AllowThousands) : 0;
                    ammunition.EquipmentCategoryEnum = EquipmentCategory.Ammunition;

                    var weightMatch = Regex.Match(ammunitionTableLineSplit[3], @"\d+");
                    ammunition.Weight = weightMatch.Success ? int.Parse(weightMatch.Value) : 0;

                    var ammunitionDescriptionStartLine =
                        lines.FindIndex(f => Regex.IsMatch(f, $@"####\s+{ammunition.Name}", RegexOptions.IgnoreCase));
                    if (ammunitionDescriptionStartLine != -1)
                    {
                        var ammunitionDescriptionEndLine = lines.FindIndex(ammunitionDescriptionStartLine + 1, f => f.StartsWith("#") || f.StartsWith('|'));
                        ammunition.Description = string.Join("\r\n",
                            lines.Skip(ammunitionDescriptionStartLine + 1)
                                .Take(ammunitionDescriptionEndLine - (ammunitionDescriptionStartLine + 1)));
                    }

                    equipmentList.Add(ammunition);
                }
                catch (Exception e)
                {
                    throw new Exception($"Failed while parsing {ammunition.Name}", e);
                }
            }
            
            return Task.FromResult(equipmentList);
        }

        private Task<List<Equipment>> ParseOtherEquipment(List<string> lines)
        {
            var equipmentList = new List<Equipment>();

            var tableStart = lines.FindIndex(f => f.Contains("_Utilities_"));
            var tableLines = lines.Skip(tableStart).ToList();

            var equipmentCategory = EquipmentCategory.Unknown;
            foreach (var tableLine in tableLines)
            {
                if (tableLine.Contains("_Utilities_"))
                {
                    equipmentCategory = EquipmentCategory.Utility;
                }
                else if (tableLine.Contains("_Weapon and Armor Accessories_"))
                {
                    equipmentCategory = EquipmentCategory.WeaponOrArmorAccessory;
                }
                else
                {
                    var otherEquipmentTableLineSplit = tableLine.Split('|');
                    var otherEquipment = new Equipment
                    {
                        ContentTypeEnum = ContentType.ExpandedContent,
                        PartitionKey = ContentType.ExpandedContent.ToString()
                    };

                    var costMatch = Regex.Match(otherEquipmentTableLineSplit[2], @"(?<!\S)(\d*\.?\d+|\d{1,3}(,\d{3})*(\.\d+)?)(?!\S)");
                    otherEquipment.Name = otherEquipmentTableLineSplit[1].RemoveHtmlWhitespace().Trim();
                    otherEquipment.RowKey = otherEquipment.Name;
                    try {
                        otherEquipment.Cost = costMatch.Success ? int.Parse(costMatch.Value, NumberStyles.AllowThousands) : 0;
                        otherEquipment.EquipmentCategoryEnum = equipmentCategory;

                        var weightMatch = Regex.Match(otherEquipmentTableLineSplit[3], @"\d+");
                        otherEquipment.Weight = weightMatch.Success ? int.Parse(weightMatch.Value) : 0;

                        var otherEquipmentDescriptionStartLine =
                            lines.FindIndex(f => Regex.IsMatch(f, $@"####\s+{otherEquipment.Name}", RegexOptions.IgnoreCase));
                        if (otherEquipmentDescriptionStartLine != -1)
                        {
                            var otherEquipmentDescriptionEndLine = lines.FindIndex(otherEquipmentDescriptionStartLine + 1, f => f.StartsWith("#") || f.StartsWith('|'));
                            otherEquipment.Description = string.Join("\r\n",
                                lines.Skip(otherEquipmentDescriptionStartLine + 1)
                                    .Take(otherEquipmentDescriptionEndLine - (otherEquipmentDescriptionStartLine + 1)));
                        }

                        equipmentList.Add(otherEquipment);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Failed while parsing {otherEquipment.Name}", e);
                    }
                }
            }

            return Task.FromResult(equipmentList);
        }

        private WeaponClassification DetermineWeaponClassification(string weaponClassificationLine)
        {
            if (Regex.IsMatch(weaponClassificationLine, @"Martial\s*Blaster", RegexOptions.IgnoreCase))
            {
                return WeaponClassification.MartialBlaster;
            }
            if (Regex.IsMatch(weaponClassificationLine, @"Simple\s*Blaster", RegexOptions.IgnoreCase))
            {
                return WeaponClassification.SimpleBlaster;
            }

            return WeaponClassification.Unknown;
        }
    }
}
