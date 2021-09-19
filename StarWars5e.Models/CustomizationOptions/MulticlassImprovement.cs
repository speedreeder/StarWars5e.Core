namespace StarWars5e.Models.CustomizationOptions
{
    public class MulticlassImprovement : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Prerequisite { get; set; }
        public string Metadata { get; set; }
    }
}
