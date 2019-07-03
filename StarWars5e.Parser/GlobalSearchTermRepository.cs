using System.Collections.Generic;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser
{
    public class GlobalSearchTermRepository
    {
        public List<GlobalSearchTerm> SearchTerms { get; set; } = new List<GlobalSearchTerm>();

        //public GlobalSearchTerm ParseSearchTerm(string line, GlobalSearchTermType type, ContentType contentType,
        //    string section)
        //{
        //    var parsed = line.RemoveMarkdownCharacters().Replace("#", "").Trim();
        //    var name = parsed;
        //    if (!string.IsNullOrWhiteSpace(section))
        //    {
        //        name = $"{section} - {parsed}";
        //    }

        //    var searchTerm = new GlobalSearchTerm
        //    {
        //        PartitionKey = contentType.ToString(),
        //        RowKey = name,
        //        GlobalSearchTermTypeEnum = type,
        //        FullName = $"{type.ToString().SplitPascalCase()}: {name}",
        //        Path = parsed.ToKebabCase()
        //    };
        //    return searchTerm;
        //}

        public GlobalSearchTerm CreateSearchTerm(string name, GlobalSearchTermType type, ContentType contentType,
            string path)
        {
            var searchTerm = new GlobalSearchTerm
            {
                PartitionKey = contentType.ToString(),
                RowKey = path.ToKebabCase(),
                GlobalSearchTermTypeEnum = type,
                FullName = $"{type.ToString().SplitPascalCase()}: {name}",
                Path = path,
                SearchText = name
            };
            return searchTerm;
        }

        public GlobalSearchTerm CreateSectionSearchTermFromName(string name, GlobalSearchTermType type,
            ContentType contentType, string section, string path, string pathOverride)
        {
            var key = path + "#" + name.ToKebabCase();
            if (!string.IsNullOrWhiteSpace(pathOverride))
            {
                key = path + "#" + pathOverride.ToKebabCase();
            }

            var nameWithOptionalSection = name;
            if (!string.IsNullOrWhiteSpace(section))
            {
                nameWithOptionalSection = $"{section} - {name}";
            }

            var searchTerm = new GlobalSearchTerm
            {
                PartitionKey = contentType.ToString(),
                RowKey = key.ToKebabCase(),
                GlobalSearchTermTypeEnum = type,
                FullName = $"{type.ToString().SplitPascalCase()}: {nameWithOptionalSection}",
                Path = key,
                SearchText = name
            };
            return searchTerm;
        }

    }
}
