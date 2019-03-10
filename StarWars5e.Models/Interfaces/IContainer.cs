using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Interfaces
{
    public interface IContainer: IEquipment
    {
        double Capacity { get; set; }
        int MaximumWeight { get; set; }
        SizeType SizeType { get; set; }
    }
}