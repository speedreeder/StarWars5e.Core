using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Monster
{
    public class Monster : BaseEntity
    {
        public Monster()
        {
            Types = new List<string>();
        }
        public string Name { get; set; }
        public MonsterSize SizeEnum { get; set; }
        public string Size
        {
            get => SizeEnum.ToString();
            set => SizeEnum = Enum.Parse<MonsterSize>(value);
        }
        public List<string> Types { get; set; }
        public string TypesJson
        {
            get => Types == null ? "" : JsonConvert.SerializeObject(Types);
            set => Types = JsonConvert.DeserializeObject<List<string>>(value);
        }
        public string Alignment { get; set; }
        public int ArmorClass { get; set; }
        public string ArmorType { get; set; }
        public int HitPoints { get; set; }
        public string HitPointRoll { get; set; }
        public int Speed { get; set; }
        public int Strength { get; set; }
        public int StrengthModifier { get; set; }
        public int Dexterity { get; set; }
        public int DexterityModifier { get; set; }
        public int Constitution { get; set; }
        public int ConstitutionModifier { get; set; }
        public int Intelligence { get; set; }
        public int IntelligenceModifier { get; set; }
        public int Wisdom { get; set; }
        public int WisdomModifier { get; set; }
        public int Charisma { get; set; }
        public int CharismaModifier { get; set; }

        public List<string> SavingThrows { get; set; }
        public string SavingThrowsJson
        {
            get => SavingThrows == null ? "" : JsonConvert.SerializeObject(SavingThrows);
            set => SavingThrows = JsonConvert.DeserializeObject<List<string>>(value);
        }

        public List<string> Skills { get; set; }
        public string SkillsJson
        {
            get => Skills == null ? "" : JsonConvert.SerializeObject(Skills);
            set => Skills = JsonConvert.DeserializeObject<List<string>>(value);
        }

        public List<DamageType> DamageImmunitiesParsed { get; set; }
        public string DamageImmunitiesParsedJson
        {
            get => DamageImmunitiesParsed == null ? "" : JsonConvert.SerializeObject(DamageImmunitiesParsed.Select(d => d.ToString()));
            set => DamageImmunitiesParsed = JsonConvert.DeserializeObject<List<int>>(value).Select(i => (DamageType) i).ToList();
        }
        public List<string> DamageImmunitiesOther { get; set; }
        public string DamageImmunitiesOtherJson
        {
            get => DamageImmunitiesOther == null ? "" : JsonConvert.SerializeObject(DamageImmunitiesOther);
            set => DamageImmunitiesOther = JsonConvert.DeserializeObject<List<string>>(value);
        }
        
        public List<DamageType> DamageResistancesParsed { get; set; }
        public string DamageResistancesParsedJson
        {
            get => DamageResistancesParsed == null ? "" : JsonConvert.SerializeObject(DamageResistancesParsed.Select(d => d.ToString()));
            set => DamageResistancesParsed = JsonConvert.DeserializeObject<List<int>>(value).Select(i => (DamageType)i).ToList();
        }
        public List<string> DamageResistancesOther { get; set; }
        public string DamageResistancesOtherJson
        {
            get => DamageResistancesOther == null ? "" : JsonConvert.SerializeObject(DamageResistancesOther);
            set => DamageResistancesOther = JsonConvert.DeserializeObject<List<string>>(value);
        }

        public List<DamageType> DamageVulnerabilitiesParsed { get; set; }
        public string DamageVulnerabilitiesParsedJson
        {
            get => DamageVulnerabilitiesParsed == null ? "" : JsonConvert.SerializeObject(DamageVulnerabilitiesParsed.Select(d => d.ToString()));
            set => DamageVulnerabilitiesParsed = JsonConvert.DeserializeObject<List<int>>(value).Select(i => (DamageType)i).ToList();
        }
        public List<string> DamageVulnerabilitiesOther { get; set; }
        public string DamageVulnerabilitiesOtherJson
        {
            get => DamageVulnerabilitiesOther == null ? "" : JsonConvert.SerializeObject(DamageVulnerabilitiesOther);
            set => DamageVulnerabilitiesOther = JsonConvert.DeserializeObject<List<string>>(value);
        }

        public List<Condition> ConditionImmunitiesParsed { get; set; }
        public string ConditionImmunitiesParsedJson
        {
            get => ConditionImmunitiesParsed == null ? "" : JsonConvert.SerializeObject(ConditionImmunitiesParsed.Select(d => d.ToString()));
            set => ConditionImmunitiesParsed = JsonConvert.DeserializeObject<List<int>>(value).Select(i => (Condition)i).ToList();
        }
        public List<string> ConditionImmunitiesOther { get; set; }
        public string ConditionImmunitiesOtherJson
        {
            get => ConditionImmunitiesOther == null ? "" : JsonConvert.SerializeObject(ConditionImmunitiesOther);
            set => ConditionImmunitiesOther = JsonConvert.DeserializeObject<List<string>>(value);
        }

        public List<string> Senses { get; set; }
        public string SensesJson
        {
            get => Senses == null ? "" : JsonConvert.SerializeObject(Senses);
            set => Senses = JsonConvert.DeserializeObject<List<string>>(value);
        }
        public List<string> Languages { get; set; }
        public string LanguagesJson
        {
            get => Languages == null ? "" : JsonConvert.SerializeObject(Languages);
            set => Languages = JsonConvert.DeserializeObject<List<string>>(value);
        }
        public string ChallengeRating { get; set; }
        public int ExperiencePoints { get; set; }
        public List<MonsterBehavior> Behaviors { get; set; }
        public string BehaviorsJson
        {
            get => Behaviors == null ? "" : JsonConvert.SerializeObject(Behaviors);
            set => Behaviors = JsonConvert.DeserializeObject<List<MonsterBehavior>>(value);
        }
    }
}
