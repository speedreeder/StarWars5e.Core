using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Search
{
    public class ClassSearch : SearchBase
    {
        public ClassSearchOrdering ClassSearchOrdering { get; set; } = ClassSearchOrdering.None;
        public string Name { get; set; }
        public ContentType? ContentType { get; set; }
    }

    public enum ClassSearchOrdering
    {
        None,
        NameAscending,
        NameDescending,
        ContentTypeAscending,
        ContentTypeDescending
    }
}
