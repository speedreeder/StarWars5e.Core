using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;
using StarWars5e.Parser.Processors.PHB;

namespace StarWars5e.Parser.Processors
{
    public class ExpandedContentCustomizationOptionsLightsaberFormsProcessor : BaseProcessor<LightsaberForm>
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
                var lightsaberFormEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("#"));

                var lightsaberFormLines = lines.Skip(lightsaberFormStartIndex).Take(lightsaberFormsEndIndex - lightsaberFormStartIndex);
                if (lightsaberFormEndIndex != -1 && lightsaberFormEndIndex < lightsaberFormsEndIndex)
                {
                    lightsaberFormLines = lines.Skip(lightsaberFormStartIndex).Take(lightsaberFormEndIndex - lightsaberFormStartIndex);
                }

                var playerHandbookCustomizationOptionsLightsaberFormsProcessor = new PlayerHandbookCustomizationOptionsLightsaberFormsProcessor();
                var lightsaberForm = playerHandbookCustomizationOptionsLightsaberFormsProcessor.ParseLightsaberForm(lightsaberFormLines.ToList(), contentType);
                lightsaberForms.Add(lightsaberForm);
            }

            return Task.FromResult(lightsaberForms);
        }
    }
}
