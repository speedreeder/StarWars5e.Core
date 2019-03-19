using Newtonsoft.Json;

namespace StarWars5e.Models.ViewModels
{
    /// <summary>
    /// KvPairs of items associated with a weapon
    /// </summary>
    public class WeaponPropertyViewModel
    {
        /// <summary>
        ///  The name of the property
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///  The content of the property
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

    }
}