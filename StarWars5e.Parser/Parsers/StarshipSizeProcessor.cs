using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Starship;

namespace StarWars5e.Parser.Parsers
{
    public class StarshipSizeProcessor : StarshipBaseProcessor<StarshipBaseSize>
    {
        private static readonly string[] SavingThrowOptions = {"Strength", "Dexterity", "Constitution"};

        public override Task<List<StarshipBaseSize>> FindBlocks(List<string> lines)
        {
            var starshipBaseSizes = new List<StarshipBaseSize>();
            var shipLinesWithSizes = new Dictionary<string, List<string>>();

            var shipLinesStart = lines.FindIndex(f => f.StartsWith("## Tiny Ships"));
            var shipLinesEnd = lines.FindIndex(shipLinesStart, f => f == "## Variant: Space Stations");
            var shipSectionLines = lines.Skip(shipLinesStart).Take(shipLinesEnd - shipLinesStart).ToList();

            for (var i = 0; i < shipSectionLines.Count; i++)
            {
                if (shipSectionLines[i].StartsWith("## ", StringComparison.InvariantCultureIgnoreCase) && !shipSectionLines[i].Contains("feature", StringComparison.InvariantCultureIgnoreCase))
                {
                    var sizeName = shipSectionLines[i].Split(' ')[1];
                    var endIndex = shipSectionLines.FindIndex(i + 1, x => x.StartsWith("## ", StringComparison.InvariantCultureIgnoreCase) && !x.Contains("feature", StringComparison.InvariantCultureIgnoreCase));
                    var shipLines = shipSectionLines.Skip(i).Take((endIndex == -1 ? shipSectionLines.Count - 1 : endIndex) - i).ToList();
                    shipLinesWithSizes.Add(sizeName, shipLines);
                }
            }

            foreach (var shipLinesWithSize in shipLinesWithSizes)
            {
                starshipBaseSizes.Add(CreateStarshipBaseSizes(shipLinesWithSize));
            }

            return Task.FromResult(starshipBaseSizes);
        }

