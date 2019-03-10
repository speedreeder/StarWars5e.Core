using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Interfaces;

namespace StarWars5e.Models.Equipment
{
    public class Vehicle : TableEntity, IVehicle
    {
        /// <inheritdoc />
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <inheritdoc />
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <inheritdoc />
        [JsonProperty("category")]
        public int Category { get; set; } = (int)ItemCategory.Vehicle;

        /// <inheritdoc />
        [JsonProperty("weight")]
        public double Weight { get; set; }

        /// <inheritdoc />
        [JsonProperty("cost")]
        public int Cost { get; set; }

        /// <inheritdoc />
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <inheritdoc/>
        [JsonProperty("rarity")]
        public ItemRarity Rarity { get; set; } = ItemRarity.Standard;

        /// <summary>
        /// Total speed of the mount (in feet)
        /// </summary>
        [JsonProperty("speed")]
        public int Speed { get; set; }
    }
}