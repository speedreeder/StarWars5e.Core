using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Search
{
    public class ArchetypeSearch : SearchBase
    {
        public ArchetypeSearchOrdering ArchetypeSearchOrdering { get; set; } = ArchetypeSearchOrdering.None;
        public string Name { get; set; }
        public ContentType? ContentType { get; set; }
        public string Class { get; set; }
    }

    public enum ArchetypeSearchOrdering
    {
        None,
        NameAscending,
        NameDescending,
        ContentTypeAscending,
        ContentTypeDescending,
        ClassAscending,
        ClassDescending
    }
}
