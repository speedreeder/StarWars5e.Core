using Newtonsoft.Json;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Interfaces;

namespace StarWars5e.Models.ViewModels
{
    public class ArmorViewModel : IArmor
    {
        /// <inheritdoc />
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <inheritdoc />
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <inheritdoc />
        [JsonProperty("category")]
        public int Category { get; set; } = (int)ItemCategory.Armor;

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
        ///  The class of armor (light medium or heavy)
        /// </summary>
        [JsonProperty("armorClassification")]
        public ArmorClassification ArmorClassification { get; set; }

        /// <summary>
        ///  The base armor class of the item
        /// </summary>
        [JsonProperty("armorClass")]
        public int ArmorClass { get; set; }

        /// <summary>
        ///  Maximum dexterity bonus that can be applied to the armor class
        /// </summary>
        [JsonProperty("dexterityMax")]
        public int DexterityMax { get; set; }

        /// <summary>
        ///  The minimum strength required to equip the item
        /// </summary>
        [JsonProperty("strengthRequirement")]
        public int StrengthRequirement { get; set; }

        /// <summary>
        ///  Indicates that the item is a shield
        /// </summary>
        [JsonProperty("isShield")]
        public bool IsShield { get; set; }

        /// <summary>
        ///  Indicates whether all stealth checks are taken at disadvantage
        /// </summary>
        [JsonProperty("stealthImpaired")]
        public bool StealthImpaired { get; set; }
    }
}