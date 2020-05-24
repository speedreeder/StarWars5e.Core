using Microsoft.WindowsAzure.Storage.Table;

namespace StarWars5e.Models.Lookup
{
    public class SpeciesImageUrlLU : TableEntity
    {
        public string Specie { get; set; }
        public string Url { get; set; }
    }
}
