using System;
using Microsoft.Azure.Search;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Search
{
    public class GlobalSearchTerm : TableEntity
    {
        public GlobalSearchTermType GlobalSearchTermTypeEnum { get; set; }
        public string GlobalSearchTermType {
            get => GlobalSearchTermTypeEnum.ToString();
            set => GlobalSearchTermTypeEnum = Enum.Parse<GlobalSearchTermType>(value);
        }
        public string Path { get; set; }
        public string FullName { get; set; }
        public bool IsDeleted { get; set; }
        public Language LanguageEnum { get; set; }
        public string Language
        {
            get => LanguageEnum.ToString();
            set => LanguageEnum = Enum.Parse<Language>(value);
        }

        [IsSearchable]
        public string SearchText { get; set; }
        public string SearchKey { get; set; }
    }
}
