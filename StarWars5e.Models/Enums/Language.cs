using System.ComponentModel;

namespace StarWars5e.Models.Enums
{
    public enum Language
    {
        [Description("None")]
        None = 0,
        [Description("English")]
        En,
        [Description("Spanish")]
        Sp,
        [Description("Russian")]
        Ru
    }
}
