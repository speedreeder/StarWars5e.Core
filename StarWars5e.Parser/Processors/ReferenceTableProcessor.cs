using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Processors
{
    public class ReferenceTableProcessor : BaseProcessor<ReferenceTable>
    {
        public override Task<List<ReferenceTable>> FindBlocks(List<string> lines)
        {
            var referenceTables = new List<ReferenceTable>
            {
                //PHB
                ParseTable(lines, Localization.ReferenceTableStartingLineAbilityScorePointCost, Localization.ReferenceTableNameAbilityScorePointCost, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineAbilityScoresAndModifiers, Localization.ReferenceTableNameAbilityScoresAndModifiers, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineXPByLevel, Localization.ReferenceTableNameXPByLevel, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineLifestyleExpenses, Localization.ReferenceTableNameLifestyleExpenses, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineMulticlassingPrerequisites, Localization.ReferenceTableNameMulticlassingPrerequisites, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineMulticlassingProficiencies, Localization.ReferenceTableNameMulticlassingProficiencies, ContentType.Core),
                
                //SOTG
                ParseTable(lines, Localization.ReferenceTableStartingLineDeploymentRankPrestige, Localization.ReferenceTableNameDeploymentRankPrestige, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeStockCost, Localization.ReferenceTableNameStarshipSizeStockCost, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeBuildingWorkforce, Localization.ReferenceTableNameStarshipSizeBuildingWorkforce, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineBaseUpgradeCostByTier, Localization.ReferenceTableNameBaseUpgradeCostByTier, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeUpgradeCost, Localization.ReferenceTableNameStarshipSizeUpgradeCost, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeUpgradeWorkforce, Localization.ReferenceTableNameStarshipSizeUpgradeWorkforce, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineModificationCategoryBaseCost, Localization.ReferenceTableNameModificationCategoryBaseCost, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeModificationCost, Localization.ReferenceTableNameStarshipSizeModificationCost, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeModificationWorkforce, Localization.ReferenceTableNameStarshipSizeModificationWorkforce, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineModificationCapacityByShipSize, Localization.ReferenceTableNameModificationCapacityByShipSize, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineModificationGradeInstallationByShipTier, Localization.ReferenceTableNameModificationGradeInstallationByShipTier, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeMaximumSuites, Localization.ReferenceTableNameStarshipSizeMaximumSuites, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeSuiteCapacity, Localization.ReferenceTableNameStarshipSizeSuiteCapacity, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeEquipmentCost, Localization.ReferenceTableNameStarshipSizeEquipmentCost, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeEquipmentWorkforce, Localization.ReferenceTableNameStarshipSizeEquipmentWorkforce, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeCargoCapacity, Localization.ReferenceTableNameStarshipSizeCargoCapacity, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeFuelCost, Localization.ReferenceTableNameStarshipSizeFuelCost, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeFuelCapacity, Localization.ReferenceTableNameStarshipSizeFuelCapacity, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeFoodCapacity, Localization.ReferenceTableNameStarshipSizeFoodCapacity, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeBaseTurningSpeed, Localization.ReferenceTableNameStarshipSizeBaseTurningSpeed, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineBaseHyperspaceTravelTimes, Localization.ReferenceTableNameBaseHyperspaceTravelTimes, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineHyperspaceMishaps, Localization.ReferenceTableNameHyperspaceMishaps, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeMinimumCrew, Localization.ReferenceTableNameStarshipSizeMinimumCrew, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeRefittingTime, Localization.ReferenceTableNameStarshipSizeRefittingTime, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSizeCategories, Localization.ReferenceTableNameStarshipSizeCategories, ContentType.Core),
                ParseTable(lines, Localization.ReferenceTableStartingLineStarshipSlowedLevel, Localization.ReferenceTableNameStarshipSlowedLevel, ContentType.Core)
            };

            return Task.FromResult(referenceTables);
        }

        private static ReferenceTable ParseTable(List<string> lines, string startLine, string name, ContentType contentType, int occurence = 1)
        {
            try
            {
                var referenceTableStart = lines.FindNthIndex(f => f.RemoveHtmlWhitespace().StartsWith(startLine), occurence);
                var referenceTableEnd =
                    lines.FindIndex(referenceTableStart + 3, string.IsNullOrWhiteSpace);
                var referenceTableLines = lines.Skip(referenceTableStart)
                    .Take(referenceTableEnd - referenceTableStart).Where(r => !r.StartsWith("#")).CleanListOfStrings()
                    .RemoveEmptyLines();

                return new ReferenceTable(name, string.Join("\r\n", referenceTableLines), contentType);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing reference table {name}", e);
            }
        }
    }
}
