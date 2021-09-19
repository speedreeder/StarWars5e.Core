using StarWars5e.Models.CustomizationOptions;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarWars5e.Parser.Processors.GGG
{
    public class ClassImprovementProcessor : BaseProcessor<ClassImprovement>
    {
        public override Task<List<ClassImprovement>> FindBlocks(List<string> lines, ContentType contentType)
        {
            var classImprovements = new List<ClassImprovement>();
            lines = lines.CleanListOfStrings().ToList();

            var classImprovementsStart = lines.FindIndex(f => f.StartsWith("## Class Improvements"));
            var classImprovementsTempEndIndex = lines.FindIndex(classImprovementsStart + 1, f => f.StartsWith("## "));
            var classImprovementsEndIndex = classImprovementsTempEndIndex != -1 ? classImprovementsTempEndIndex : lines.Count;

            for (var i = classImprovementsStart; i < classImprovementsEndIndex; i++)
            {
                if (!lines[i].StartsWith("#### ")) continue;

                var classImprovementStartIndex = i;
                var classImprovementEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("#### "));
                    
                var classImprovementLines = lines.Skip(classImprovementStartIndex).Take(classImprovementsEndIndex - classImprovementStartIndex);
                if (classImprovementEndIndex != -1 && classImprovementEndIndex < classImprovementsEndIndex)
                {
                    classImprovementLines = lines.Skip(classImprovementStartIndex).Take(classImprovementEndIndex - classImprovementStartIndex);
                }

                var classImprovement = ParseClassImprovement(classImprovementLines.ToList(), contentType);
                classImprovements.Add(classImprovement);
            }

            return Task.FromResult(classImprovements);
        }

        public ClassImprovement ParseClassImprovement(List<string> classImprovementLines, ContentType contentType)
        {
            var name = classImprovementLines[0].Split("####")[1].Trim();
            try
            {
                var classImprovement = new ClassImprovement
                {
                    ContentTypeEnum = contentType,
                    Name = name,
                    PartitionKey = contentType.ToString(),
                    RowKey = name,
                };

                var prerequisiteIndex =
                    classImprovementLines.FindIndex(f =>
                        f.StartsWith($"*{Localization.Prerequisite}") || f.StartsWith($"{Localization.Prerequisite}") ||
                        f.StartsWith($"_**{Localization.Prerequisite}") || f.StartsWith($"_*{Localization.Prerequisite}"));

                if (prerequisiteIndex != -1)
                {
                    classImprovement.Prerequisite = classImprovementLines[prerequisiteIndex].Split(':')[1].Replace("*", string.Empty).Replace("_", string.Empty)
                        .RemoveHtmlWhitespace().Trim();
                    classImprovement.Description = string.Join("\r\n", classImprovementLines.Skip(prerequisiteIndex + 1).ToList());
                }
                else
                {
                    classImprovement.Description = string.Join("\r\n", classImprovementLines.Skip(1).ToList());
                }

                return classImprovement;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing class improvement {name}", e);
            }
        }
    }
}
