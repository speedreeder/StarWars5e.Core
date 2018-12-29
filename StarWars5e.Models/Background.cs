using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Models
{
    public class Background : TableEntity, IBackground
    {
        /// <summary>
        ///  The list of proficiencies this background gets to learn from
        /// </summary>
        [JsonIgnore]
        public List<string> ProficiencyOptions
        {
            get => string.IsNullOrEmpty(this.ProficiencyOptionJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(this.ProficiencyOptionJson);
            set => this.ProficiencyOptionJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("proficiencyOptions")]
        public string ProficiencyOptionJson { get; set; } = "";

        /// <summary>
        ///  The list of tools this background gets to learn from
        /// </summary>
        [JsonIgnore]
        public List<string> ToolProficiencyOptions
        {
            get => string.IsNullOrEmpty(this.ToolProficiencyJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(this.ToolProficiencyJson);
            set => this.ToolProficiencyJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("toolOptions")]
        public string ToolProficiencyJson { get; set; } = "";


        /// <summary>
        ///  List of languages this background explicitly gets
        /// </summary>
        [JsonIgnore]
        public List<string> LanguagesLearned
        {
            get => string.IsNullOrEmpty(this.LanguagesLearnedJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(this.LanguagesLearnedJson);
            set => this.LanguagesLearnedJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("languagesLearned")]
        public string LanguagesLearnedJson { get; set; } = "";

        /// <summary>
        ///  The die roll that contains speciality options for this bakcground
        /// </summary>
        [JsonIgnore]
        public DieRollViewModel SpecialityOptions
        {
            get => string.IsNullOrEmpty(this.SpecialityJson) ? new DieRollViewModel() : JsonConvert.DeserializeObject<DieRollViewModel>(this.SpecialityJson);
            set => this.SpecialityJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("specialityOptions")]
        public string SpecialityJson { get; set; } = "";

        /// <summary>
        ///  The Die roll that was contains options for feats that this background can choose
        /// </summary>
        [JsonIgnore]
        public DieRollViewModel FeatOptions
        {
            get => string.IsNullOrEmpty(this.FeatOptionsJson) ? new DieRollViewModel() : JsonConvert.DeserializeObject<DieRollViewModel>(this.FeatOptionsJson);
            set => this.FeatOptionsJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("featOptions")]
        public string FeatOptionsJson { get; set; } = "";

        /// <summary>
        ///  The die roll that provides suggested personality traits
        /// </summary>
        [JsonIgnore]
        public DieRollViewModel PersonalityTraits
        {
            get => string.IsNullOrEmpty(this.PersonalityJson) ? new DieRollViewModel() : JsonConvert.DeserializeObject<DieRollViewModel>(this.PersonalityJson);
            set => this.PersonalityJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("personalityTraits")]
        public string PersonalityJson { get; set; } = "";

        /// <summary>
        ///  The Die roll containing suggested ideals
        /// </summary>
        [JsonIgnore]
        public DieRollViewModel Ideals
        {
            get => string.IsNullOrEmpty(this.IdealJson) ? new DieRollViewModel() : JsonConvert.DeserializeObject<DieRollViewModel>(this.IdealJson);
            set => this.IdealJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("ideals")]
        public string IdealJson { get; set; } = "";

        /// <summary>
        ///  Die roll containing suggested Bonds
        /// </summary>
        [JsonIgnore]
        public DieRollViewModel Bonds
        {
            get => string.IsNullOrEmpty(this.BondJson) ? new DieRollViewModel() : JsonConvert.DeserializeObject<DieRollViewModel>(this.BondJson);
            set => this.BondJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("bonds")]
        public string BondJson { get; set; } = "";

        /// <summary>
        ///  Die roll containing suggested flaws
        /// </summary>
        [JsonIgnore]
        public DieRollViewModel Flaws
        {
            get => string.IsNullOrEmpty(this.FlawJson) ? new DieRollViewModel() : JsonConvert.DeserializeObject<DieRollViewModel>(this.FlawJson);
            set => this.FlawJson = JsonConvert.SerializeObject(value);
        }

        [JsonProperty("flaws")]
        public string FlawJson { get; set; } = "";




        /// <summary>
        ///  The name of the background
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///  The description of the background
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        ///  The number of proficiencies that you get to choose
        /// </summary>
        [JsonProperty("proficiencyCount")]
        public int ProficiencyCount { get; set; }

        /// <summary>
        ///  The flavor text that contains the proficiency list
        /// </summary>
        [JsonProperty("proficiencyMarkdown")]
        public string ProficiencyMarkdown { get; set; }

        /// <summary>
        ///  The flavor text that contains the tool proficiency list
        /// </summary>
        [JsonProperty("toolProficiencyMarkdown")]
        public string ToolProficiencyMarkdown { get; set; }

        /// <summary>
        ///  The number of languages that this background gets to learn
        /// </summary>
        [JsonProperty("languages")]
        public int Languages { get; set; }

        /// <summary>
        ///  The flavor text for languages this background gets
        /// </summary>
        [JsonProperty("languageMarkdown")]
        public string LanguageMarkdown { get; set; }

        /// <summary>
        ///  The flavor text associated with the equipment options this bakcground has
        /// </summary>
        [JsonProperty("equipmentMarkdown")]
        public string EquipmentMarkdown { get; set; }

        /// <summary>
        ///  The name of the (optional) speciality of this background
        /// </summary>
        [JsonProperty("specialityName")]
        public string SpecialityName { get; set; }

        /// <summary>
        ///  The flavor text for this background speciality
        /// </summary>
        [JsonProperty("specialityMarkdown")]
        public string SpecialityMarkdown { get; set; }

        /// <summary>
        ///  The name of the special feature this background has
        /// </summary>
        [JsonProperty("featureName")]
        public string FeatureName { get; set; }

        /// <summary>
        ///  The flavor text that this background feature has
        /// </summary>
        [JsonProperty("featureMarkdown")]
        public string FeatureMarkdown { get; set; }

        /// <summary>
        ///  The flavor text for the background feat
        /// </summary>
        [JsonProperty("backgroundFeatMarkdown")]
        public string BackgroundFeatMarkdown { get; set; }

        /// <summary>
        ///  The flavor text for the suggested characteristics of this background
        /// </summary>
        [JsonProperty("suggestedCharacteristicsMarkdown")]
        public string SuggestedCharacteristicsMarkdown { get; set; }
    }
}