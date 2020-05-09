using Microsoft.WindowsAzure.Storage.Table;

namespace StarWars5e.Models.Lookup
{
    public class FeatureDataLU : TableEntity
    {
        public string FeatureRowKey { get; set; }
        public int Level { get; set; }
    }
}
