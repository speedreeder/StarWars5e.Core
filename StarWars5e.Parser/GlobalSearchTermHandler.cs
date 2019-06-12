using System.Collections.Generic;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser
{
    public static class GlobalSearchTermHandler
    {
        public static List<GlobalSearchTerm> SearchTerms { get; set; }

        public static void ParseSearchTerm(string line, GlobalSearchTermType type)
        {
            var parsed = line.RemoveMarkdownCharacters().Replace("#", "").Trim();
            var searchTerm = new GlobalSearchTerm
            {
                GlobalSearchTermType = type,
                Name = parsed,
                FullName = $"{type.ToString().SplitPascalCase()}: {parsed}",
                Path = parsed.ToKebabCase()
            };
            SearchTerms.Add(searchTerm);
        }

    }
}
