using Newtonsoft.Json;

namespace StarWars5e.Models.Utils
{
    /// <summary>
    /// Simple K/V Pair
    /// </summary>
    public class KvPair
    {
        /// <summary>
        /// Key
        /// </summary>
        [JsonProperty("keyName")]
        public string Name { get; set; }

        /// <summary>
        /// Value (converted into string)
        /// </summary>
        [JsonProperty("keyValue")]
        public string Value { get; set; }
    }
}