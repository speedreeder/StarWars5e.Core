using System.Collections.Generic;
using Newtonsoft.Json;
using StarWars5e.Models.Monster;
using StarWars5e.Models.Utils;

namespace StarWars5e.Models.ViewModels
{
    public class MonsterViewModel
    {
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public MonsterViewModel() { }

        [JsonProperty("basics")]
        public BasicMonsterInfo Basics { get; set; }

        [JsonProperty("stats")]
        public StatValues Stats { get; set; }
        
        [JsonProperty("speed")]
        public MovementSpeeds Speed { get; set; }

        [JsonProperty("challenge")]
        public string Challenge { get; set; }

        [JsonProperty("experiencePoints")]
        public string ExperiencePoints { get; set; }

        [JsonProperty("skills")]
        public IEnumerable<KvPair> Skills { get; set; }

        [JsonProperty("senses")]
        public IEnumerable<KvPair> Senses { get; set; }

        [JsonProperty("languages")]
        public IEnumerable<string> Languages { get; set; }

        [JsonProperty("resistances")]
        public Resistances Resistances { get; set; }

        [JsonProperty("features")]
        public IEnumerable<KvPair> Features { get; set; }

        [JsonProperty("actions")]
        public IEnumerable<KvPair> Actions { get; set; }

        [JsonProperty("legendaryActionDescription")]
        public string LegendaryActionsDescription { get; set; }

        [JsonProperty("legendaryActions")]
        public IEnumerable<KvPair> LegendaryActions { get; set; }

        [JsonProperty("reactions")]
        public IEnumerable<KvPair> Reactions { get; set; }
    }
}