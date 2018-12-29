using System.Collections.Generic;
using Newtonsoft.Json;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Models
{
    public interface IBackground
    {
        /// <summary>
        ///  The name of the background
        /// </summary>
        [JsonProperty("name")]
        string Name { get; set; }

        /// <summary>
        ///  The description of the background
        /// </summary>
        [JsonProperty("description")]
        string Description { get; set; }

        /// <summary>
        ///  The list of proficiencies this background gets to learn from
        /// </summary>
        [JsonProperty("proficiencyOptions")]
        List<string> ProficiencyOptions { get; set; }

        /// <summary>
        ///  The number of proficiencies that you get to choose
        /// </summary>
        [JsonProperty("proficiencyCount")]
        int ProficiencyCount { get; set; }


        /// <summary>
        ///  The flavor text that contains the proficiency list
        /// </summary>
        [JsonProperty("proficiencyMarkdown")]
        string ProficiencyMarkdown { get; set; }

        /// <summary>
        ///  The list of tools this background gets to learn from
        /// </summary>
        [JsonProperty("toolOptions")]
        List<string> ToolProficiencyOptions { get; set; }

        /// <summary>
        ///  The flavor text that contains the tool proficiency list
        /// </summary>
        [JsonProperty("toolProficiencyMarkdown")]
        string ToolProficiencyMarkdown { get; set; }

        /// <summary>
        ///  List of languages this background explicitly gets
        /// </summary>
        [JsonProperty("languagesLearned")]
        List<string> LanguagesLearned { get; set; }

        /// <summary>
        ///  The number of languages that this background gets to learn
        /// </summary>
        [JsonProperty("languages")]
        int Languages { get; set; }

        /// <summary>
        ///  The flavor text for languages this background gets
        /// </summary>
        [JsonProperty("languageMarkdown")]
        string LanguageMarkdown { get; set; }

        /// <summary>
        ///  The flavor text associated with the equipment options this bakcground has
        /// </summary>
        [JsonProperty("equipmentMarkdown")]
        string EquipmentMarkdown { get; set; }

        /// <summary>
        ///  The name of the (optional) speciality of this background
        /// </summary>
        [JsonProperty("specialityName")]
        string SpecialityName { get; set; }

        /// <summary>
        ///  The flavor text for this background speciality
        /// </summary>
        [JsonProperty("specialityMarkdown")]
        string SpecialityMarkdown { get; set; }

        /// <summary>
        ///  The die roll that contains speciality options for this bakcground
        /// </summary>
        [JsonProperty("specialityOptions")]
        DieRollViewModel SpecialityOptions { get; set; }


        /// <summary>
        ///  The name of the special feature this background has
        /// </summary>
        [JsonProperty("featureName")]
        string FeatureName { get; set; }

        /// <summary>
        ///  The flavor text that this background feature has
        /// </summary>
        [JsonProperty("featureMarkdown")]
        string FeatureMarkdown { get; set; }

        /// <summary>
        ///  The flavor text for the background feat
        /// </summary>
        [JsonProperty("backgroundFeatMarkdown")]
        string BackgroundFeatMarkdown { get; set; }


        /// <summary>
        ///  The Die roll that was contains options for feats that this background can choose
        /// </summary>
        [JsonProperty("featOptions")]
        DieRollViewModel FeatOptions { get; set; }

        /// <summary>
        ///  The flavor text for the suggested characteristics of this background
        /// </summary>
        [JsonProperty("suggestedCharacteristicsMarkdown")]
        string SuggestedCharacteristicsMarkdown { get; set; }

        /// <summary>
        ///  The die roll that provides suggested personality traits
        /// </summary>
        [JsonProperty("personalityTraits")]
        DieRollViewModel PersonalityTraits { get; set; }

        /// <summary>
        ///  The Die roll containing suggested ideals
        /// </summary>
        [JsonProperty("ideals")]
        DieRollViewModel Ideals { get; set; }

        /// <summary>
        ///  Die roll containing suggested Bonds
        /// </summary>
        [JsonProperty("bonds")]
        DieRollViewModel Bonds { get; set; }

        /// <summary>
        ///  Die roll containing suggested flaws
        /// </summary>
        [JsonProperty("flaws")]
        DieRollViewModel Flaws { get; set; }
    }
}