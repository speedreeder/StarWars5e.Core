using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models.Class;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Lookup;
using StarWars5e.Models.Utils;
using StarWars5e.Parser.Processors.PHB;

namespace StarWars5e.Parser.Processors
{
    public class ExpandedContentArchetypeProcessor : BaseProcessor<Archetype>
    {
        private readonly List<ClassImageLU> _classImageLus;
        private readonly List<CasterRatioLU> _casterRatioLus;

        public ExpandedContentArchetypeProcessor(List<ClassImageLU> classImageLus, List<CasterRatioLU> casterRatioLus)
        {
            _classImageLus = classImageLus;
            _casterRatioLus = casterRatioLus;
        }
        public override Task<List<Archetype>> FindBlocks(List<string> lines)
        {
            var archetypes = new List<Archetype>();

            var tableOfContentsStart = lines.FindIndex(f => f.Contains("# Table of Contents"));
            var tableOfContentsEnd = lines.FindIndex(tableOfContentsStart + 1, f => f.StartsWith('#'));
            var tableLines = lines.Skip(tableOfContentsStart + 3).Take(tableOfContentsEnd - (tableOfContentsStart + 3))
                .Where(f => Regex.IsMatch(f, @"^\|[a-zA-Z&]")).ToList();

            var archetypeNames = tableLines.Select(t =>
            {
                t = t.Split('|')[1];
                if (t.HasLeadingHtmlWhitespace())
                {
                    return t.Trim().RemoveHtmlWhitespace();
                }

                return null;
            }).Where(t => t != null).ToList();

            var trakataIndex = archetypeNames.FindIndex(a => a.Equals("Form IX: Trakata"));
            archetypeNames[trakataIndex] = "Form IX: Tr�kata";

            var starWarsClass = "Berserker";
            foreach (var tableLine in tableLines)
            {
                var tableLineSplit = tableLine.Split('|');
                
                if (!tableLineSplit[1].HasLeadingHtmlWhitespace()) starWarsClass = tableLineSplit[1];
                else
                {
                    var archetypeName = tableLineSplit[1].Trim().RemoveHtmlWhitespace();
                    if (archetypeName == "Form IX: Trakata") archetypeName = "Form IX: Tr�kata";
                    var archetypeLinesStart = lines.FindIndex(f => f.Contains($"## {archetypeName}"));
                    
                    var archetypeLinesEnd = lines.FindIndex(archetypeLinesStart + 1, f => f.StartsWith("## ") && archetypeNames.Contains(f.Trim().Split("## ")[1]));
                    var archetypeLines = lines.Skip(archetypeLinesStart);
                    if (archetypeLinesEnd != -1)
                    {
                        archetypeLines = lines.Skip(archetypeLinesStart)
                            .Take(archetypeLinesEnd - archetypeLinesStart);
                    }

                    var playerHandbookClassProcessor = new PlayerHandbookClassProcessor(_classImageLus, _casterRatioLus);

                    archetypes.Add(playerHandbookClassProcessor.ParseArchetype(archetypeLines.ToList(), starWarsClass,
                        ContentType.ExpandedContent));
                }
            }

            return Task.FromResult(archetypes);
        }
    }
}
