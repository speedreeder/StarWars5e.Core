namespace StarWars5e.Models
{
    public class Feat : BaseEntity
    {
        public string Name { get; set; }
        public string Prerequisite { get; set; }
        public string Text { get; set; }
    }
}
