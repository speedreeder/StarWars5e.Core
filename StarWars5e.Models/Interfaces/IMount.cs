namespace StarWars5e.Models.Interfaces
{
    public interface IMount : IEquipment
    {
        int CarryingCapacity { get; set; }

        int Speed { get; set; }
    }
}