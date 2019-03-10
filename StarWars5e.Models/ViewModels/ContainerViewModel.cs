using Newtonsoft.Json;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Equipment;
using StarWars5e.Models.Interfaces;

namespace StarWars5e.Models.ViewModels
{
    public class ContainerViewModel : IContainer
    {
        /// <inheritdoc />
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <inheritdoc />
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <inheritdoc />
        [JsonProperty("category")]
        public int Category { get; set; } = (int)ItemCategory.Container;

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
        ///  The amount of capacity the container contains
        /// </summary>
        [JsonProperty("capacity")]
        public double Capacity { get; set; }

        /// <summary>
        ///  The type of storage space in this container
        /// </summary>
        [JsonProperty("sizeType")]
        public SizeType SizeType { get; set; }

        /// <summary>
        ///  The total amount of gear that can be held by this pack
        /// </summary>
        [JsonProperty("maximumWeight")]
        public int MaximumWeight { get; set; }
    }
}