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

            var tertiaryAmmunitionTableStartIndex = lines.FindIndex(f => f.Contains(Localization.SOTGTertiaryAmmunitionTableStartingLine, StringComparison.InvariantCultureIgnoreCase)) + 1;
            var tertiaryAmmunitionTableEndIndex = lines.FindIndex(tertiaryAmmunitionTableStartIndex, string.IsNullOrWhiteSpace);
            var tertiaryAmmunitionTableLines = lines.Skip(tertiaryAmmunitionTableStartIndex).Take(tertiaryAmmunitionTableEndIndex - tertiaryAmmunitionTableStartIndex).ToList();

            var quaternaryAmmunitionTableStartIndex = lines.FindIndex(f => f.Contains(Localization.SOTGQuaternaryAmmunitionTableStartingLine, StringComparison.InvariantCultureIgnoreCase)) + 1;
            var quaternaryAmmunitionTableEndIndex = lines.FindIndex(quaternaryAmmunitionTableStartIndex, string.IsNullOrWhiteSpace);
            var quaternaryAmmunitionTableLines = lines.Skip(quaternaryAmmunitionTableStartIndex).Take(quaternaryAmmunitionTableEndIndex - quaternaryAmmunitionTableStartIndex).ToList();

            var hyperdriveTableStartIndex = lines.FindIndex(f => f.Contains(Localization.SOTGHyperdrivesTableStartingLine, StringComparison.InvariantCultureIgnoreCase)) + 1;
            var hyperdriveTableEndIndex = lines.FindIndex(hyperdriveTableStartIndex, string.IsNullOrWhiteSpace);
            var hyperdriveTableLines = lines.Skip(hyperdriveTableStartIndex).Take(hyperdriveTableEndIndex - hyperdriveTableStartIndex).ToList();

            var navcomputerTableStartIndex = lines.FindIndex(f => f.Contains(Localization.SOTGNavcomputerTableStartingLine, StringComparison.InvariantCultureIgnoreCase)) + 1;
            var navcomputerTableEndIndex = lines.FindIndex(navcomputerTableStartIndex, string.IsNullOrWhiteSpace);
            var navcomputerTableLines = lines.Skip(navcomputerTableStartIndex).Take(navcomputerTableEndIndex - navcomputerTableStartIndex).ToList();

            var reactorsAndPowerCouplingsTableStartIndex = lines.FindIndex(f => f.Contains(Localization.SOTGReactorsTableStartingLineFuelCosts, StringComparison.InvariantCultureIgnoreCase)) + 1;
            var reactorsAndPowerCouplingsEndIndex = lines.FindIndex(reactorsAndPowerCouplingsTableStartIndex, string.IsNullOrWhiteSpace);
            var reactorsAndPowerCouplingsTableLines = lines.Skip(reactorsAndPowerCouplingsTableStartIndex).Take(reactorsAndPowerCouplingsEndIndex - reactorsAndPowerCouplingsTableStartIndex).ToList();

            starshipEquipment.AddRange(CreateArmorAndShields(armorTableLines, lines));
            starshipEquipment.AddRange(CreateWeapons(smallWeaponTableLines, true));
            starshipEquipment.AddRange(CreateWeapons(hugeWeaponTableLines, false));
            starshipEquipment.AddRange(CreateAmmunition(tertiaryAmmunitionTableLines, lines));
            starshipEquipment.AddRange(CreateAmmunition(quaternaryAmmunitionTableLines, lines));
            starshipEquipment.AddRange(CreateHyperdrives(hyperdriveTableLines));
            starshipEquipment.AddRange(CreateNavcomputers(navcomputerTableLines));
            starshipEquipment.AddRange(CreateReactorsAndPowerCouplings(reactorsAndPowerCouplingsTableLines, lines));

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
                        Properties = weaponColumns[5]
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

                    var reloadIndex = weaponColumns[5].IndexOf(Localization.reload, StringComparison.InvariantCultureIgnoreCase);
                    if (reloadIndex != -1)
                    {
                        var checkForReload = Regex.Match(weaponColumns[5].Substring(reloadIndex), @"-?\d+");
                        if (checkForReload.Success)
                        {
                            weapon.Reload = int.Parse(checkForReload.Value);
                        }
                    }

                    var rangeIndex = weaponColumns[5].IndexOf(Localization.range, StringComparison.InvariantCultureIgnoreCase);
                    if (rangeIndex != -1)
                    {
                        var shortRangeMatch = Regex.Match(weaponColumns[5].Substring(rangeIndex),
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
                var ammunitionLines = categoryLineIndex == categoryLineIndexes.Last()
                    ? ammunitionTableLines.Skip(categoryLineIndex + 1).Take(ammunitionTableLines.Count - categoryLineIndex - 1)
                        .ToList()
                    : ammunitionTableLines.Skip(categoryLineIndex + 1)
                        .Take(categoryLineIndexes[categoryLineIndexes.IndexOf(categoryLineIndex) + 1] - categoryLineIndex - 1).ToList();

                ammunitionLines = ammunitionLines.Select(a => a.RemoveHtmlWhitespace()).ToList();

                foreach (var ammunitionLine in ammunitionLines)
                {
                    var ammunitionColumns = ammunitionLine.Split('|');
                    var ammunition = new StarshipEquipment
                    {
                        PartitionKey = ContentType.Core.ToString(),
                        ContentType = ContentType.Core.ToString(),
                        RowKey = ammunitionColumns[1].RemoveHtmlWhitespace().Trim(),
                        TypeEnum = StarshipEquipmentType.Ammunition,
                        Name = ammunitionColumns[1].RemoveHtmlWhitespace().Trim(),
                        Cost = int.TryParse(ammunitionColumns[2].Trim().Replace(" cr", string.Empty), NumberStyles.AllowThousands, null, out var cost) ? cost : 0
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
                    Cost = int.TryParse(hyperdriveColumns[2].Replace(" cr", string.Empty), NumberStyles.AllowThousands, null, out var cost) ? cost : 0,
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
                    Cost = int.TryParse(navcomputerColumns[2].Replace(" cr", string.Empty), NumberStyles.AllowThousands, null, out var cost) ? cost : 0,
                    NavcomputerBonus = navcomputerBonusMatch.Success ? int.Parse(navcomputerBonusMatch.Value) : 0
                };
                navcomputerList.Add(navcomputer);
            }

            return navcomputerList;
        }

        private IEnumerable<StarshipEquipment> CreateReactorsAndPowerCouplings(List<string> reactorsTableLines, List<string> allLines)
        {
            var reactorsAndPowerCouplings = new List<StarshipEquipment>();

            var reactorsStart = reactorsTableLines.FindIndex(f => f.Contains(Localization.SOTGReactorsTableReactorsStartingLine, StringComparison.InvariantCultureIgnoreCase)) + 1;
            var powerCouplingsStart =
                reactorsTableLines.FindIndex(f => f.Contains(Localization.SOTGReactorsTablePowerCouplingsStartingLine, StringComparison.InvariantCultureIgnoreCase)) + 1;

            var reactorLines = reactorsTableLines.Skip(reactorsStart).Take(powerCouplingsStart - reactorsStart - 1);
            var powerCouplingLines = reactorsTableLines.Skip(powerCouplingsStart);

            foreach (var reactorLine in reactorLines)
            {
                var reactorColumns = reactorLine.Split('|').Select(s => s.RemoveHtmlWhitespace().Trim()).ToList();
                var reactor = new StarshipEquipment
                {
                    PartitionKey = ContentType.Core.ToString(),
                    ContentType = ContentType.Core.ToString(),
                    RowKey = reactorColumns[1],
                    TypeEnum = StarshipEquipmentType.Reactor,
                    Name = reactorColumns[1],
                    Cost = int.TryParse(reactorColumns[2].Replace(" cr", string.Empty), NumberStyles.AllowThousands, null, out var cost) ? cost : 0,
                    FuelCostsModifier = Regex.IsMatch(reactorColumns[3], @"^\s*-\s*$") ? null : reactorColumns[3],
                    PowerDiceRecover = Regex.IsMatch(reactorColumns[4], @"^\s*-\s*$") ? null : reactorColumns[4],
                    CentralStorageCapacity = Regex.IsMatch(reactorColumns[5], @"^\s*-\s*$") ? null : reactorColumns[5],
                    SystemStorageCapacity = Regex.IsMatch(reactorColumns[6], @"^\s*-\s*$") ? null : reactorColumns[6]
                };

                var reactorDescriptionStart = allLines.FindIndex(f => f.Contains($"#### {reactor.Name}", StringComparison.InvariantCultureIgnoreCase));
                if (reactorDescriptionStart != -1)
                {
                    var reactorDescriptionEnd = allLines.FindIndex(reactorDescriptionStart, string.IsNullOrWhiteSpace);
                    reactor.Description = string.Join("\r\n", allLines.Skip(reactorDescriptionStart + 1)
                        .Take(reactorDescriptionEnd - (reactorDescriptionStart + 1)).CleanListOfStrings());
                }

                reactorsAndPowerCouplings.Add(reactor);
            }

            foreach (var powerCouplingLine in powerCouplingLines)
            {
                var powerCouplingColumns = powerCouplingLine.Split('|').Select(s => s.RemoveHtmlWhitespace().Trim()).ToList();
                var powerCoupling = new StarshipEquipment
                {
                    PartitionKey = ContentType.Core.ToString(),
                    ContentType = ContentType.Core.ToString(),
                    RowKey = powerCouplingColumns[1],
                    TypeEnum = StarshipEquipmentType.PowerCoupling,
                    Name = powerCouplingColumns[1],
                    Cost = int.TryParse(powerCouplingColumns[2].Replace(" cr", string.Empty), NumberStyles.AllowThousands, null, out var cost) ? cost : 0,
                    FuelCostsModifier = Regex.IsMatch(powerCouplingColumns[3], @"^\s*-\s*$") ? null : powerCouplingColumns[3],
                    PowerDiceRecover = Regex.IsMatch(powerCouplingColumns[4], @"^\s*-\s*$") ? null : powerCouplingColumns[4],
                    CentralStorageCapacity = Regex.IsMatch(powerCouplingColumns[5], @"^\s*-\s*$") ? null : powerCouplingColumns[5],
                    SystemStorageCapacity = Regex.IsMatch(powerCouplingColumns[6], @"^\s*-\s*$") ? null : powerCouplingColumns[6]
                };

                var powerCouplingDescriptionStart = allLines.FindIndex(f => f.Contains($"#### {powerCoupling.Name}", StringComparison.InvariantCultureIgnoreCase));
                if (powerCouplingDescriptionStart != -1)
                {
                    var powerCouplingDescriptionEnd = allLines.FindIndex(powerCouplingDescriptionStart, string.IsNullOrWhiteSpace);
                    powerCoupling.Description = string.Join("\r\n", allLines.Skip(powerCouplingDescriptionStart + 1)
                        .Take(powerCouplingDescriptionEnd - (powerCouplingDescriptionStart + 1)).CleanListOfStrings());
                }

                reactorsAndPowerCouplings.Add(powerCoupling);
            }

            return reactorsAndPowerCouplings;
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
