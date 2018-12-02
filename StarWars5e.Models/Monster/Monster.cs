using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Models.Monster
{
    /// <summary>
    /// Representation of a Monster in the Sw5E Monster Manual
    /// </summary>
    public class Monster : TableEntity
    {
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public Monster() { }

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

        [JsonIgnore]
        public IEnumerable<KvPair> Features
        {
            get => this.FeatureJson == null ? new List<KvPair>() : JsonConvert.DeserializeObject<List<KvPair>>(this.FeatureJson) ?? new List<KvPair>();
            set => this.FeatureJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("features")]
        public string FeatureJson { get; set; } = "";


        [JsonProperty("actions")]
        public string ActionJson { get; private set; } = "";

        [JsonIgnore]
        public IEnumerable<KvPair> Actions
        {
            get => this.ActionJson == null ? new List<KvPair>() :  JsonConvert.DeserializeObject<List<KvPair>>(this.ActionJson) ?? new List<KvPair>();
            set => this.ActionJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("legendaryActions")]
        public string LegendaryActionsJson { get; set; } = "";

        [JsonIgnore]
        public List<KvPair> LegendaryActions
        {
            get => this.LegendaryActionsJson == null ? new List<KvPair>() : JsonConvert.DeserializeObject<List<KvPair>>(this.LegendaryActionsJson) ?? new List<KvPair>();
            set => this.LegendaryActionsJson = JsonConvert.SerializeObject(value);
        }

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
        public string SkillsJson  { get; set; } = "";

        [JsonIgnore]
        public IEnumerable<KvPair> Skills
        {
            get => JsonConvert.DeserializeObject<List<KvPair>>(this.SkillsJson) ?? new List<KvPair>();
            set => this.SkillsJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("senses")]
        public string SensesJson { get; set; } = "";


        [JsonIgnore]
        public IEnumerable<KvPair> Senses
        {
            get => JsonConvert.DeserializeObject<List<KvPair>>(this.SensesJson) ?? new List<KvPair>();
            set => this.SensesJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("languages")]
        public string LanguagesJson { get; set; } = "";

        [JsonIgnore]
        public IEnumerable<string> Languages
        {
            get => JsonConvert.DeserializeObject<List<string>>(this.LanguagesJson) ?? new List<string>();
            set => this.LanguagesJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("savingThrows")]
        public string SavingThrowsJson { get; set; } = "";

        [JsonIgnore]
        public IEnumerable<KvPair> SavingThrows
        {
            get => JsonConvert.DeserializeObject<List<KvPair>>(this.SavingThrowsJson) ?? new List<KvPair>();
            set => this.SavingThrowsJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("damageVulnerabilities")]
        public string DamageVulnerabilitiesJson { get; set; } = "";

        [JsonIgnore]
        public IEnumerable<string> DamageVulnerabilities
        {
            get => JsonConvert.DeserializeObject<List<string>>(this.DamageVulnerabilitiesJson) ?? new List<string>();
            set => this.DamageVulnerabilitiesJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("damageResistances")]
        public string DamageResistancesJson { get; set; } = "";

        [JsonIgnore]
        public IEnumerable<string> DamageResistances
        {
            get => JsonConvert.DeserializeObject<List<string>>(this.DamageResistancesJson) ?? new List<string>();
            set => this.DamageResistancesJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("damageImmunities")]
        public string DamageImmunitiesJson { get; set; } = "";

        [JsonIgnore]
        public IEnumerable<string> DamageImmunities
        {
            get => JsonConvert.DeserializeObject<List<string>>(this.DamageImmunitiesJson) ?? new List<string>();
            set => this.DamageImmunitiesJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("conditionImmunities")]
        public string ConditionImmunitiesJson { get; set; } = "";

        [JsonIgnore]
        public IEnumerable<string> ConditionImmunities
        {
            get => JsonConvert.DeserializeObject<List<string>>(this.ConditionImmunitiesJson) ?? new List<string>();
            set => this.ConditionImmunitiesJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("reactions")]
        public string ReactionsJson { get; set; } = "";

        [JsonIgnore]
        public IEnumerable<KvPair> Reactions
        {
            get => JsonConvert.DeserializeObject<List<KvPair>>(this.ReactionsJson) ?? new List<KvPair>();
            set => this.ReactionsJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("legendaryActionDescription")]
        public string LegendaryActionsDescription { get; set; } = "";
    }
}