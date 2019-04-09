namespace StarWars5e.Models
{
    public class ChapterRules : BaseEntity
    {
        public string ChapterName { get; set; }
        public int ChapterNumber { get; set; }
        public string ContentMarkdown { get; set; }
    }
}
