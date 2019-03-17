namespace StarWars5e.Models.Starship
{
    public class StarshipShield : StarshipEquipment
    {
        public int Cost { get; set; }
        public decimal CapacityMultiplier { get; set; }
        public decimal RegenerationRateCoefficient { get; set; }
    }
}
