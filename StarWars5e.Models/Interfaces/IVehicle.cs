namespace StarWars5e.Models.Interfaces
{
    public interface IVehicle : IEquipment
    {
        int Speed { get; set; }
    }
}