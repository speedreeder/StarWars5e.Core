using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Processors
{
    public class ArmorPropertyProcessor : BaseProcessor<ArmorProperty>
    {
        private readonly ContentType _contentType;
        private readonly List<(string name, string startLine, int occurence)> _nameStartingLines;

        public ArmorPropertyProcessor(ContentType contentType, List<(string name, string startLine, int occurence)> nameStartingLines)
        {
            _contentType = contentType;
            _nameStartingLines = nameStartingLines;
        }

        public override Task<List<ArmorProperty>> FindBlocks(List<string> lines)
        {
            var armorProperties = new List<ArmorProperty>();

            foreach (var nameStartingLine in _nameStartingLines)
            {
                armorProperties.Add(ParseProperty(lines, nameStartingLine.startLine, nameStartingLine.name, _contentType, nameStartingLine.occurence));
            }
            
            return Task.FromResult(armorProperties);
        }

        private static ArmorProperty ParseProperty(List<string> lines, string startLine, string name, ContentType contentType, int occurence)
        {
            try
            {
                var armorPropertyStart = lines.FindNthIndex(f => f.RemoveHtmlWhitespace().StartsWith(startLine, StringComparison.InvariantCultureIgnoreCase), occurence);
                var armorPropertyStartEnd =
                    lines.FindIndex(armorPropertyStart, string.IsNullOrWhiteSpace);
                var armorPropertyLines = lines.Skip(armorPropertyStart)
                    .Take(armorPropertyStartEnd - armorPropertyStart).CleanListOfStrings()
                    .RemoveEmptyLines();

                return new ArmorProperty(name, string.Join("\r\n", armorPropertyLines), contentType);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing armor property {name}", e);
            }
        }
    }
}
