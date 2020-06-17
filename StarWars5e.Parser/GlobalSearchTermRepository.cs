using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser
{
    public class GlobalSearchTermRepository
    {
        public List<GlobalSearchTerm> SearchTerms { get; set; } = new List<GlobalSearchTerm>();

        public GlobalSearchTerm CreateSearchTerm(string name, GlobalSearchTermType type, ContentType contentType,
            string path)
        {
            var searchTerm = new GlobalSearchTerm
            {
                PartitionKey = contentType.ToString(),
                RowKey = Guid.NewGuid().ToString(),
                GlobalSearchTermTypeEnum = type,
                FullName = $"{type.ToString().SplitPascalCase()}: {name}",
                Path = path,
                SearchText = name,
                IsDeleted = false
            };
            searchTerm.SearchKey = Regex.Replace(searchTerm.FullName, @"[^a-zA-Z0-9-=_]", "");
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
                RowKey = Guid.NewGuid().ToString(),
                GlobalSearchTermTypeEnum = type,
                FullName = $"{type.ToString().SplitPascalCase()}: {nameWithOptionalSection}",
                Path = key,
                SearchText = name,
                IsDeleted = false
            };
            searchTerm.SearchKey = Regex.Replace(searchTerm.FullName, @"[^a-zA-Z0-9-=_]", "");
            return searchTerm;
        }

    }
}