        private static StarshipBaseSize CreateStarshipBaseSizes(KeyValuePair<string, List<string>> shipLinesWithSize)
        {
            var starshipBaseSize = new StarshipBaseSize
            {
                Name = shipLinesWithSize.Key,
                PartitionKey = ContentType.Base.ToString(),
                RowKey = shipLinesWithSize.Key
            };

            var strengthNums = Regex.Matches(shipLinesWithSize.Value.Find(s => s.Contains("Strength at Tier 0")),
                @"-?\d+");
            starshipBaseSize.Strength = int.Parse(strengthNums[1].ToString());
            starshipBaseSize.StrengthModifier = int.Parse(strengthNums[2].ToString());
            var dexNums = Regex.Matches(shipLinesWithSize.Value.Find(s => s.Contains("Dexterity at Tier 0")),
                @"-?\d+");
            starshipBaseSize.Dexterity = int.Parse(dexNums[1].ToString());
            starshipBaseSize.DexterityModifier = int.Parse(dexNums[2].ToString());
            var constitutionNums = Regex.Matches(shipLinesWithSize.Value.Find(s => s.Contains("Constitution at Tier 0")),
                @"-?\d+");
            starshipBaseSize.Constitution = int.Parse(constitutionNums[1].ToString());
            starshipBaseSize.ConstitutionModifier = int.Parse(constitutionNums[2].ToString());

            var hitDice = Regex.Matches(shipLinesWithSize.Value.Find(s => s.Contains("Hit Dice at Tier 0")),
                @"\d+");
            starshipBaseSize.HitDiceNumberOfDice = int.Parse(hitDice[2].ToString());
            starshipBaseSize.HitDiceDieType = int.Parse(hitDice[1].ToString());

            var additionalHitDice = shipLinesWithSize.Value.Find(s => s.Contains("Hit Points for subsequent Hit Die:"));
            starshipBaseSize.AdditionalHitDiceText = additionalHitDice.Substring(additionalHitDice.LastIndexOf("**", StringComparison.InvariantCultureIgnoreCase))
                .Replace("**", string.Empty).Trim();

            var maxSuiteSystems = Regex.Matches(shipLinesWithSize.Value.Find(s => s.Contains("Maximum Suite Systems:")),
                @"\d+");
            starshipBaseSize.MaxSuiteSystems = maxSuiteSystems.Any() ?  int.Parse(maxSuiteSystems[0].ToString()) : 0;

            var modSlotsAtTier0 = Regex.Matches(shipLinesWithSize.Value.Find(s => s.Contains("Modification Slots at Tier 0:")),
                @"\d+");
            starshipBaseSize.ModSlotsAtTier0 = int.Parse(modSlotsAtTier0[1].ToString());

            var stockModificationsLine = shipLinesWithSize.Value.Find(s => s.Contains("Stock Modifications:"));
            var suiteChoiceSplitIndex =
                stockModificationsLine.IndexOf(", and", StringComparison.InvariantCultureIgnoreCase);

            var stockModsStartIndex = stockModificationsLine.LastIndexOf("**", StringComparison.InvariantCultureIgnoreCase);
            var stockModsEndIndex = suiteChoiceSplitIndex == -1 ? stockModificationsLine.Length : suiteChoiceSplitIndex;
            starshipBaseSize.StockModificationNames = stockModificationsLine
                .Substring(stockModsStartIndex,
                    stockModsEndIndex - stockModsStartIndex)
                .Split(", ")
                .Select(t => t.Trim().Replace("** ", string.Empty))
                .ToList();

            if (suiteChoiceSplitIndex > 0)
            {
                starshipBaseSize.StockModificationSuiteChoices = stockModificationsLine
                    .Substring(suiteChoiceSplitIndex).Trim().Split(", ")
                    .ToList();
            }

            var savingThrowsLine = shipLinesWithSize.Value.Find(s => s.Contains("Saving Throws:"));
            starshipBaseSize.SavingThrowOptions = savingThrowsLine.Split(' ')
                .Where(s => SavingThrowOptions.Contains(s)).ToList();

            var startingEquipmentLine = shipLinesWithSize.Value.Find(s => s.Contains("**Starting Equipment:**"));
            var endOfArmorChoices = startingEquipmentLine.IndexOf(';') == -1 ? startingEquipmentLine.Length : startingEquipmentLine.IndexOf(';');
            var lastIndexOfChoicePre = startingEquipmentLine.LastIndexOf(" Your choice of ", StringComparison.InvariantCultureIgnoreCase) + " Your choice of ".Length;
            starshipBaseSize.StartingEquipmentArmorChoices =
                startingEquipmentLine
                    .Substring(lastIndexOfChoicePre, endOfArmorChoices - lastIndexOfChoicePre)
                    .Split(',').Select(s => s.Replace(" or ", string.Empty).Trim()).ToList();

            if (endOfArmorChoices != startingEquipmentLine.Length)
            {
                starshipBaseSize.StartingEquipmentNonShield = startingEquipmentLine.Substring(endOfArmorChoices)
                    .Split(';')
                    .Select(s => s.Replace(" a ", string.Empty).Replace(" and ", string.Empty))
                    .ToList();
            }

            var featLines =
                shipLinesWithSize.Value.Skip(shipLinesWithSize.Value.IndexOf("<div class='classTable'>")).ToList();

            var startIndex = featLines.FindIndex(f =>
                f.Contains("Tier", StringComparison.InvariantCultureIgnoreCase) &&
                f.Contains("Features", StringComparison.InvariantCultureIgnoreCase));
            var endIndex = featLines.FindIndex(startIndex, x => x.StartsWith("</div>"));
            var featTableLines = featLines.Skip(startIndex + 2).Take(endIndex - startIndex - 3).ToList();

            var feats = new List<StarshipFeature>();
            for (var i = 0; i < featTableLines.Count; i++)
            {
                var splitFeatTable = featTableLines[i].Split('|');
                var a = splitFeatTable[2].Split(',');
                var baseFeats = a.Select(featName => new StarshipFeature
                {
                    Name = Regex.Replace(featName.Trim(), @"[^\u0000-\u007F]+", string.Empty),
                    Tier = int.Parse(Regex.Match(splitFeatTable[1], @"\d+").Value)
                }).ToList();

                feats.AddRange(baseFeats);
            }

            starshipBaseSize.Features = feats.Where(f => !string.IsNullOrEmpty(f.Name)).ToList();

            foreach (var starshipFeature in starshipBaseSize.Features)
            {
                var currentFeatNameLineIndex = featLines.IndexOf($"### {starshipFeature.Name}");
                var nextFeatNameLineIndex = featLines.IndexOf("### ", currentFeatNameLineIndex) == -1 ? featLines.Count : featLines.IndexOf("### ", currentFeatNameLineIndex);
                var starshipFeatureContentLines = featLines.Skip(currentFeatNameLineIndex)
                    .Take(nextFeatNameLineIndex - currentFeatNameLineIndex).Where(s =>
                        !string.IsNullOrWhiteSpace(s) &&
                        !s.StartsWith('\\') &&
                        !s.StartsWith('<')).ToList();
                if (starshipFeature.Name == "Starship Improvements" && starshipFeatureContentLines.Any(s => s == "#### Additional Modifications"))
                {
                    var additionalImprovementLineIndex = starshipFeatureContentLines.IndexOf("#### Additional Modifications");
                    starshipBaseSize.ModSlotsPerLevel = int.Parse(Regex
                        .Match(starshipFeatureContentLines[additionalImprovementLineIndex + 1], @"\d+").Value);
                }

                starshipFeature.Content = string.Join("\r\n", starshipFeatureContentLines);
                
            }

            return starshipBaseSize;
        }
    }
}
