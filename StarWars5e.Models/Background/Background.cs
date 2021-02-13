using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

namespace StarWars5e.Models.Background
{
    public class Background : BaseEntity
    {
        public string Name { get; set; }
        public string FlavorText { get; set; }
        public string FlavorName { get; set; }
        public string FlavorDescription { get; set; }
        public List<BackgroundOption> FlavorOptions { get; set; }
        public string FlavorOptionsJson
        {
            get => FlavorOptions == null ? "" : JsonConvert.SerializeObject(FlavorOptions);
            set => FlavorOptions = JsonConvert.DeserializeObject<List<BackgroundOption>>(value);
        }
        public string SkillProficiencies { get; set; }
        public string ToolProficiencies { get; set; }
        public string Languages { get; set; }
        public string Equipment { get; set; }
        public string SuggestedCharacteristics { get; set; }
        public string FeatureName { get; set; }
        public string FeatureText { get; set; }
        public List<BackgroundOption> FeatOptions { get; set; }
        public string FeatOptionsJson {
            get => FeatOptions == null ? "" : JsonConvert.SerializeObject(FeatOptions);
            set => FeatOptions = JsonConvert.DeserializeObject<List<BackgroundOption>>(value);
        }
        public List<BackgroundOption> PersonalityTraitOptions { get; set; }
        public string PersonalityTraitOptionsJson
        {
            get => PersonalityTraitOptions == null ? "" : JsonConvert.SerializeObject(PersonalityTraitOptions);
            set => PersonalityTraitOptions = JsonConvert.DeserializeObject<List<BackgroundOption>>(value);
        }
        public List<BackgroundOption> IdealOptions { get; set; }
        public string IdealOptionsJson
        {
            get => IdealOptions == null ? "" : JsonConvert.SerializeObject(IdealOptions);
            set => IdealOptions = JsonConvert.DeserializeObject<List<BackgroundOption>>(value);
        }
        public List<BackgroundOption> FlawOptions { get; set; }
        public string FlawOptionsJson
        {
            get => FlawOptions == null ? "" : JsonConvert.SerializeObject(FlawOptions);
            set => FlawOptions = JsonConvert.DeserializeObject<List<BackgroundOption >> (value);
        }
        public List<BackgroundOption> BondOptions { get; set; }
        public string BondOptionsJson
        {
            get => BondOptions == null ? "" : JsonConvert.SerializeObject(BondOptions);
            set => BondOptions = JsonConvert.DeserializeObject<List<BackgroundOption>>(value);
        }

        [IgnoreProperty]
        public List<Feature> Features { get; set; }
        private List<string> _featureRowKeys;
        public List<string> FeatureRowKeys
        {
            get => Features?.Select(f => f.RowKey).ToList();
            set => _featureRowKeys = value;
        }

        public string FeatureRowKeysJson
        {
            get => FeatureRowKeys == null ? "" : JsonConvert.SerializeObject(FeatureRowKeys);
            set => FeatureRowKeys = JsonConvert.DeserializeObject<List<string>>(value);
        }
    }
}
