using System.Collections.Generic;
using Newtonsoft.Json;
using StarWars5e.Models.Utils;

namespace StarWars5e.Models.Monster
{
    public class Resistances
    {
        [JsonProperty("savingThrows")]
        public List<KvPair> SavingThrows { get; set; }

        [JsonProperty("damageVulnerabilities")]
        public IEnumerable<string> DamageVulnerabilities { get; set; }

        [JsonProperty("damageResistances")]
        public IEnumerable<string> DamageResistances { get; set; }

        [JsonProperty("damageImmunities")]
        public IEnumerable<string> DamageImmunities { get; set; }

        [JsonProperty("conditionImmunities")]
        public IEnumerable<string> ConditionImmunities { get; set; }
    }
}