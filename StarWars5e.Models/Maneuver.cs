namespace StarWars5e.Models
{
    public  class Maneuver : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Metadata { get; set; }
        public string Prerequisite { get; set; }
        public string Type { get; set; }
    }
}
