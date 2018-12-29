using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.ViewModels
{
    public class DieRollViewModel
    {
        /// <summary>
        ///  The type of dice to roll for this 
        /// </summary>
        [JsonProperty("diceType")]
        public DiceType DiceType { get; set; }

        /// <summary>
        ///  Name of the die roll (could be generic or quite specific)
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }


        /// <summary>
        ///  The different results that can happen because of this
        /// </summary>
        [JsonProperty("values")]
        public List<DieRollValueViewModel> Values { get; set; } = new List<DieRollValueViewModel>();

    }
    public class DieRollValueViewModel
    {
        /// <summary>
        ///  The minimum value for this thing to happen
        /// </summary>
        [JsonProperty("minValue")]
        public int MinValue { get; set; }

        /// <summary>
        ///  The max value for this thing to happen
        /// </summary>
        [JsonProperty("maxValue")]
        public int MaxValue { get; set; }
    
        /// <summary>
        ///  The result of this roll
        /// </summary>
        [JsonProperty("result")]
        public string Result { get; set; }

    }
}
