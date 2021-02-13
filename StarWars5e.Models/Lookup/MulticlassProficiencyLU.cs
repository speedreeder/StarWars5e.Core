using Microsoft.Azure.Cosmos.Table;

namespace StarWars5e.Models.Lookup
{
    public class MulticlassProficiencyLU : TableEntity
    {
        public string Class { get; set; }
        public string Proficiency { get; set; }
    }
}
