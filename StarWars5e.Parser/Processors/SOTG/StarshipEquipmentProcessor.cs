using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Starship;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Processors.SOTG
{
    public class StarshipEquipmentProcessor : BaseProcessor<StarshipEquipment>
    {
        public override Task<List<StarshipEquipment>> FindBlocks(List<string> lines)
        {
            var starshipEquipment = new List<StarshipEquipment>();

            var armorTableStartingIndex = lines.FindIndex(f => f.Contains(Localization.SOTGArmorTableStartingLineArmorClass) && f.Contains(Localization.SOTGArmorTableStartingLineShieldRegeneration));
            var armorTableEndIndex = lines.FindIndex(armorTableStartingIndex, string.IsNullOrWhiteSpace);
            var armorTableLines = lines.Skip(armorTableStartingIndex).Take(armorTableEndIndex - armorTableStartingIndex).ToList();

            var smallWeaponStart = lines.FindIndex(f => f.Contains(Localization.SOTGSmallWeaponsTableStartingLine, StringComparison.InvariantCultureIgnoreCase)) + 1;
            var smallWeaponsEndIndex = lines.FindIndex(smallWeaponStart, string.IsNullOrWhiteSpace);
            var smallWeaponTableLines = lines.Skip(smallWeaponStart).Take(smallWeaponsEndIndex - smallWeaponStart).ToList();

            var hugeWeaponStartIndex = lines.FindIndex(f => f.Contains(Localization.SOTGHugeWeaponsTableStartingLine, StringComparison.InvariantCultureIgnoreCase)) + 1;
            var hugeWeaponsEndIndex = lines.FindIndex(hugeWeaponStartIndex, string.IsNullOrWhiteSpace);
            var hugeWeaponTableLines = lines.Skip(hugeWeaponStartIndex).Take(hugeWeaponsEndIndex - hugeWeaponStartIndex).ToList();

            var ammunitionTableStartIndex = lines.FindIndex(f => f.Contains(Localization.SOTGAmmunitionTableStartingLine, StringComparison.InvariantCultureIgnoreCase)) + 1;
            var ammunitionTableEndIndex = lines.FindIndex(ammunitionTableStartIndex, string.IsNullOrWhiteSpace);
            var ammunitionTableLines = lines.Skip(ammunitionTableStartIndex).Take(ammunitionTableEndIndex - ammunitionTableStartIndex).ToList();

            var hyperdriveTableStartIndex = lines.FindIndex(f => f.Contains(Localization.SOTGHyperdrivesTableStartingLine, StringComparison.InvariantCultureIgnoreCase)) + 1;
            var hyperdriveTableEndIndex = lines.FindIndex(hyperdriveTableStartIndex, string.IsNullOrWhiteSpace);
            var hyperdriveTableLines = lines.Skip(hyperdriveTableStartIndex).Take(hyperdriveTableEndIndex - hyperdriveTableStartIndex).ToList();

            var navcomputerTableStartIndex = lines.FindIndex(f => f.Contains(Localization.SOTGNavcomputerTableStartingLine, StringComparison.InvariantCultureIgnoreCase)) + 1;
            var navcomputerTableEndIndex = lines.FindIndex(navcomputerTableStartIndex, string.IsNullOrWhiteSpace);
            var navcomputerTableLines = lines.Skip(navcomputerTableStartIndex).Take(navcomputerTableEndIndex - navcomputerTableStartIndex).ToList();

            starshipEquipment.AddRange(CreateArmorAndShields(armorTableLines, lines));
            starshipEquipment.AddRange(CreateWeapons(smallWeaponTableLines, true));
            starshipEquipment.AddRange(CreateWeapons(hugeWeaponTableLines, false));
            starshipEquipment.AddRange(CreateAmmunition(ammunitionTableLines, lines));
            starshipEquipment.AddRange(CreateHyperdrives(hyperdriveTableLines));
            starshipEquipment.AddRange(CreateNavcomputers(navcomputerTableLines));

            return Task.FromResult(starshipEquipment);
        }

        private IEnumerable<StarshipEquipment> CreateArmorAndShields(List<string> armorTableLines, List<string> allLines)
        {
            var armorAndShields = new List<StarshipEquipment>();

            var armorStart = armorTableLines.FindIndex(f => f.Contains(Localization.SOTGArmorTableArmorStartingLine, StringComparison.InvariantCultureIgnoreCase)) + 1;
            var shieldsStart =
                armorTableLines.FindIndex(f => f.Contains(Localization.SOTGArmorTableShieldsStartingLine, StringComparison.InvariantCultureIgnoreCase)) + 1;

            var armorLines = armorTableLines.Skip(armorStart).Take(shieldsStart - armorStart - 1);
            var shieldLines = armorTableLines.Skip(shieldsStart);

            foreach (var armorLine in armorLines)
            {
                var armorColumns = armorLine.Split('|').Select(s => s.RemoveHtmlWhitespace().Trim()).ToList();
                var armor = new StarshipEquipment
                {
                    PartitionKey = ContentType.Core.ToString(),
                    ContentType = ContentType.Core.ToString(),
                    RowKey = armorColumns[1],
                    TypeEnum = StarshipEquipmentType.Armor,
                    Name = armorColumns[1],
                    Cost = int.Parse(armorColumns[2].Replace(" cr", string.Empty), NumberStyles.AllowThousands),
                    ArmorClassBonus = int.TryParse(armorColumns[3], out var acBonus) ? acBonus : 0,
                    HitPointsPerHitDie = int.TryParse(armorColumns[4], out var hpPerHd) ? hpPerHd : 0
                };

                var armorDescriptionStart = allLines.FindIndex(f => f.Contains($"#### {armor.Name}", StringComparison.InvariantCultureIgnoreCase));
                if (armorDescriptionStart != -1)
                {
                    var armorDescriptionEnd = allLines.FindIndex(armorDescriptionStart, string.IsNullOrWhiteSpace);
                    armor.Description = string.Join("\r\n", allLines.Skip(armorDescriptionStart + 1)
                        .Take(armorDescriptionEnd - (armorDescriptionStart + 1)).CleanListOfStrings());
                }

                armorAndShields.Add(armor);
            }

            foreach (var shieldLine in shieldLines)
            {
                var shieldColumns = shieldLine.Split('|').Select(s => s.RemoveHtmlWhitespace().Trim()).ToList();
                var shield = new StarshipEquipment
                {
                    PartitionKey = ContentType.Core.ToString(),
                    ContentType = ContentType.Core.ToString(),
                    RowKey = shieldColumns[1],
                    TypeEnum = StarshipEquipmentType.Shield,
                    Name = shieldColumns[1],
                    Cost = int.TryParse(shieldColumns[2].Replace(" cr", string.Empty), NumberStyles.AllowThousands, null, out var cost) ? cost : 0,
                    CapacityMultiplier = shieldColumns[5].Trim(),
                    RegenerationRateCoefficient = shieldColumns[6].Trim()
                };

                var shieldDescriptionStart = allLines.FindIndex(f => f.Contains($"#### {shield.Name}", StringComparison.InvariantCultureIgnoreCase));
                if (shieldDescriptionStart != -1)
                {
                    var shieldDescriptionEnd = allLines.FindIndex(shieldDescriptionStart, string.IsNullOrWhiteSpace);
                    shield.Description = string.Join("\r\n", allLines.Skip(shieldDescriptionStart + 1)
                        .Take(shieldDescriptionEnd - (shieldDescriptionStart + 1)).CleanListOfStrings());
                }

                armorAndShields.Add(shield);
            }

            return armorAndShields;
        }

        private IEnumerable<StarshipEquipment> CreateWeapons(IList<string> weaponTableLines, bool isSmallWeapons)
        {
            var weapons = new List<StarshipEquipment>();

            var titleLineIndexes = weaponTableLines.Where(s => s.StartsWith("|_")).Select(weaponTableLines.IndexOf).ToList();
            foreach (var titleLineIndex in titleLineIndexes)
            {
                var weaponCategory = GetStarshipWeaponCategoryFromString(weaponTableLines[titleLineIndex]
                    .Replace("|", string.Empty).Replace("_", string.Empty).Trim());
                var weaponLines = titleLineIndex == titleLineIndexes.Last()
                    ? weaponTableLines.Skip(titleLineIndex + 1).Take(weaponTableLines.Count - titleLineIndex - 1)
                        .ToList()
                    : weaponTableLines.Skip(titleLineIndex + 1)
                        .Take(titleLineIndexes[titleLineIndexes.IndexOf(titleLineIndex) + 1] - titleLineIndex - 1).ToList();
                foreach (var weaponLine in weaponLines)
                {
                    var weaponColumns = weaponLine.Split('|');
                    var weapon = new StarshipEquipment
                    {
                        PartitionKey = ContentType.Core.ToString(),
                        ContentType = ContentType.Core.ToString(),
                        RowKey = weaponColumns[1].RemoveHtmlWhitespace().Trim(),
                        TypeEnum = StarshipEquipmentType.Weapon,
                        WeaponSizeEnum = isSmallWeapons ? StarshipWeaponSize.Small : StarshipWeaponSize.Huge,
                        WeaponCategoryEnum = weaponCategory,
                        Name = weaponColumns[1].RemoveHtmlWhitespace().Trim(),
                        Cost = int.Parse(weaponColumns[2].Replace(" cr", string.Empty), NumberStyles.AllowThousands),
                        DamageType = weaponColumns[4].Split(' ')[1].Trim(),
                        AttacksPerRound = int.Parse(weaponColumns[6]),
                        Properties = weaponColumns[7]
                    };

                    var damage = Regex.Matches(weaponColumns[4], @"-?\d+");
                    if (damage.Any())
                    {
                        weapon.DamageNumberOfDice = int.Parse(damage[0].Value);
                        weapon.DamageDiceDieTypeEnum = (DiceType)int.Parse(damage[1].Value);
                    }

                    if (damage.ElementAtOrDefault(2) != null)
                    {
                        weapon.DamageDieModifier = int.Parse(damage[2].Value);
                    }

                    var reloadIndex = weaponColumns[7].IndexOf(Localization.reload, StringComparison.InvariantCultureIgnoreCase);
                    if (reloadIndex != -1)
                    {
                        var checkForReload = Regex.Match(weaponColumns[7].Substring(reloadIndex), @"-?\d+");
                        if (checkForReload.Success)
                        {
                            weapon.Reload = int.Parse(checkForReload.Value);
                        }
                    }

                    var attackBonusMatch = Regex.Match(weaponColumns[5], @"-?\d+");
                    if (attackBonusMatch.Success)
                    {
                        weapon.AttackBonus = int.Parse(attackBonusMatch.Value);
                    }

                    var rangeIndex = weaponColumns[7].IndexOf(Localization.range, StringComparison.InvariantCultureIgnoreCase);
                    if (rangeIndex != -1)
                    {
                        var shortRangeMatch = Regex.Match(weaponColumns[7].Substring(rangeIndex),
                            @"[0-9]+(,[0-9]+)*");
                        weapon.ShortRange = int.Parse(shortRangeMatch.Value, NumberStyles.AllowThousands);
                        weapon.LongRange = int.Parse(shortRangeMatch.NextMatch().Value, NumberStyles.AllowThousands);
                    }

                    weapons.Add(weapon);
                }
            }

            return weapons;
        }

        private IEnumerable<StarshipEquipment> CreateAmmunition(IList<string> ammunitionTableLines, List<string> allLines)
        {
            var ammunitionList = new List<StarshipEquipment>();

            var categoryLineIndexes = ammunitionTableLines.Where(s => s.StartsWith("|_")).Select(ammunitionTableLines.IndexOf).ToList();
            foreach (var categoryLineIndex in categoryLineIndexes)
            {
                var weaponCategory = GetStarshipWeaponCategoryFromString(ammunitionTableLines[categoryLineIndex]
                    .Replace("|", string.Empty).Replace("_", string.Empty).Trim());
                var ammunitionLines = categoryLineIndex == categoryLineIndexes.Last()
                    ? ammunitionTableLines.Skip(categoryLineIndex + 1).Take(ammunitionTableLines.Count - categoryLineIndex - 1)
                        .ToList()
                    : ammunitionTableLines.Skip(categoryLineIndex + 1)
                        .Take(categoryLineIndexes[categoryLineIndexes.IndexOf(categoryLineIndex) + 1] - categoryLineIndex - 1).ToList();
                foreach (var ammunitionLine in ammunitionLines)
                {
                    var ammunitionColumns = ammunitionLine.Split('|');
                    var ammunition = new StarshipEquipment
                    {
                        PartitionKey = ContentType.Core.ToString(),
                        ContentType = ContentType.Core.ToString(),
                        RowKey = ammunitionColumns[1].RemoveHtmlWhitespace().Trim(),
                        TypeEnum = StarshipEquipmentType.Ammunition,
                        StarshipWeaponCategoryEnum = weaponCategory,
                        Name = ammunitionColumns[1].RemoveHtmlWhitespace().Trim(),
                        Cost = int.Parse(ammunitionColumns[2].Replace(" cr", string.Empty).Trim(), NumberStyles.AllowThousands)
                    };

                    var ammunitionDescriptionStart = allLines.FindIndex(f => f.Contains($"#### {ammunition.Name}", StringComparison.InvariantCultureIgnoreCase));
                    if (ammunitionDescriptionStart != -1)
                    {
                        var ammunitionDescriptionEnd = allLines.FindIndex(ammunitionDescriptionStart, string.IsNullOrWhiteSpace);
                        ammunition.Description = string.Join("\r\n", allLines.Skip(ammunitionDescriptionStart + 1)
                            .Take(ammunitionDescriptionEnd - (ammunitionDescriptionStart + 1)).CleanListOfStrings());
                    }

                    ammunitionList.Add(ammunition);
                }
            }

            return ammunitionList;
        }

        private static IEnumerable<StarshipEquipment> CreateHyperdrives(IEnumerable<string> hyperDriveTableLines)
        {
            var hyperdriveList = new List<StarshipEquipment>();
    
            foreach (var hyperdriveLine in hyperDriveTableLines.Skip(2))
            {
                var hyperdriveColumns = hyperdriveLine.Split('|');
                var doubleArray = Regex.Split(hyperdriveColumns[1], @"[^0-9\.]+")
                    .Where(c => c != "." && c.Trim() != "").ToList();
                var hyperdrive = new StarshipEquipment
                {
                    PartitionKey = ContentType.Core.ToString(),
                    ContentType = ContentType.Core.ToString(),
                    RowKey = hyperdriveColumns[1].RemoveHtmlWhitespace().Trim(),
                    TypeEnum = StarshipEquipmentType.Hyperdrive,
                    Name = hyperdriveColumns[1].RemoveHtmlWhitespace().Trim(),
                    Cost = int.Parse(hyperdriveColumns[2].Replace(" cr", string.Empty).Trim(), NumberStyles.AllowThousands),
                    HyperDriveClass = doubleArray[0]
                };
                hyperdriveList.Add(hyperdrive);
            }

            return hyperdriveList;
        }

        private static IEnumerable<StarshipEquipment> CreateNavcomputers(IEnumerable<string> navcomputerTableLines)
        {
            var navcomputerList = new List<StarshipEquipment>();

            foreach (var navcomputerLine in navcomputerTableLines.Skip(2))
            {
                var navcomputerColumns = navcomputerLine.Split('|');
                var navcomputerBonusMatch = Regex.Match(navcomputerColumns[1], @"\d+");
                var navcomputer = new StarshipEquipment
                {
                    PartitionKey = ContentType.Core.ToString(),
                    ContentType = ContentType.Core.ToString(),
                    RowKey = navcomputerColumns[1].RemoveHtmlWhitespace().Trim(),
                    TypeEnum = StarshipEquipmentType.Navcomputer,
                    Name = navcomputerColumns[1].RemoveHtmlWhitespace().Trim(),
                    Cost = int.Parse(navcomputerColumns[2].Replace(" cr", string.Empty).Trim(), NumberStyles.AllowThousands),
                    NavcomputerBonus = navcomputerBonusMatch.Success ? int.Parse(navcomputerBonusMatch.Value) : 0
                };
                navcomputerList.Add(navcomputer);
            }

            return navcomputerList;
        }

        private StarshipWeaponCategory GetStarshipWeaponCategoryFromString(string weaponCategoryText)
        {
            if (weaponCategoryText == Localization.PrimaryWeapons)
                return StarshipWeaponCategory.Primary;
            if (weaponCategoryText == Localization.SecondaryWeapons)
                return StarshipWeaponCategory.Secondary;
            if (weaponCategoryText == Localization.TertiaryWeapons)
                return StarshipWeaponCategory.Tertiary;
            return weaponCategoryText == Localization.QuaternaryWeapons ? StarshipWeaponCategory.Quaternary : StarshipWeaponCategory.None;
        }
    }
}
