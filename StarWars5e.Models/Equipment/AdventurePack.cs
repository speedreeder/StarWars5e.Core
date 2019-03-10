using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Interfaces;

namespace StarWars5e.Models.Equipment
{
    public class AdventurePack : TableEntity, IAdventurePack
    {
        /// <inheritdoc />
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <inheritdoc />
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <inheritdoc />
        [JsonProperty("category")]
        public int Category { get; set; } = (int) ItemCategory.AdventurePack;

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
        public ItemRarity Rarity { get; set; }

        /// <inheritdoc/>
        [JsonIgnore]
        public IDictionary<string, int> Contents { get; set; }

        /// <summary>
        ///  The contents of an adventure pack converted to a  JSON string
        /// </summary>
        [JsonProperty("contentsString")]
        public string ContentsString
        {
            get => Contents == null ? "" : JsonConvert.SerializeObject(Contents);
            set => Contents = JsonConvert.DeserializeObject<Dictionary<string, int>>(value);

        }

    }
}