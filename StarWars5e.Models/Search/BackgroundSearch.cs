using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Search
{
    public class BackgroundSearch : SearchBase
    {
        public BackgroundSearchOrdering BackgroundSearchOrdering { get; set; } = BackgroundSearchOrdering.None;
        public string Name { get; set; }
        public ContentType? ContentType { get; set; }
    }

    public enum BackgroundSearchOrdering
    {
        None,
        NameAscending,
        NameDescending,
        ContentTypeAscending,
        ContentTypeDescending
    }
}
