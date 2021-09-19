using StarWars5e.Models.CustomizationOptions;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarWars5e.Parser.Processors.GGG
{
    public class MulticlassImprovementProcessor : BaseProcessor<MulticlassImprovement>
    {
        public override Task<List<MulticlassImprovement>> FindBlocks(List<string> lines, ContentType contentType)
        {
            var multiclassImprovements = new List<MulticlassImprovement>();
            lines = lines.CleanListOfStrings().ToList();

            var multiclassImprovementsStart = lines.FindIndex(f => f.StartsWith("## Multiclass Improvements"));
            var multiclassImprovementsTempEndIndex = lines.FindIndex(multiclassImprovementsStart + 1, f => f.StartsWith("## "));
            var multiclassImprovementsEndIndex = multiclassImprovementsTempEndIndex != -1 ? multiclassImprovementsTempEndIndex : lines.Count;

            for (var i = multiclassImprovementsStart; i < multiclassImprovementsEndIndex; i++)
            {
                if (!lines[i].StartsWith("#### ")) continue;

                var multiclassImprovementStartIndex = i;
                var multiclassImprovementEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("#### "));

                var multiclassImprovementLines = lines.Skip(multiclassImprovementStartIndex).Take(multiclassImprovementsEndIndex - multiclassImprovementStartIndex);
                if (multiclassImprovementEndIndex != -1 && multiclassImprovementEndIndex < multiclassImprovementsEndIndex)
                {
                    multiclassImprovementLines = lines.Skip(multiclassImprovementStartIndex).Take(multiclassImprovementEndIndex - multiclassImprovementStartIndex);
                }

                var multiclassImprovement = ParseMulticlassImprovement(multiclassImprovementLines.ToList(), contentType);
                multiclassImprovements.Add(multiclassImprovement);
            }

            return Task.FromResult(multiclassImprovements);
        }

        public MulticlassImprovement ParseMulticlassImprovement(List<string> multiclassImprovementLines, ContentType contentType)
        {
            var name = multiclassImprovementLines[0].Split("####")[1].Trim();
            try
            {
                var multiclassImprovement = new MulticlassImprovement
                {
                    ContentTypeEnum = contentType,
                    Name = name,
                    PartitionKey = contentType.ToString(),
                    RowKey = name,
                };

                var prerequisiteIndex =
                    multiclassImprovementLines.FindIndex(f =>
                        f.StartsWith($"*{Localization.Prerequisite}") || f.StartsWith($"{Localization.Prerequisite}") ||
                        f.StartsWith($"_**{Localization.Prerequisite}") || f.StartsWith($"_*{Localization.Prerequisite}"));

                if (prerequisiteIndex != -1)
                {
                    multiclassImprovement.Prerequisite = multiclassImprovementLines[prerequisiteIndex].Split(':')[1].Replace("*", string.Empty).Replace("_", string.Empty)
                        .RemoveHtmlWhitespace().Trim();
                    multiclassImprovement.Description = string.Join("\r\n", multiclassImprovementLines.Skip(prerequisiteIndex + 1).ToList());
                }
                else
                {
                    multiclassImprovement.Description = string.Join("\r\n", multiclassImprovementLines.Skip(1).ToList());
                }

                return multiclassImprovement;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing multiclass improvement {name}", e);
            }
        }
    }
}
