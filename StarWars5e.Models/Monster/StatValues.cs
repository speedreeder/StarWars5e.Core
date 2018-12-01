using Newtonsoft.Json;

namespace StarWars5e.Models.Monster
{
    public class StatValues
    {
        [JsonProperty("strength")]
        public int Strength { get; set; }

        [JsonProperty("strengthModifier")]
        public int StrengthModifier { get; set; }

        [JsonProperty("constitution")]
        public int Constitution { get; set; }

        [JsonProperty("constitutionModifier")]
        public int ConstitutionModifer { get; set; }

        [JsonProperty("dexterity")]
        public int Dexterity { get; set; }

        [JsonProperty("dexterityModifier")]
        public int DexterityModifier { get; set; }

        [JsonProperty("intelligence")]
        public int Intelligence { get; set; }

        [JsonProperty("intelligenceModifier")]
        public int IntelligenceModifier { get; set; }

        [JsonProperty("wisdom")]
        public int Wisdom { get; set; }

        [JsonProperty("wisdomModifier")]
        public int WisdomModifier { get; set; }

        [JsonProperty("charisma")]
        public int Charisma { get; set; }

        [JsonProperty("charismaModifier")]
        public int CharismaModifier { get; set; }
    }
}