using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Search
{
    public class ClassSearch : SearchBase
    {
        public string Name { get; set; }
        public ContentType? ContentType { get; set; }
    }
}
