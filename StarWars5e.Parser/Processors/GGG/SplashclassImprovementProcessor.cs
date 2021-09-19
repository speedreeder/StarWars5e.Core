using StarWars5e.Models.CustomizationOptions;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarWars5e.Parser.Processors.GGG
{
    public class SplashclassImprovementProcessor : BaseProcessor<SplashclassImprovement>
    {
        public override Task<List<SplashclassImprovement>> FindBlocks(List<string> lines, ContentType contentType)
        {
            var splashclassImprovements = new List<SplashclassImprovement>();
            lines = lines.CleanListOfStrings().ToList();

            var splashclassImprovementsStart = lines.FindIndex(f => f.StartsWith("## Splashclass Improvements"));
            var splashclassImprovementsTempEndIndex = lines.FindIndex(splashclassImprovementsStart + 1, f => f.StartsWith("## "));
            var splashclassImprovementsEndIndex = splashclassImprovementsTempEndIndex != -1 ? splashclassImprovementsTempEndIndex : lines.Count;

            for (var i = splashclassImprovementsStart; i < splashclassImprovementsEndIndex; i++)
            {
                if (!lines[i].StartsWith("#### ")) continue;

                var splashclassImprovementStartIndex = i;
                var splashclassImprovementEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("#### "));

                var splashclassImprovementLines = lines.Skip(splashclassImprovementStartIndex).Take(splashclassImprovementsEndIndex - splashclassImprovementStartIndex);
                if (splashclassImprovementEndIndex != -1 && splashclassImprovementEndIndex < splashclassImprovementsEndIndex)
                {
                    splashclassImprovementLines = lines.Skip(splashclassImprovementStartIndex).Take(splashclassImprovementEndIndex - splashclassImprovementStartIndex);
                }

                var splashclassImprovement = ParseSplashclassImprovement(splashclassImprovementLines.ToList(), contentType);
                splashclassImprovements.Add(splashclassImprovement);
            }

            return Task.FromResult(splashclassImprovements);
        }

        public SplashclassImprovement ParseSplashclassImprovement(List<string> splashclassImprovementLines, ContentType contentType)
        {
            var name = splashclassImprovementLines[0].Split("####")[1].Trim();
            try
            {
                var splashclassImprovement = new SplashclassImprovement
                {
                    ContentTypeEnum = contentType,
                    Name = name,
                    PartitionKey = contentType.ToString(),
                    RowKey = name,
                };

                var prerequisiteIndex =
                    splashclassImprovementLines.FindIndex(f =>
                        f.StartsWith($"*{Localization.Prerequisite}") || f.StartsWith($"{Localization.Prerequisite}") ||
                        f.StartsWith($"_**{Localization.Prerequisite}") || f.StartsWith($"_*{Localization.Prerequisite}"));

                if (prerequisiteIndex != -1)
                {
                    splashclassImprovement.Prerequisite = splashclassImprovementLines[prerequisiteIndex].Split(':')[1].Replace("*", string.Empty).Replace("_", string.Empty)
                        .RemoveHtmlWhitespace().Trim();
                    splashclassImprovement.Description = string.Join("\r\n", splashclassImprovementLines.Skip(prerequisiteIndex + 1).ToList());
                }
                else
                {
                    splashclassImprovement.Description = string.Join("\r\n", splashclassImprovementLines.Skip(1).ToList());
                }

                return splashclassImprovement;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing splashclass improvement {name}", e);
            }
        }
    }
}
