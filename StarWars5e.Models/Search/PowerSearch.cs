using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Search
{
    public class PowerSearch : SearchBase
    {
        public string Name { get; set; }
        public ContentType? ContentType { get; set; }
        public int? MaxLevel { get; set; }
        public int? MinLevel { get; set; }
        public PowerType? PowerType { get; set; }
        public ForceAlignment? ForceAlignment { get; set; }
        public bool? IsConcentration { get; set; }
    }
}
