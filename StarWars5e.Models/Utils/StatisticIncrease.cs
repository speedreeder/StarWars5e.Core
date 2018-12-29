using Newtonsoft.Json;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Utils
{
    /// <summary>
    /// Models how much a statistic is increased by a species selection
    /// </summary>
    public class StatisticIncrease
    {
        /// <summary>
        ///  The name of the stat
        /// </summary>
        [JsonProperty("statisticName")]
        public string StatisticName
        {
            get => EnumMaps.ConvertAttributeToString(this.Attribute);
            set => this.Attribute = EnumMaps.RetrieveAttribute(value);
        }

        /// <summary>
        ///  The enum that is dealt with during transformation time
        /// </summary>
        [JsonIgnore]
        public CharacterAttribute Attribute{ get; set; }


        /// <summary>
        ///  The amount that it is increased by
        /// </summary>
        [JsonProperty("increase")]
        public int Increase { get; set; }

    }
}