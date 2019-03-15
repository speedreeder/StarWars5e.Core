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
        private static readonly string[] _savingThrowOptions = {"Strength", "Dexterity", "Constitution"};

        public override async Task<List<StarshipBaseSize>> FindBlocks(List<string> lines)
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

            return starshipBaseSizes;
        }

        private static StarshipBaseSize CreateStarShipBaseSize(KeyValuePair<string, List<string>> shipLinesWithSize)
        {
            var starshipBaseSize = new StarshipBaseSize
            {
                Name = shipLinesWithSize.Key
            };

            var strengthNums = Regex.Split(shipLinesWithSize.Value.Find(s => s.Contains("Strength at Tier 0")),
                @"\-*\d+");
            starshipBaseSize.Strength = int.Parse(strengthNums[1]);
            starshipBaseSize.StrengthModifier = int.Parse(strengthNums[2]);
            var dexNums = Regex.Split(shipLinesWithSize.Value.Find(s => s.Contains("Dexterity at Tier 0")),
                @"\-*\d+");
            starshipBaseSize.Dexterity = int.Parse(dexNums[1]);
            starshipBaseSize.DexterityModifier = int.Parse(strengthNums[2]);
            var constitutionNums = Regex.Split(shipLinesWithSize.Value.Find(s => s.Contains("Constitution at Tier 0")),
                @"\-*\d+");
            starshipBaseSize.Constitution = int.Parse(constitutionNums[1]);
            starshipBaseSize.ConstitutionModifier = int.Parse(strengthNums[2]);

            var hitDice = Regex.Split(shipLinesWithSize.Value.Find(s => s.Contains("Hit Dice at Tier 0")),
                @"\d+");
            starshipBaseSize.HitDiceNumberOfDice = int.Parse(hitDice[2]);
            starshipBaseSize.HitDiceDieType = int.Parse(hitDice[1]);

            var maxSuiteSystems = Regex.Split(shipLinesWithSize.Value.Find(s => s.Contains("Maximum Suite Systems:")),
                @"\d+");
            starshipBaseSize.MaxSuiteSystems = int.Parse(maxSuiteSystems[0]);

            var modSlotsAtTier0 = Regex.Split(shipLinesWithSize.Value.Find(s => s.Contains("Modification Slots at Tier 0:")),
                @"\d+");
            starshipBaseSize.ModSlotsAtTier0 = int.Parse(modSlotsAtTier0[1]);

            var stockModificationsLine = shipLinesWithSize.Value.Find(s => s.Contains("Stock Modifications:"));
            starshipBaseSize.StockModificationNames = stockModificationsLine
                .Substring(stockModificationsLine.LastIndexOf("**", StringComparison.InvariantCultureIgnoreCase),
                    stockModificationsLine.IndexOf(", and", StringComparison.InvariantCultureIgnoreCase)).Trim().Split(", ")
                .ToList();
            starshipBaseSize.StockModificationSuiteChoices = stockModificationsLine
                .Substring(stockModificationsLine.IndexOf(", and", StringComparison.InvariantCultureIgnoreCase)).Trim().Split(", ")
                .ToList();

            var savingThrowsLine = shipLinesWithSize.Value.Find(s => s.Contains("Saving Throws:"));
            starshipBaseSize.SavingThrowOptions = savingThrowsLine.Split(' ')
                .Where(s => _savingThrowOptions.Contains(s)).ToList();

            var featLines =
                shipLinesWithSize.Value.Skip(shipLinesWithSize.Value.IndexOf("<div class='classTable'>")).ToList();

            var startIndex = featLines.FindIndex(f => f.Equals("| Tier | Features | "));
            var endindex = featLines.FindIndex(startIndex, x => x.StartsWith("</div>"));
            var featTableLines = featLines.Skip(startIndex).Take(endindex - startIndex).ToList();

            //for (var i = 0; i < featTableLines.Count; i++)
            //{
                
            //}


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
