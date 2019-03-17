namespace StarWars5e.Models.Starship
{
    public class StarshipArmor : StarshipEquipment
    {
        public int Cost { get; set; }
        public int ArmorClassBonus { get; set; }
        public int? HitPointsPerHitDie { get; set; }
    }
}
