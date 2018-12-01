using Newtonsoft.Json;

namespace StarWars5e.Models.Monster
{
    public class BasicMonsterInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("monsterType")]
        public string MonsterType { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("alignment")]
        public string Alignment { get; set; }

        [JsonProperty("armorClass")]
        public int ArmorClass { get; set; }

        [JsonProperty("armorType")]
        public string ArmorType { get; set; }

        [JsonProperty("hitPoints")]
        public string HitPoints { get; set; }
    }
}