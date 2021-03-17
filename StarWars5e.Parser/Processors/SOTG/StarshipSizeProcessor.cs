using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Starship;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Processors.SOTG
{
    public class StarshipSizeProcessor : BaseProcessor<StarshipBaseSize>
    {
        public override Task<List<StarshipBaseSize>> FindBlocks(List<string> lines)
        {
            var starshipBaseSizes = new List<StarshipBaseSize>();
            var shipLinesWithSizes = new Dictionary<string, List<string>>();

            var shipLinesStart = lines.FindIndex(f => f.StartsWith(Localization.SOTGShipSizeStartLine));
            var shipLinesEnd = lines.FindIndex(shipLinesStart, f => f == Localization.SOTGVariantStart);
            var shipSectionLines = lines.Skip(shipLinesStart).Take(shipLinesEnd - shipLinesStart).ToList();

            for (var i = 0; i < shipSectionLines.Count; i++)
            {
                if (shipSectionLines[i].StartsWith("## ", StringComparison.InvariantCultureIgnoreCase) &&
                    !shipSectionLines[i].Contains(Localization.feature, StringComparison.InvariantCultureIgnoreCase))
                {
                    var sizeName = shipSectionLines[i].Split(' ')[1];
                    var endIndex = shipSectionLines.FindIndex(i + 1,
                        x => x.StartsWith("## ", StringComparison.InvariantCultureIgnoreCase) &&
                             !x.Contains(Localization.feature, StringComparison.InvariantCultureIgnoreCase));
                    var shipLines = shipSectionLines.Skip(i)
                        .Take((endIndex == -1 ? shipSectionLines.Count - 1 : endIndex) - i).ToList();
                    shipLinesWithSizes.Add(sizeName, shipLines);
                }
            }

            foreach (var shipLinesWithSize in shipLinesWithSizes)
            {
                starshipBaseSizes.Add(CreateStarshipBaseSizes(shipLinesWithSize));
            }

            return Task.FromResult(starshipBaseSizes);
        }

        private StarshipBaseSize CreateStarshipBaseSizes(KeyValuePair<string, List<string>> shipLinesWithSize)
        {
            var starshipBaseSize = new StarshipBaseSize
            {
                Name = shipLinesWithSize.Key,
                PartitionKey = ContentType.Core.ToString(),
                RowKey = shipLinesWithSize.Key
            };

            starshipBaseSize.FullText = string.Join("\r\n", shipLinesWithSize.Value.Skip(1).CleanListOfStrings());
            var flavorLinesEnd = shipLinesWithSize.Value.FindIndex(f => f.Equals(Localization.SOTGStarshipFeatures));
            starshipBaseSize.FlavorText = string.Join("\r\n", shipLinesWithSize.Value.Skip(1).Take(flavorLinesEnd - 1).CleanListOfStrings());

            //var strengthNums = Regex.Matches(shipLinesWithSize.Value.Find(s => s.Contains(Localization.SOTGStrengthAtTierZero)),
            //    @"-?\d+");
            //starshipBaseSize.Strength = int.Parse(strengthNums[1].ToString());
            //starshipBaseSize.StrengthModifier = int.Parse(strengthNums[2].ToString());
            //var dexNums = Regex.Matches(shipLinesWithSize.Value.Find(s => s.Contains(Localization.SOTGDexterityAtTierZero)),
            //    @"-?\d+");
            //starshipBaseSize.Dexterity = int.Parse(dexNums[1].ToString());
            //starshipBaseSize.DexterityModifier = int.Parse(dexNums[2].ToString());
            //var constitutionNums = Regex.Matches(shipLinesWithSize.Value.Find(s => s.Contains(Localization.SOTGConstitutionAtTierZero)),
            //    @"-?\d+");
            //starshipBaseSize.Constitution = int.Parse(constitutionNums[1].ToString());
            //starshipBaseSize.ConstitutionModifier = int.Parse(constitutionNums[2].ToString());

            //var hitDice = Regex.Matches(shipLinesWithSize.Value.Find(s => s.Contains(Localization.SOTGHitDiceAtTierZero)),
            //    @"\d+");
            //starshipBaseSize.HitDiceNumberOfDice = int.Parse(hitDice[1].ToString());
            //starshipBaseSize.HitDiceDieTypeEnum = (DiceType)int.Parse(hitDice[2].ToString());

            //var additionalHitDice = shipLinesWithSize.Value.Find(s => s.Contains(Localization.SOTGHitPointsForSubsequentHitDie));
            //starshipBaseSize.AdditionalHitDiceText = additionalHitDice.Substring(additionalHitDice.LastIndexOf("**", StringComparison.InvariantCultureIgnoreCase))
            //    .Replace("**", string.Empty).Trim();

            //var maxSuiteSystems = Regex.Matches(shipLinesWithSize.Value.Find(s => s.Contains(Localization.SOTGMaximumSuiteSystems)),
            //    @"\d+");
            //starshipBaseSize.MaxSuiteSystems = maxSuiteSystems.Any() ?  int.Parse(maxSuiteSystems[0].ToString()) : 0;

            //var modSlotsAtTier0 = Regex.Matches(shipLinesWithSize.Value.Find(s => s.Contains(Localization.SOTGModificationSlotsAtTierZero)),
            //    @"\d+");
            //starshipBaseSize.ModSlotsAtTier0 = int.Parse(modSlotsAtTier0[1].ToString());

            //var stockModificationsLine = shipLinesWithSize.Value.Find(s => s.Contains(Localization.SOTGStockModifications));
            //var suiteChoiceSplitIndex =
            //    stockModificationsLine.IndexOf(Localization.SOTGSuiteChoiceSplitIndex, StringComparison.InvariantCultureIgnoreCase);

            //var stockModsStartIndex = stockModificationsLine.LastIndexOf("**", StringComparison.InvariantCultureIgnoreCase);
            //var stockModsEndIndex = suiteChoiceSplitIndex == -1 ? stockModificationsLine.Length : suiteChoiceSplitIndex;
            //starshipBaseSize.StockModificationNames = stockModificationsLine
            //    .Substring(stockModsStartIndex,
            //        stockModsEndIndex - stockModsStartIndex)
            //    .Split(", ")
            //    .Select(t => t.Trim().Replace("** ", string.Empty))
            //    .ToList();

            //if (suiteChoiceSplitIndex > 0)
            //{
            //    starshipBaseSize.StockModificationSuiteChoices = stockModificationsLine
            //        .Substring(suiteChoiceSplitIndex).Trim().Split(", ")
            //        .ToList();
            //}

            //var savingThrowsLine = shipLinesWithSize.Value.Find(s => s.Contains(Localization.SOTGSavingThrows));
            //starshipBaseSize.SavingThrowOptions = savingThrowsLine.Replace(",", string.Empty).Split(' ')
            //    .Where(s => Localization.ShipSavingThrowOptions.Contains(s)).ToList();

            //var startingEquipmentLine = shipLinesWithSize.Value.Find(s => s.Contains(Localization.SOTGStartingEquipment));
            //var endOfArmorChoices = startingEquipmentLine.IndexOf(';') == -1 ? startingEquipmentLine.Length : startingEquipmentLine.IndexOf(';');
            //var lastIndexOfChoicePre =
            //    startingEquipmentLine.LastIndexOf(Localization.SOTGYourChoiceOf, StringComparison.InvariantCultureIgnoreCase) +
            //    Localization.SOTGYourChoiceOf.Length;
            //starshipBaseSize.StartingEquipmentArmorChoices =
            //    startingEquipmentLine
            //        .Substring(lastIndexOfChoicePre, endOfArmorChoices - lastIndexOfChoicePre)
            //        .Split(',').Select(s => s.Replace($" {Localization.or} ", string.Empty).Trim()).ToList();

            //if (endOfArmorChoices != startingEquipmentLine.Length)
            //{
            //    starshipBaseSize.StartingEquipmentNonShield = startingEquipmentLine.Substring(endOfArmorChoices)
            //        .Split(';')
            //        .Select(s => s.Replace($" {Localization.a} ", string.Empty).Replace($" {Localization.and} ", string.Empty))
            //        .ToList();
            //}

            //var featLines =
            //    shipLinesWithSize.Value.Skip(shipLinesWithSize.Value.IndexOf(Localization.SOTGClassTableStart)).ToList();

            //var startIndex = featLines.FindIndex(f =>
            //    f.Contains(Localization.Tier, StringComparison.InvariantCultureIgnoreCase) &&
            //    f.Contains(Localization.Features, StringComparison.InvariantCultureIgnoreCase));
            //var endIndex = featLines.FindIndex(startIndex, x => x.StartsWith(Localization.SOTGClassTableEnd));
            //var featTableLines = featLines.Skip(startIndex + 2).Take(endIndex - startIndex - 3).ToList();

            //var feats = new List<StarshipFeature>();
            //for (var i = 0; i < featTableLines.Count; i++)
            //{
            //    var splitFeatTable = featTableLines[i].Split('|');
            //    var a = splitFeatTable[2].Split(',');
            //    var baseFeats = a.Select(featName => new StarshipFeature
            //    {
            //        Name = Regex.Replace(featName.Trim(), @"[^\u0000-\u007F]+", string.Empty),
            //        Tier = int.Parse(Regex.Match(splitFeatTable[1], @"\d+").Value)
            //    }).ToList();

            //    feats.AddRange(baseFeats);
            //}

            //starshipBaseSize.Features = feats.Where(f => !string.IsNullOrEmpty(f.Name)).ToList();

            //foreach (var starshipFeature in starshipBaseSize.Features)
            //{
            //    var currentFeatNameLineIndex = featLines.IndexOf($"### {starshipFeature.Name}");
            //    var nextFeatNameLineIndex = featLines.IndexOf("### ", currentFeatNameLineIndex) == -1
            //        ? featLines.Count
            //        : featLines.IndexOf("### ", currentFeatNameLineIndex);
            //    var starshipFeatureContentLines = featLines.Skip(currentFeatNameLineIndex)
            //        .Take(nextFeatNameLineIndex - currentFeatNameLineIndex).Where(s =>
            //            !string.IsNullOrWhiteSpace(s) &&
            //            !s.StartsWith('\\') &&
            //            !s.StartsWith('<')).ToList();
            //    if (starshipFeature.Name == Localization.SOTGStarshipImprovements && starshipFeatureContentLines.Any(s => s == Localization.SOTGAdditionalModifications))
            //    {
            //        var additionalImprovementLineIndex = starshipFeatureContentLines.IndexOf(Localization.SOTGAdditionalModifications);
            //        starshipBaseSize.ModSlotsPerLevel = int.Parse(Regex
            //            .Match(starshipFeatureContentLines[additionalImprovementLineIndex + 1], @"\d+").Value);
            //    }

            //    starshipFeature.Content = string.Join("\r\n", starshipFeatureContentLines);
                
            //}

            return starshipBaseSize;
        }
    }
}
