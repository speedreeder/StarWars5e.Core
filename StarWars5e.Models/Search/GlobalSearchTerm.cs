using System;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Search
{
    public class GlobalSearchTerm : TableEntity
    {
        public string Name { get; set; }
        public GlobalSearchTermType GlobalSearchTermTypeEnum { get; set; }
        public string GlobalSearchTermType {
            get => GlobalSearchTermTypeEnum.ToString();
            set => GlobalSearchTermTypeEnum = Enum.Parse<GlobalSearchTermType>(value);
        }
        public string Path { get; set; }
        public string FullName { get; set; }
    }
}
