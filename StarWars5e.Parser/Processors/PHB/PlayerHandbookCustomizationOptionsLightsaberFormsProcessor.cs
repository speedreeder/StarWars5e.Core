using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Processors.PHB
{
    public class PlayerHandbookCustomizationOptionsLightsaberFormsProcessor : BaseProcessor<LightsaberForm>
    {
        public override Task<List<LightsaberForm>> FindBlocks(List<string> lines, ContentType contentType)
        {
            var lightsaberForms = new List<LightsaberForm>();
            lines = lines.CleanListOfStrings().ToList();

            var lightsaberFormsStart = lines.FindIndex(f => f.StartsWith("## Lightsaber Forms"));
            var lightsaberFormsTempEndIndex = lines.FindIndex(lightsaberFormsStart + 1, f => f.StartsWith("## "));
            var lightsaberFormsEndIndex = lightsaberFormsTempEndIndex != -1 ? lightsaberFormsTempEndIndex : lines.Count;

            for (var i = lightsaberFormsStart; i < lightsaberFormsEndIndex; i++)
            {
                if (!lines[i].StartsWith("#### ")) continue;

                var lightsaberFormStartIndex = i;
                var lightsaberFormEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("#### "));

                var lightsaberFormLines = lines.Skip(lightsaberFormStartIndex).Take(lightsaberFormsEndIndex - lightsaberFormStartIndex);
                if (lightsaberFormEndIndex != -1 && lightsaberFormEndIndex < lightsaberFormsEndIndex)
                {
                    lightsaberFormLines = lines.Skip(lightsaberFormStartIndex).Take(lightsaberFormEndIndex - lightsaberFormStartIndex);
                }

                var lightsaberForm = ParseLightsaberForm(lightsaberFormLines.ToList(), contentType);
                lightsaberForms.Add(lightsaberForm);
            }

            return Task.FromResult(lightsaberForms);
        }

        public LightsaberForm ParseLightsaberForm(List<string> lightsaberFormLines, ContentType contentType)
        {
            var name = lightsaberFormLines[0].Split("####")[1].Trim();
            try
            {
                var lightsaberForm = new LightsaberForm
                {
                    ContentTypeEnum = contentType,
                    Name = name,
                    PartitionKey = contentType.ToString(),
                    RowKey = name.Replace("/", string.Empty).Replace(@"\", string.Empty),
                    Description = string.Join("\r\n", lightsaberFormLines.Skip(1).ToList())
                };

                return lightsaberForm;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing lightsaber form {name}", e);
            }
        }
    }
}
