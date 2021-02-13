using Microsoft.Azure.Cosmos.Table;

namespace StarWars5e.Models
{
    public class FeatureOption : TableEntity
    {
        public string Feature { get; set; }
        public string Metadata { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
    }
}
