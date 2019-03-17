using System.Collections.Generic;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models
{
    /// <summary>
    /// All base level fields for the Species class
    /// </summary>
    public interface ISpecies
    {
        /// <summary>
        ///  Name of the species
        /// </summary>
        [JsonProperty("name")]
        string SpeciesName { get; set; }

        /// <summary>
        ///  Images that are registered for this species
        /// </summary>
        [JsonProperty("imageUrls")]
        IEnumerable<string> ImageUrls { get; set; }

        /// <summary>
        ///  Valid colors of the skin
        /// </summary>
        [JsonProperty("skinColors")]
        IEnumerable<string> SkinColors { get; set; }

        /// <summary>
        ///  Valid colors of the species hair
        /// </summary>
        [JsonProperty("hairColors")]
        IEnumerable<string> HairColors { get; set; }

        /// <summary>
        ///  Valid colors of the species eyes
        /// </summary>
        [JsonProperty("eyeColors")]
        IEnumerable<string> EyeColors { get; set; }

        /// <summary>
        ///  Distinctive Features of the species
        /// </summary>
        [JsonProperty("distinctions")]
        string Distinctions { get; set; }


        /// <summary>
        ///  The low end of height for the species
        /// </summary>
        [JsonProperty("mininumHeight")]
        string MininumHeight { get; set; }

        /// <summary>
        ///  The Modifier for the height of the species
        /// </summary>
        [JsonProperty("heightModifier")]
        string HeightModifier { get; set; }

        /// <summary>
        ///  The low end of the height for the species
        /// </summary>
        [JsonProperty("minimumWeight")]
        string MinimumWeight { get; set; }

        /// <summary>
        ///  The modifier that is applied to the weight
        /// </summary>
        [JsonProperty("weightModifier")]
        string WeightModifier { get; set; }


        /// <summary>
        ///  The name of the species homeworld
        /// </summary>
        [JsonProperty("homeworld")]
        string Homeworld { get; set; }

        /// <summary>
        ///  The languages the species (generally) knows by default
        /// </summary>
        [JsonProperty("defaultLanguages")]
        IEnumerable<string> DefaultLanguages { get; set; }

        /// <summary>
        ///  The biology and appearance description of the species
        /// </summary>
        [JsonProperty("biologyMarkdown")]
        string BiologyMarkdown { get; set; }

        /// <summary>
        ///  Name of the Biology section differs (based on droids etc)
        /// </summary>
        [JsonProperty("biologyName")]
        string BiologyName { get; set; }

        /// <summary>
        ///  The society and culture markdown of the species
        /// </summary>
        [JsonProperty("societyMarkdown")]
        string SocietyMarkdown { get; set; }

        /// <summary>
        ///  The short description that is displayed before the common names
        /// </summary>
        [JsonProperty("nameMarkdown")]
        string NameMarkdown { get; set; }

        /// <summary>
        ///  Common female names of the species
        /// </summary>
        [JsonProperty("femaleNames")]
        IEnumerable<string> FemaleNames { get; set; }

        /// <summary>
        ///  Common male names of the species
        /// </summary>
        [JsonProperty("maleNames")]
        IEnumerable<string> MaleNames { get; set; }

        /// <summary>
        ///  Common surnames for the species
        /// </summary>
        [JsonProperty("surnames")]
        IEnumerable<string> Surnames { get; set; }

        /// <summary>
        ///  Common age milestones for the species
        /// </summary>
        [JsonProperty("age")]
        string Age { get; set; }

        /// <summary>
        ///  Alignment tendencies for the species
        /// </summary>
        [JsonProperty("alignment")]
        string Alignment { get; set; }

        /// <summary>
        ///  The size for the species
        /// </summary>
        [JsonProperty("size")]
        StarshipSize Size { get; set; }

        /// <summary>
        ///  The size description of the species
        /// </summary>
        [JsonProperty("sizeMarkdown")]
        string SizeMarkdown { get; set; }

        /// <summary>
        ///  The speed description of the species
        /// </summary>
        [JsonProperty("speedMarkdown")]
        string SpeedMarkdown { get; set; }
        

        /// <summary>
        ///  Other attribute KvPair for the species
        /// </summary>
        [JsonProperty("otherAttributes")]
        string OtherAttributesJson { get; set; }

        /// <summary>
        ///  The JSON representation of all statistic increases this species gets (used for tagging)
        /// </summary>
        [JsonProperty("statisticIncreases")]
        string StatisticIncreasesJson { get; set; }

        /// <summary>
        ///  Other proficiencies that the species gets (duplicated from other attributes etc)
        /// </summary>
        /// <remarks>This is used to help tagging of the species</remarks>
        [JsonProperty("proficienciesGained")]
        string ProficienciesGainedJson { get; set; }

        /// <summary>
        /// The value for statistic increases that will be displayed in the text
        /// </summary>
        [JsonProperty("statisticIncreaseMarkdown")]
        string StatisticIncreaseMarkdown { get; set; }

        /// <summary>
        ///  Indicates that this species has darkvision
        /// </summary>
        [JsonProperty("darkvision")]
        bool Darkvision { get; set; }

        /// <summary>
        /// companies that make this droid
        /// </summary>
        [JsonProperty("manufacturers")]
        IEnumerable<string> Manufacturers { get; set; }

        /// <summary>
        ///  Valid droid color schemes
        /// </summary>
        [JsonProperty("colorSchemes")]
        IEnumerable<string> ColorSchemes { get; set; }

        /// <summary>
        /// Markdown for the utility section of droids
        /// </summary>
        [JsonProperty("utilityMarkdown")]
        string UtilityMarkdown { get; set; }

        /// <summary>
        /// List of common first names for the species
        /// </summary>
        [JsonProperty("firstNames")]
        IEnumerable<string> FirstNames { get; set; }

        /// <summary>
        /// List of common first names for the species
        /// </summary>
        [JsonProperty("names")]
        IEnumerable<string> Names { get; set; }

        /// <summary>
        /// List of common first names for the species
        /// </summary>
        [JsonProperty("nicknames")]
        IEnumerable<string> Nicknames { get; set; }
    }
}