using System.Collections.Generic;
using Newtonsoft.Json;
using StarWars5e.Models.Utils;

namespace StarWars5e.Models.Monster
{
    /// <summary>
    /// Representation of a Monster in the Sw5E Monster Manual
    /// </summary>
    public class Monster
    {
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public Monster() { }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("monsterType")]
        public MonsterType MonsterType { get; set; }

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

        [JsonProperty("features")]
        public IEnumerable<KvPair> Features { get; set; }

        [JsonProperty("actions")]
        public IEnumerable<KvPair> Actions { get; set; }

        [JsonProperty("legendaryActions")]
        public IEnumerable<KvPair> LegendaryActions { get; set; }

        [JsonProperty("walkingSpeed")]
        public int WalkingSpeed { get; set; }

        [JsonProperty("climbingSpeed")]
        public int ClimbingSpeed { get; set; }

        [JsonProperty("flyingSpeed")]
        public int FlyingSpeed { get; set; }

        [JsonProperty("swimmingSpeed")]
        public int SwimmingSpeed { get; set; }

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

        [JsonProperty("reactions")]
        public IEnumerable<KvPair> Reactions { get; set; }

        [JsonProperty("legendaryActionDescription")]
        public string LegendaryActionsDescription { get; set; }
    }
}