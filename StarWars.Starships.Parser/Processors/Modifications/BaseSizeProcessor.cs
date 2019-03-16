using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models.Starship;

namespace StarWars.Starships.Parser.Processors.Modifications
{
    public class BaseSizeProcessor : StarshipBaseProcessor<StarshipBaseSize>
    {
        private static readonly string[] SavingThrowOptions = {"Strength", "Dexterity", "Constitution"};

        public override Task<List<StarshipBaseSize>> FindBlocks(List<string> lines)
        {
            var starshipBaseSizes = new List<StarshipBaseSize>();
            var shipLinesWithSizes = new Dictionary<string, List<string>>();

            for (var i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("## ", StringComparison.InvariantCultureIgnoreCase) && !lines[i].Contains("feature", StringComparison.InvariantCultureIgnoreCase))
                {
                    var sizeName = lines[i].Split(' ')[1];
                    var endIndex = lines.FindIndex(i + 1, x => x.StartsWith("## ", StringComparison.InvariantCultureIgnoreCase) && !x.Contains("feature", StringComparison.InvariantCultureIgnoreCase));
                    var shipLines = lines.Skip(i).Take((endIndex == -1 ? lines.Count - 1 : endIndex) - i).ToList();
                    shipLinesWithSizes.Add(sizeName, shipLines);
                }
            }

            foreach (var shipLinesWithSize in shipLinesWithSizes)
            {
                starshipBaseSizes.Add(CreateStarShipBaseSize(shipLinesWithSize));
            }

            return Task.FromResult(starshipBaseSizes);
        }

        private static StarshipBaseSize CreateStarShipBaseSize(KeyValuePair<string, List<string>> shipLinesWithSize)
        {
            var starshipBaseSize = new StarshipBaseSize
            {
                Name = shipLinesWithSize.Key
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

            var maxSuiteSystems = Regex.Matches(shipLinesWithSize.Value.Find(s => s.Contains("Maximum Suite Systems:")),
                @"\d+");
            starshipBaseSize.MaxSuiteSystems = maxSuiteSystems.Any() ?  int.Parse(maxSuiteSystems[0].ToString()) : 0;

            var modSlotsAtTier0 = Regex.Matches(shipLinesWithSize.Value.Find(s => s.Contains("Modification Slots at Tier 0:")),
                @"\d+");
            starshipBaseSize.ModSlotsAtTier0 = int.Parse(modSlotsAtTier0[1].ToString());

            var stockModificationsLine = shipLinesWithSize.Value.Find(s => s.Contains("Stock Modifications:"));
            var suiteChoiceSplitIndex =
                stockModificationsLine.IndexOf(", and", StringComparison.InvariantCultureIgnoreCase);

            //var c = stockModificationsLine.LastIndexOf("**", StringComparison.InvariantCultureIgnoreCase);
            //var b = suiteChoiceSplitIndex == -1 ? stockModificationsLine.Length : suiteChoiceSplitIndex;
            //var a = stockModificationsLine
            //    .Substring(c,
            //        b - c);
            //    var e = a.Trim()
            //    .Split(", ")
            //    .ToList();
            //stockModificationsLine

            starshipBaseSize.StockModificationSuiteChoices = stockModificationsLine
                .Substring(stockModificationsLine.IndexOf(", and", StringComparison.InvariantCultureIgnoreCase)).Trim().Split(", ")
                .ToList();

            var savingThrowsLine = shipLinesWithSize.Value.Find(s => s.Contains("Saving Throws:"));
            starshipBaseSize.SavingThrowOptions = savingThrowsLine.Split(' ')
                .Where(s => SavingThrowOptions.Contains(s)).ToList();

            var featLines =
                shipLinesWithSize.Value.Skip(shipLinesWithSize.Value.IndexOf("<div class='classTable'>")).ToList();

            var startIndex = featLines.FindIndex(f => f.Equals("| Tier | Features | "));
            var endIndex = featLines.FindIndex(startIndex, x => x.StartsWith("</div>"));
            var featTableLines = featLines.Skip(startIndex).Take(endIndex - startIndex).ToList();

            var feats = new List<StarshipFeature>();
            for (var i = 0; i < featTableLines.Count; i++)
            {
                var splitFeatTable = featTableLines[i].Split('|');
                feats = splitFeatTable[2].Split(',').Select(featNames => new StarshipFeature
                {
                    Name = featNames.Trim(),
                    Tier = int.Parse(splitFeatTable[1])
                }).ToList();

            }

            starshipBaseSize.Features = feats;


            //for (var i = 0; i < shipLinesWithSize.Value.Count; i++)
            //{
            //    if (shipLinesWithSize.Value[i].StartsWith("## ", StringComparison.InvariantCultureIgnoreCase) && !shipLinesWithSize.Value[i].Contains("feature", StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        var sizeName = shipLinesWithSize.Value[i].Split(' ')[1];
            //        var endIndex = shipLinesWithSize.Value.FindIndex(i + 1, x => x.StartsWith("## ", StringComparison.InvariantCultureIgnoreCase) && !x.Contains("feature", StringComparison.InvariantCultureIgnoreCase));
            //        var shipLines = shipLinesWithSize.Value.Skip(i).Take((endIndex == -1 ? lines.Count - 1 : endIndex) - i).ToList();
            //        shipLinesWithSizes.Add(sizeName, shipLines);
            //    }
            //}

            return starshipBaseSize;
        }
    }
}
