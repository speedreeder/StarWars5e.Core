using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models.Class;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;
using StarWars5e.Parser.Parsers.PHB;

namespace StarWars5e.Parser.Parsers
{
    public class ExpandedContentArchetypeProcessor : BaseProcessor<Archetype>
    {
        public override Task<List<Archetype>> FindBlocks(List<string> lines)
        {
            var archetypes = new List<Archetype>();

            var tableOfContentsStart = lines.FindIndex(f => f.Contains("# Table of Contents"));
            var tableOfContentsEnd = lines.FindIndex(tableOfContentsStart + 1, f => f.StartsWith('#'));
            var tableLines = lines.Skip(tableOfContentsStart + 3).Take(tableOfContentsEnd - (tableOfContentsStart + 3))
                .Where(f => Regex.IsMatch(f, @"^\|[a-zA-Z&]"));

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
                    
                    var archetypeLinesEnd = lines.FindIndex(archetypeLinesStart + 1, f => f.StartsWith("## "));
                    var archetypeLines = lines.Skip(archetypeLinesStart);
                    if (archetypeLinesEnd != -1)
                    {
                        archetypeLines = lines.Skip(archetypeLinesStart)
                            .Take(archetypeLinesEnd - archetypeLinesStart);
                    }

                    archetypes.Add(PlayerHandbookClassProcessor.ParseArchetype(archetypeLines.ToList(), starWarsClass,
                        ContentType.ExpandedContent));
                }
            }

            return Task.FromResult(archetypes);
        }
    }
}
