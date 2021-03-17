using System.ComponentModel;

namespace StarWars5e.Models.Enums
{
    public enum Language
    {
        [Description("None")]
        None = 0,
        [Description("English")]
        en,
        [Description("Spanish")]
        sp,
        [Description("Russian")]
        ru,
        [Description("Gungan")]
        gu
    }
}
