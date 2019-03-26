using Microsoft.WindowsAzure.Storage.Table;

namespace StarWars5e.Models.Starship
{
    public class StarshipChapterRules : BaseEntity
    {
        public string ChapterName { get; set; }
        public int ChapterNumber { get; set; }
        public string ContentMarkdown { get; set; }
    }
}
