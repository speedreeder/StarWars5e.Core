using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models;
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

            equipment.AddRange(await ParseWeapons(lines, "_Simple Blasters_", true, 1, ContentType.ExpandedContent));
            equipment.AddRange(await ParseOtherEquipment(lines.ToList(), "_Ammunition_", true, 1, ContentType.ExpandedContent));
            //equipment.AddRange(await ParseOtherEquipment(lines.ToList(), "_Utilities_", false, 1, ContentType.ExpandedContent));

            return equipment;
        }

        public Task<List<Equipment>> ParseWeapons(List<string> lines, string tableName, bool tableNameIsStartingCategory, int tableNameOccurence, ContentType contentType)
        {
            var equipmentList = new List<Equipment>();
            List<string> tableLines;


            if (tableNameIsStartingCategory)
            {
                var tableStart = lines.FindNthIndex(f => f.Contains(tableName), tableNameOccurence);
                var tableEnd = lines.FindIndex(tableStart, f => f == string.Empty);
                if (tableEnd == -1) tableEnd = lines.Count - 1;
                tableLines = lines.Skip(tableStart).Take(tableEnd - tableStart).CleanListOfStrings(false).ToList();
            }
            else
            {
                var tableStart = lines.FindNthIndex(f => f.Contains(tableName), tableNameOccurence);
                var tableEnd = lines.FindIndex(tableStart + 3, f => f == string.Empty);
                if (tableEnd == -1) tableEnd = lines.Count - 1;
                tableLines = lines.Skip(tableStart + 3).Take(tableEnd - (tableStart + 3)).CleanListOfStrings(false).ToList();
            }

            //var tableNameIndex = lines.FindIndex(f => f.Contains(tableName));

            //var tableStart = lines.FindIndex(tableNameIndex, f => Regex.IsMatch(f, @"^\|.*_\w*"));
            //var tableEnd = lines.FindIndex(tableStart + 1, f => f == string.Empty);
            //var tableLines = lines.Skip(tableStart).Take(tableEnd - tableStart).CleanListOfStrings().ToList();

            var weaponClassification = WeaponClassification.Unknown;
            foreach (var tableLine in tableLines)
            {
                var tableLineSplit = tableLine.Split('|');

                var costMatch = Regex.Match(tableLineSplit[2], @"(?<!\S)(\d*\.?\d+|\d{1,3}(,\d{3})*(\.\d+)?)(?!\S)");
                if (!tableLineSplit[1].Contains("_"))
                {
                    var weapon = new Equipment
                    {
                        ContentTypeEnum = contentType,
                        PartitionKey = contentType.ToString()
                    };

                    var weightMatch = Regex.Match(tableLineSplit[4], @"\d+");
                    weapon.Name = tableLineSplit[1].RemoveHtmlWhitespace().Trim();
                    weapon.RowKey = weapon.Name;
                    try
                    {
                        weapon.Weight = weightMatch.Success ? int.Parse(weightMatch.Value) : 0;
                        weapon.Properties = tableLineSplit[5].Split(',').Select(s => s.Trim().RemoveHtmlWhitespace().RemovePlaceholderCharacter())
                            .Where(p => !string.IsNullOrWhiteSpace(p))
                            .ToList();
                        weapon.PropertiesMap = weapon.Properties.ToDictionary(
                            s => WeaponPropertyConstant.WeaponProperties.FirstOrDefault(f =>
                                     s.Contains(f, StringComparison.InvariantCultureIgnoreCase)) ?? "", s => s);

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
                            weapon.WeaponClassificationEnum = weaponClassification;
                            weapon.EquipmentCategoryEnum = EquipmentCategory.Weapon;

                            var weaponDescriptionStartLine =
                                lines.FindIndex(f => Regex.IsMatch(f, $@"####\s+{weapon.Name}", RegexOptions.IgnoreCase) ||
                                                     weapon.Name.Equals("IWS", StringComparison.InvariantCultureIgnoreCase) && Regex.IsMatch(f, @"####\s+Interchangeable\s+Weapons\s+System", RegexOptions.IgnoreCase));
                            if (weaponDescriptionStartLine != -1)
                            {
                                var weaponDescriptionEndLine = lines.FindIndex(weaponDescriptionStartLine + 1, f => f.StartsWith("#") || f.StartsWith('|'));
                                weapon.Description = string.Join("\r\n",
                                    lines.Skip(weaponDescriptionStartLine + 1)
                                        .Take(weaponDescriptionEndLine - (weaponDescriptionStartLine + 1)).CleanListOfStrings());
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

        public Task<List<Equipment>> ParseOtherEquipment(List<string> lines, string tableName, bool tableNameIsStartingCategory, int tableNameOccurence, ContentType contentType)
        {
            var equipmentList = new List<Equipment>();
            List<string> tableLines;

            if (tableNameIsStartingCategory)
            {
                var tableStart = lines.FindNthIndex(f => f.Contains(tableName), tableNameOccurence);
                var tableEnd = lines.FindIndex(tableStart, f => f == string.Empty);
                if (tableEnd == -1) tableEnd = lines.Count - 1;
                tableLines = lines.Skip(tableStart).Take(tableEnd - tableStart).CleanListOfStrings(false).ToList();   
            }
            else
            {
                var tableStart = lines.FindNthIndex(f => f.Contains(tableName), tableNameOccurence);
                var tableEnd = lines.FindIndex(tableStart + 3, f => f == string.Empty);
                if (tableEnd == -1) tableEnd = lines.Count - 1;
                tableLines = lines.Skip(tableStart + 3).Take(tableEnd - (tableStart + 3)).CleanListOfStrings(false).ToList();
            }

            var equipmentCategory = EquipmentCategory.Unknown;
            foreach (var tableLine in tableLines)
            {
                var otherEquipmentTableLineSplit = tableLine.Split('|');

                if (!otherEquipmentTableLineSplit[1].Contains("_"))
                {
                    var otherEquipment = new Equipment
                    {
                        ContentTypeEnum = contentType,
                        PartitionKey = contentType.ToString()
                    };

                    var costMatch = Regex.Match(otherEquipmentTableLineSplit[2],
                        @"(?<!\S)(\d*\.?\d+|\d{1,3}(,\d{3})*(\.\d+)?)(?!\S)");
                    otherEquipment.Name = otherEquipmentTableLineSplit[1].RemoveHtmlWhitespace().Trim();
                    otherEquipment.RowKey = otherEquipment.Name;
                    try
                    {
                        otherEquipment.Cost = costMatch.Success
                            ? int.Parse(costMatch.Value, NumberStyles.AllowThousands)
                            : 0;
                        otherEquipment.EquipmentCategoryEnum =
                            otherEquipmentTableLineSplit[1].HasLeadingHtmlWhitespace() ||
                            otherEquipmentTableLineSplit[1].HasLeadingWhitespace()
                                ? equipmentCategory
                                : EquipmentCategory.Unknown;

                        var weightMatch = Regex.Match(otherEquipmentTableLineSplit[3], @"\d+");
                        otherEquipment.Weight = weightMatch.Success ? int.Parse(weightMatch.Value) : 0;

                        var otherEquipmentDescriptionStartLine =
                            lines.FindIndex(f =>
                                Regex.IsMatch(f, $@"####\s+{otherEquipment.Name}", RegexOptions.IgnoreCase));
                        if (otherEquipmentDescriptionStartLine != -1)
                        {
                            var otherEquipmentDescriptionEndLine = lines.FindIndex(
                                otherEquipmentDescriptionStartLine + 1,
                                f => f.StartsWith("#") || f.StartsWith('|'));
                            if (otherEquipmentDescriptionEndLine != -1)
                            {
                                otherEquipment.Description = string.Join("\r\n",
                                    lines.Skip(otherEquipmentDescriptionStartLine + 1)
                                        .Take(otherEquipmentDescriptionEndLine -
                                              (otherEquipmentDescriptionStartLine + 1)).CleanListOfStrings());
                            }
                            else
                            {
                                otherEquipment.Description = string.Join("\r\n",
                                    lines.Skip(otherEquipmentDescriptionStartLine + 1).CleanListOfStrings());
                            }
                        }

                        equipmentList.Add(otherEquipment);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Failed while parsing {otherEquipment.Name}", e);
                    }
                }
                else
                {
                    equipmentCategory = DetermineEquipmentCategory(otherEquipmentTableLineSplit[1]);
                }
            }

            return Task.FromResult(equipmentList);
        }

        public Task<List<Equipment>> ParseArmor(List<string> lines, string tableName, ContentType contentType)
        {
            var equipmentList = new List<Equipment>();

            var tableStart = lines.FindIndex(f => f.Contains(tableName));
            var tableEnd = lines.FindIndex(tableStart + 3, f => f == string.Empty);
            var tableLines = lines.Skip(tableStart + 3).Take(tableEnd - (tableStart + 3)).CleanListOfStrings().ToList();

            var armorClassification = ArmorClassification.Unknown;
            foreach (var tableLine in tableLines)
            {
                var tableLineSplit = tableLine.Split('|');
                
                if (!tableLineSplit[1].Contains("_"))
                {
                    var armor = new Equipment
                    {
                        ContentTypeEnum = contentType,
                        PartitionKey = contentType.ToString()
                    };

                    var costMatch = Regex.Match(tableLineSplit[2], @"(?<!\S)(\d*\.?\d+|\d{1,3}(,\d{3})*(\.\d+)?)(?!\S)");
                    armor.Name = tableLineSplit[1].RemoveHtmlWhitespace().Trim();
                    armor.RowKey = armor.Name;
                    armor.EquipmentCategoryEnum = EquipmentCategory.Armor;
                    try
                    {
                        armor.ArmorClassificationEnum = armorClassification;
                        armor.Cost = costMatch.Success
                            ? int.Parse(costMatch.Value, NumberStyles.AllowThousands)
                            : 0;

                        var weightMatch = Regex.Match(tableLineSplit[7], @"\d+");
                        armor.Weight = weightMatch.Success ? int.Parse(weightMatch.Value) : 0;
                        equipmentList.Add(armor);

                        armor.AC = tableLineSplit[4].Trim().RemoveHtmlWhitespace();
                        armor.StrengthRequirement = tableLineSplit[5].Trim().RemoveHtmlWhitespace();
                        armor.StealthDisadvantage = tableLineSplit[6].Trim().RemoveHtmlWhitespace()
                            .Equals("Disadvantage", StringComparison.InvariantCultureIgnoreCase);

                        var armorDescriptionStartLine =
                            lines.FindIndex(f =>
                                Regex.IsMatch(f, $@"####\s+{armor.Name}", RegexOptions.IgnoreCase));
                        if (armorDescriptionStartLine != -1)
                        {
                            var otherEquipmentDescriptionEndLine = lines.FindIndex(
                                armorDescriptionStartLine + 1,
                                f => f.StartsWith("#") || f.StartsWith('|'));
                            armor.Description = string.Join("\r\n",
                                lines.Skip(armorDescriptionStartLine + 1)
                                    .Take(otherEquipmentDescriptionEndLine -
                                          (armorDescriptionStartLine + 1)).CleanListOfStrings());
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Failed while parsing {armor.Name}", e);
                    }
                }
                else
                {
                    armorClassification = DetermineArmorClassification(tableLineSplit[1]);
                }
            }

            return Task.FromResult(equipmentList);
        }

        private static WeaponClassification DetermineWeaponClassification(string weaponClassificationLine)
        {
            if (Regex.IsMatch(weaponClassificationLine, @"_\s*Martial\s*Blaster[s]?\s*_", RegexOptions.IgnoreCase))
            {
                return WeaponClassification.MartialBlaster;
            }
            if (Regex.IsMatch(weaponClassificationLine, @"_\s*Simple\s*Blaster[s]?\s*_", RegexOptions.IgnoreCase))
            {
                return WeaponClassification.SimpleBlaster;
            }
            if (Regex.IsMatch(weaponClassificationLine, @"_\s*Simple\s*Lightweapon[s]?\s*_", RegexOptions.IgnoreCase))
            {
                return WeaponClassification.SimpleLightweapon;
            }
            if (Regex.IsMatch(weaponClassificationLine, @"_\s*Martial\s*Lightweapon[s]?\s*_", RegexOptions.IgnoreCase))
            {
                return WeaponClassification.MartialLightweapon;
            }
            if (Regex.IsMatch(weaponClassificationLine, @"_\s*Simple\s*Vibroweapon[s]?\s*_", RegexOptions.IgnoreCase))
            {
                return WeaponClassification.SimpleVibroweapon;
            }
            if (Regex.IsMatch(weaponClassificationLine, @"_\s*Martial\s*Vibroweapon[s]?\s*_", RegexOptions.IgnoreCase))
            {
                return WeaponClassification.MartialVibroweapon;
            }

            return WeaponClassification.Unknown;
        }

        private static EquipmentCategory DetermineEquipmentCategory(string equipmentCategoryLine)
        {
            if (Regex.IsMatch(equipmentCategoryLine, @"_\s*Utilities\s*_", RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Utility;
            }
            if (Regex.IsMatch(equipmentCategoryLine, @"_\s*Weapon\s*and\s*Armor\s*Accessories\s*_", RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.WeaponOrArmorAccessory;
            }
            if (Regex.IsMatch(equipmentCategoryLine, @"_\s*Ammunition\s*_", RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Ammunition;
            }
            if (Regex.IsMatch(equipmentCategoryLine, @"_\s*Clothing\s*_", RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Clothing;
            }
            if (Regex.IsMatch(equipmentCategoryLine, @"_\s*Communications\s*_", RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Communications;
            }
            if (Regex.IsMatch(equipmentCategoryLine, @"_\s*Data\s*Recording\s*and\s*Storage\s*_", RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.DataRecordingAndStorage;
            }
            if (Regex.IsMatch(equipmentCategoryLine, @"_\s*Explosives\s*_", RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Explosive;
            }
            if (Regex.IsMatch(equipmentCategoryLine, @"_\s*Life\s*Support\s*_", RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.LifeSupport;
            }
            if (Regex.IsMatch(equipmentCategoryLine, @"_\s*Medical\s*_", RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Medical;
            }
            if (Regex.IsMatch(equipmentCategoryLine, @"_\s*Storage\s*_", RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Storage;
            }
            if (Regex.IsMatch(equipmentCategoryLine, @"_\s*Artisan's\s*tools\s*_", RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Tool;
            }
            if (Regex.IsMatch(equipmentCategoryLine, @"_\s*Gaming\s*set\s*_", RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.GamingSet;
            }
            if (Regex.IsMatch(equipmentCategoryLine, @"_\s*Musical\s*instrument\s*_", RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.MusicalInstrument;
            }
            if (Regex.IsMatch(equipmentCategoryLine, @"_\s*Specialist's\s*kit\s*_", RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Kit;
            }

            return EquipmentCategory.Unknown;
        }

        private static ArmorClassification DetermineArmorClassification(string armorClassificationLine)
        {
            if (Regex.IsMatch(armorClassificationLine, @"_\s*Light\s*Armor\s*_", RegexOptions.IgnoreCase))
            {
                return ArmorClassification.Light;
            }
            if (Regex.IsMatch(armorClassificationLine, @"_\s*Medium\s*Armor\s*_", RegexOptions.IgnoreCase))
            {
                return ArmorClassification.Medium;
            }
            if (Regex.IsMatch(armorClassificationLine, @"_\s*Heavy\s*Armor\s*_", RegexOptions.IgnoreCase))
            {
                return ArmorClassification.Heavy;
            }
            if (Regex.IsMatch(armorClassificationLine, @"_\s*Shield\s*_", RegexOptions.IgnoreCase))
            {
                return ArmorClassification.Shield;
            }

            return ArmorClassification.Unknown;
        }
    }
}
