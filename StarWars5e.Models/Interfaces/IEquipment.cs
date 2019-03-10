using System.Collections.Generic;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Equipment;

namespace StarWars5e.Models.Interfaces
{
    [JsonConverter(typeof(EquipmentConverter))]
    public interface IEquipment
    {
        /// <summary>
        ///  The Id of the item (random GUID that relates to db)
        /// </summary>
        [JsonProperty("id")]
        string Id { get; set; }


        /// <summary>
        /// Name of the item
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Weight of the item (in pounds)
        /// </summary>
        double Weight { get; set; }

        /// <summary>
        ///  A general category of the item (this can be used to lock down the type of subclass to serialize)
        /// </summary>
        [JsonProperty("category")]
        int Category { get; set; }

        /// <summary>
        /// Cost of the item (in credits)
        /// </summary>
        int Cost { get; set; }

        /// <summary>
        /// Optional description for the item
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Rarity of the item
        /// </summary>
        ItemRarity Rarity { get; set; }
    }

    public interface IAdventurePack: IEquipment
    {
        /// <summary>
        /// A list of all items in the pack (and their associated quantity)
        /// </summary>
        IDictionary<string, int> Contents { get; set; }
    }
}