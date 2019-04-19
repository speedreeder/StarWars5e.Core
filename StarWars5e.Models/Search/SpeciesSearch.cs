using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Search
{
    public class SpeciesSearch : SearchBase
    {
        public SpeciesSearchOrdering SpeciesSearchOrdering { get; set; } = SpeciesSearchOrdering.None;
        public string Name { get; set; }
        public ContentType? ContentType { get; set; }
    }

    public enum SpeciesSearchOrdering
    {
        None,
        NameAscending,
        NameDescending,
        ContentTypeAscending,
        ContentTypeDescending
    }
}
