using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Interfaces;

namespace StarWars5e.Models.Equipment
{
    public class Weapon : TableEntity, IWeapon
    {
        /// <inheritdoc />
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <inheritdoc />
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <inheritdoc />
        [JsonProperty("category")]
        public int Category { get; set; } = (int)ItemCategory.Weapon;

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
        ///  The minimum strength required to equip the item
        /// </summary>
        [JsonProperty("strengthRequirement")]
        public int StrengthRequirement { get; set; }

        /// <summary>
        ///  The type of damage inflicted by this weapon
        /// </summary>
        [JsonProperty("damageType")]
        public DamageType DamageType { get; set; }

        /// <summary>
        ///  The number of dice to be rolled with this weapon
        /// </summary>
        [JsonProperty("numberOfDie")]
        public int NumberOfDie { get; set; }

        /// <summary>
        ///  The size of dice to be rolled (e.g. d4 or d12)
        /// </summary>
        [JsonProperty("diceType")]
        public DiceType DiceType { get; set; }

        /// <summary>
        ///  Special properties associated with this weapon
        /// </summary>
        [JsonProperty("weaponProperties")]
        public IEnumerable<WeaponPropertyViewModel> WeaponProperties { get; set; }

        /// <summary>
        ///  The general family that weapon falls into
        /// </summary>
        [JsonProperty("weaponClassification")]
        public WeaponClassification WeaponClassification { get; set; }

        /// <summary>
        ///  The short range of the weapon
        /// </summary>
        [JsonProperty("shortRange")]
        public int ShortRange { get; set; }

        /// <summary>
        ///  The long range of the weapon
        /// </summary>
        [JsonProperty("longRange")]
        public int LongRange { get; set; }

        /// <summary>
        ///  The size of a single blaster clip for a ranged weapon
        /// </summary>
        [JsonProperty("clipSize")]
        public int ClipSize { get; set; }

        /// <summary>
        ///  Indicates that this weapon is able to use the burst ranged attack
        /// </summary>
        [JsonProperty("isBurst")]
        public bool IsBurst { get; set; }

        /// <summary>
        ///  The number of rounds used per burst ranged attack
        /// </summary>
        [JsonProperty("burstRoundsUsed")]
        public int BurstRoundsUsed { get; set; }

        /// <summary>
        ///  Number of hands required to wield this weapons
        /// </summary>
        [JsonProperty("requiredHands")]
        public int RequiredHands { get; set; } = 1;

        /// <summary>
        ///  Indicates that the weapon can be wielded in either one or two hands
        /// </summary>
        [JsonProperty("isVersatile")]
        public bool IsVersatile { get; set; }

        /// <summary>
        ///  The number of dice to be rolled with this weapon when wielded in 2 hands
        /// </summary>
        [JsonProperty("versatileNumberOfDie")]
        public int VersatileNumberOfDie { get; set; }

        /// <summary>
        ///  The size of dice to be rolled (e.g. d4 or d12) when wieleded in 2 hands
        /// </summary>
        [JsonProperty("versatileDiceType")]
        public DiceType VersatileDiceType { get; set; }

        /// <summary>
        ///  Indicates that the weapon is light (and easy to wield in an offhand)
        /// </summary>
        [JsonProperty("isLight")]
        public bool IsLight { get; set; }

        /// <summary>
        ///  Indicates that this weapon is easily hidden and you receive advantage on sleight of hand checks to hide it
        /// </summary>
        [JsonProperty("isHidden")]
        public bool IsHidden { get; set; }

        /// <summary>
        ///  Indicates that the weapon is heavy (and small creatures receive disadvantage on attack rolls)
        /// </summary>
        [JsonProperty("isHeavy")]
        public bool IsHeavy { get; set; }

        /// <summary>
        ///  Indicates that the weapon has the finesse attribute 
        /// </summary>
        [JsonProperty("isFinesse")]
        public bool IsFinesse { get; set; }

        /// <summary>
        ///  Indicates that the weapon has the reach attribute (and gets an extra five foot melee reach)
        /// </summary>
        [JsonProperty("isReach")]
        public bool IsReach { get; set; }

        /// <summary>
        ///  Indicates the weapon can be thrown
        /// </summary>
        [JsonProperty("isThrown")]
        public bool IsThrown { get; set; }

        /// <summary>
        ///  Indicates the weapon is double sided and attacks can be made with both sides
        /// </summary>
        [JsonProperty("isDouble")]
        public bool IsDouble { get; set; }
        
        /// <summary>
        ///  Indiates the weapon gives off a glow
        /// </summary>
        [JsonProperty("isLuminous")]
        public bool IsLuminous { get; set; }
    }
}