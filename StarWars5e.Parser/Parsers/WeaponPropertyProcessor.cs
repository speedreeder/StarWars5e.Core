using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Parsers
{
    public class WeaponPropertyProcessor : BaseProcessor<WeaponProperty>
    {
        private readonly ContentType _contentType;
        private readonly Dictionary<string, string> _nameStartingLines;

        public WeaponPropertyProcessor(ContentType contentType, Dictionary<string, string> nameStartingLines)
        {
            _contentType = contentType;
            _nameStartingLines = nameStartingLines;
        }
        public override Task<List<WeaponProperty>> FindBlocks(List<string> lines)
        {
            var weaponProperties = new List<WeaponProperty>();

            foreach (var nameStartingLine in _nameStartingLines)
            {
                var occurence = 1;
                if (nameStartingLine.Key == "Strength") occurence = 2;
                weaponProperties.Add(ParseProperty(lines, nameStartingLine.Value, nameStartingLine.Key, _contentType, occurence));
            }
            
            return Task.FromResult(weaponProperties);
        }

        private static WeaponProperty ParseProperty(List<string> lines, string startLine, string name, ContentType contentType, int occurence)
        {
            try
            {
                var weaponPropertyStart = lines.FindNthIndex(f => f.RemoveHtmlWhitespace().StartsWith(startLine), occurence);
                var weaponPropertyStartEnd =
                    lines.FindIndex(weaponPropertyStart, string.IsNullOrWhiteSpace);
                var weaponPropertyLines = lines.Skip(weaponPropertyStart)
                    .Take(weaponPropertyStartEnd - weaponPropertyStart).CleanListOfStrings()
                    .RemoveEmptyLines();

                return new WeaponProperty(name, string.Join("\r\n", weaponPropertyLines), contentType);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing weapon property {name}", e);
            }
        }
    }
}
