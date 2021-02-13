using Microsoft.Azure.Cosmos.Table;

namespace StarWars5e.Models.Lookup
{
    public class SpeciesImageUrlLU : TableEntity
    {
        public string Specie { get; set; }
        public string Url { get; set; }
    }
}
