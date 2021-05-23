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
using StarWars5e.Parser.Localization;

namespace StarWars5e.Parser.Processors
{
    public class ExpandedContentEquipmentProcessor : BaseProcessor<Equipment>
    {
        public ExpandedContentEquipmentProcessor(ILocalization localization)
        {
            Localization = localization;
        }
        public override async Task<List<Equipment>> FindBlocks(List<string> lines)
        {
            var equipment = new List<Equipment>();

            equipment.AddRange(await ParseWeapons(lines, Localization.ECBlastersStartLine, false, 1, ContentType.ExpandedContent));
            equipment.AddRange(await ParseWeapons(lines, Localization.ECMartialLightweaponsStartLine, true, 1, ContentType.ExpandedContent));
            equipment.AddRange(await ParseWeapons(lines, Localization.ECMartialVibroweaponsStartLine, true, 1, ContentType.ExpandedContent));
            equipment.AddRange(await ParseOtherEquipment(lines, Localization.ECAmmunitionStartLine, true, 1, ContentType.ExpandedContent));
            equipment.AddRange(await ParseOtherEquipment(lines, Localization.ECStorageStartLine, true, 1, ContentType.ExpandedContent));

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

                    var weightMatch = Regex.Match(tableLineSplit[4], @"\d+\s*\d*\/*\d*");
                    weapon.Name = tableLineSplit[1].RemoveHtmlWhitespace().Trim();
                    weapon.RowKey = weapon.Name;
                    try
                    {
                        weapon.Weight = weightMatch.Success ? weightMatch.Value.Trim() : "0";
                        weapon.Properties = Regex.Split(tableLineSplit[5], @",\s*(?![^()]*\))").Select(s => s.Trim().RemoveHtmlWhitespace().RemovePlaceholderCharacter())
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
                                lines.FindIndex(f =>
                                    Regex.IsMatch(f, $@"####\s+{weapon.Name}", RegexOptions.IgnoreCase) ||
                                    weapon.Name.Equals(Localization.IWS, StringComparison.InvariantCultureIgnoreCase) &&
                                    Regex.IsMatch(f, Localization.ECInterchangeableWeaponsSystemPattern,
                                        RegexOptions.IgnoreCase));
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

                        var weightMatch = Regex.Match(otherEquipmentTableLineSplit[3], @"\d+\s*\d*\/*\d*");
                        otherEquipment.Weight = weightMatch.Success ? weightMatch.Value.Trim() : "0";

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

                        var weightMatch = Regex.Match(tableLineSplit[4], @"\d+\s*\d*\/*\d*");
                        armor.Weight = weightMatch.Success ? weightMatch.Value.Trim() : "0";
                        armor.Properties = tableLineSplit[5].Split(',').Select(s => s.Trim().RemoveHtmlWhitespace().RemovePlaceholderCharacter())
                            .Where(p => !string.IsNullOrWhiteSpace(p))
                            .ToList();
                        armor.PropertiesMap = armor.Properties.ToDictionary(
                            s => ArmorPropertyConstant.ArmorProperties.FirstOrDefault(f =>
                                     s.Contains(f, StringComparison.InvariantCultureIgnoreCase)) ?? "", s => s);

                        equipmentList.Add(armor);

                        armor.AC = tableLineSplit[3].Trim().RemoveHtmlWhitespace();
                        armor.StrengthRequirement = Regex.Match(tableLineSplit[5].Trim().RemoveHtmlWhitespace(), Localization.ECStrengthRequirementPattern).Value;
                        armor.StealthDisadvantage = tableLineSplit[5].Trim().RemoveHtmlWhitespace()
                            .Contains(Localization.Bulky, StringComparison.InvariantCultureIgnoreCase);

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

        private WeaponClassification DetermineWeaponClassification(string weaponClassificationLine)
        {
            if (Regex.IsMatch(weaponClassificationLine, Localization.ECClassificationMartialBlasters, RegexOptions.IgnoreCase))
            {
                return WeaponClassification.MartialBlaster;
            }
            if (Regex.IsMatch(weaponClassificationLine, Localization.ECClassificationSimpleBlasters, RegexOptions.IgnoreCase))
            {
                return WeaponClassification.SimpleBlaster;
            }
            if (Regex.IsMatch(weaponClassificationLine, Localization.ECClassificationSimpleLightweapons, RegexOptions.IgnoreCase))
            {
                return WeaponClassification.SimpleLightweapon;
            }
            if (Regex.IsMatch(weaponClassificationLine, Localization.ECClassificationMartialLightweapons, RegexOptions.IgnoreCase))
            {
                return WeaponClassification.MartialLightweapon;
            }
            if (Regex.IsMatch(weaponClassificationLine, Localization.ECClassificationSimpleVibroweapons, RegexOptions.IgnoreCase))
            {
                return WeaponClassification.SimpleVibroweapon;
            }
            if (Regex.IsMatch(weaponClassificationLine, Localization.ECClassificationMartialVibroweapons, RegexOptions.IgnoreCase))
            {
                return WeaponClassification.MartialVibroweapon;
            }

            return WeaponClassification.Unknown;
        }

        private EquipmentCategory DetermineEquipmentCategory(string equipmentCategoryLine)
        {
            if (Regex.IsMatch(equipmentCategoryLine, Localization.ECClassificationUtilities, RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Utility;
            }
            if (Regex.IsMatch(equipmentCategoryLine, Localization.ECClassificationWeaponAndArmorAccessories, RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.WeaponOrArmorAccessory;
            }
            if (Regex.IsMatch(equipmentCategoryLine, Localization.ECClassificationAmmunition, RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Ammunition;
            }
            if (Regex.IsMatch(equipmentCategoryLine, Localization.ECClassificationClothing, RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Clothing;
            }
            if (Regex.IsMatch(equipmentCategoryLine, Localization.ECClassificationCommunication, RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Communications;
            }
            if (Regex.IsMatch(equipmentCategoryLine, Localization.ECClassificationDataRecordingAndStorage, RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.DataRecordingAndStorage;
            }
            if (Regex.IsMatch(equipmentCategoryLine, Localization.ECClassificationExplosives, RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Explosive;
            }
            if (Regex.IsMatch(equipmentCategoryLine, Localization.ECClassificationLifeSupport, RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.LifeSupport;
            }
            if (Regex.IsMatch(equipmentCategoryLine, Localization.ECClassificationMedical, RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Medical;
            }
            if (Regex.IsMatch(equipmentCategoryLine, Localization.ECClassificationStorage, RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Storage;
            }
            if (Regex.IsMatch(equipmentCategoryLine, Localization.ECClassificationArtisansImplements, RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Tool;
            }
            if (Regex.IsMatch(equipmentCategoryLine, Localization.ECClassificationGamingSet, RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.GamingSet;
            }
            if (Regex.IsMatch(equipmentCategoryLine, Localization.ECClassificationMusicalInstrument, RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.MusicalInstrument;
            }
            if (Regex.IsMatch(equipmentCategoryLine, Localization.ECClassificationSpecialistsKit, RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Kit;
            }
            if (Regex.IsMatch(equipmentCategoryLine, Localization.ECClassificationAlcoholicBeverages, RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.AlcoholicBeverage;
            }
            if (Regex.IsMatch(equipmentCategoryLine, Localization.ECClassificationSpices, RegexOptions.IgnoreCase))
            {
                return EquipmentCategory.Spice;
            }

            return EquipmentCategory.Unknown;
        }

        private ArmorClassification DetermineArmorClassification(string armorClassificationLine)
        {
            if (Regex.IsMatch(armorClassificationLine, Localization.ECClassificationLightArmor, RegexOptions.IgnoreCase))
            {
                return ArmorClassification.Light;
            }
            if (Regex.IsMatch(armorClassificationLine, Localization.ECClassificationMediumArmor, RegexOptions.IgnoreCase))
            {
                return ArmorClassification.Medium;
            }
            if (Regex.IsMatch(armorClassificationLine, Localization.ECClassificationHeavyArmor, RegexOptions.IgnoreCase))
            {
                return ArmorClassification.Heavy;
            }
            if (Regex.IsMatch(armorClassificationLine, Localization.ECClassificationShield, RegexOptions.IgnoreCase))
            {
                return ArmorClassification.Shield;
            }

            return ArmorClassification.Unknown;
        }
    }
}
