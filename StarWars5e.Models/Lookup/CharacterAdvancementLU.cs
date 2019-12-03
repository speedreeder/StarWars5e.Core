namespace StarWars5e.Models.Lookup
{
    public class CharacterAdvancementLU : BaseEntity
    {
        public int Level { get; set; }
        public int ExperiencePoints { get; set; }
        public int ProficiencyBonus { get; set; }
    }
}
