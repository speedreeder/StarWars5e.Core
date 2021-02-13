using System;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models
{
    public class Feature : TableEntity
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public int? Level { get; set; }
        public FeatureSource SourceEnum { get; set; }
        public string Source
        {
            get => SourceEnum.ToString();
            set => SourceEnum = Enum.Parse<FeatureSource>(value);
        }
        public string SourceName { get; set; }
        public string Metadata { get; set; }
    }
}
