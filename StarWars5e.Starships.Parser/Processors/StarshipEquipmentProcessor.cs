using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Starship;
using StarWars5e.Models.Utils;

namespace StarWars.Starships.Parser.Processors
{
    public class StarshipEquipmentProcessor : StarshipBaseProcessor<StarshipEquipment>
    {
        public override Task<List<StarshipEquipment>> FindBlocks(List<string> lines)
        {
            var starshipEquipment = new List<StarshipEquipment>();
            var armorAndShieldTable = new List<string>();
            var weaponsTable = new List<string>();

            var armorTableStartingIndex = lines.FindIndex(f => f.Contains("|Armor Class") && f.Contains("|Shield Regeneration"));
            var armorTableEndIndex = lines.FindIndex(armorTableStartingIndex, string.IsNullOrWhiteSpace);
            var armorTableLines = lines.Skip(armorTableStartingIndex).Take(armorTableEndIndex - armorTableStartingIndex).ToList();

            starshipEquipment.AddRange(CreateArmorAndShields(armorTableLines));

            return Task.FromResult(starshipEquipment);
        }

        private static List<StarshipEquipment> CreateArmorAndShields(List<string> armorTableLines)
        {
            var armorAndShields = new List<StarshipEquipment>();

            var armorStart = armorTableLines.FindIndex(f => f.Contains("_armor_", StringComparison.InvariantCultureIgnoreCase)) + 1;
            var shieldsStart =
                armorTableLines.FindIndex(f => f.Contains("_shields_", StringComparison.InvariantCultureIgnoreCase)) + 1;

            var armorLines = armorTableLines.Skip(armorStart).Take(shieldsStart - armorStart - 1);
            var shieldLines = armorTableLines.Skip(shieldsStart);

            foreach (var armorLine in armorLines)
            {
                var armorColumns = armorLine.Split('|').Select(s => s.RemoveHtmlWhitespace().Trim()).ToList();
                var armor = new StarshipArmor
                {
                    TypeEnum = StarshipEquipmentType.Armor,
                    Name = armorColumns[1],
                    Cost = int.Parse(armorColumns[2].Replace(" cr", string.Empty), NumberStyles.AllowThousands),
                    ArmorClassBonus = int.TryParse(armorColumns[3], out var acBonus) ? acBonus : 0,
                    HitPointsPerHitDie = int.TryParse(armorColumns[4], out var hpPerHd) ? hpPerHd : 0
                };

                armorAndShields.Add(armor);
            }

            foreach (var shieldLine in shieldLines)
            {
                var shieldColumns = shieldLine.Split('|').Select(s => s.RemoveHtmlWhitespace().Trim()).ToList();
                var shield = new StarshipShield
                {
                    TypeEnum = StarshipEquipmentType.Armor,
                    Name = shieldColumns[1],
                    Cost = int.TryParse(shieldColumns[2].Replace(" cr", string.Empty), out var cost) ? cost : 0,
                    CapacityMultiplier = decimal.TryParse(shieldColumns[5].Replace("x ", string.Empty), out var capacityMultiplier) ? capacityMultiplier : 0,
                    RegenerationRateCoefficient = decimal.Parse(
                        shieldColumns[6].Replace("x ", string.Empty)
                    )
                };

                armorAndShields.Add(shield);
            }

            return armorAndShields;
        }
    }
}
