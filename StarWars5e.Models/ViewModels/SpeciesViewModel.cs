using System.Collections.Generic;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Models.ViewModels
{
    public class SpeciesViewModel : ISpecies
    {
        /// <summary>
        ///  Bonuses that the species gets 
        /// </summary>
        [JsonIgnore]
        public IEnumerable<StatisticIncrease> StatisticIncreases
        {
            get => string.IsNullOrEmpty(this.StatisticIncreasesJson) ? new List<StatisticIncrease>() : JsonConvert.DeserializeObject<List<StatisticIncrease>>(this.StatisticIncreasesJson);
            set => this.StatisticIncreasesJson = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        ///  Other proficiencies that the species gets (duplicated from other attributes etc)
        /// </summary>
        /// <remarks>This is used to help tagging of the species</remarks>
        [JsonIgnore]
        public IEnumerable<string> ProficienciesGained
        {
            get => string.IsNullOrEmpty(this.ProficienciesGainedJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(this.ProficienciesGainedJson);
            set => this.ProficienciesGainedJson = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        ///  Other attribute KvPair for the species
        /// </summary>
        [JsonIgnore]
        public IEnumerable<KvPair> OtherAttributes
        {
            get => string.IsNullOrEmpty(this.OtherAttributesJson) ? new List<KvPair>() : JsonConvert.DeserializeObject<List<KvPair>>(this.OtherAttributesJson);
            set => this.OtherAttributesJson = JsonConvert.SerializeObject(value);
        }


        /// <summary>
        ///  Name of the species
        /// </summary>
        [JsonProperty("name")]
        public string SpeciesName { get; set; }

        /// <summary>
        ///  Images that are registered for this species
        /// </summary>
        [JsonProperty("imageUrls")]
        public string ImageUrlJson { get; set; } = "";

        /// <summary>
        ///  Other proficiencies that the species gets (duplicated from other attributes etc)
        /// </summary>
        /// <remarks>This is used to help tagging of the species</remarks>
        [JsonIgnore]
        public IEnumerable<string> ImageUrls
        {
            get => string.IsNullOrEmpty(this.ImageUrlJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(this.ImageUrlJson);
            set => this.ImageUrlJson = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        ///  Valid colors of the skin
        /// </summary>
        [JsonProperty("skinColors")]
        public string SkinColorJson { get; set; }

        /// <summary>
        ///  Other proficiencies that the species gets (duplicated from other attributes etc)
        /// </summary>
        /// <remarks>This is used to help tagging of the species</remarks>
        [JsonIgnore]
        public IEnumerable<string> SkinColors
        {
            get => string.IsNullOrEmpty(this.SkinColorJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(this.SkinColorJson);
            set => this.SkinColorJson = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        ///  Valid colors of the species hair
        /// </summary>
        [JsonProperty("hairColors")]
        public string HairColorJson { get; set; } = "";

        /// <summary>
        ///  Other proficiencies that the species gets (duplicated from other attributes etc)
        /// </summary>
        /// <remarks>This is used to help tagging of the species</remarks>
        [JsonIgnore]
        public IEnumerable<string> HairColors
        {
            get => string.IsNullOrEmpty(this.HairColorJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(this.HairColorJson);
            set => this.HairColorJson = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        ///  Valid colors of the species eyes
        /// </summary>
        [JsonProperty("eyeColors")]
        public string EyeColorJson { get; set; } = "";

        /// <summary>
        ///  Other proficiencies that the species gets (duplicated from other attributes etc)
        /// </summary>
        /// <remarks>This is used to help tagging of the species</remarks>
        [JsonIgnore]
        public IEnumerable<string> EyeColors
        {
            get => string.IsNullOrEmpty(this.EyeColorJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(this.EyeColorJson);
            set => this.EyeColorJson = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        ///  Distinctive Features of the species
        /// </summary>
        [JsonProperty("distinctions")]
        public string Distinctions { get; set; }


        /// <summary>
        ///  The low end of height for the species
        /// </summary>
        [JsonProperty("mininumHeight")]
        public string MininumHeight { get; set; }

        /// <summary>
        ///  The Modifier for the height of the species
        /// </summary>
        [JsonProperty("heightModifier")]
        public string HeightModifier { get; set; }

        /// <summary>
        ///  The low end of the height for the species
        /// </summary>
        [JsonProperty("minimumWeight")]
        public string MinimumWeight { get; set; }

        /// <summary>
        ///  The modifier that is applied to the weight
        /// </summary>
        [JsonProperty("weightModifier")]
        public string WeightModifier { get; set; }


        /// <summary>
        ///  The name of the species homeworld
        /// </summary>
        [JsonProperty("homeworld")]
        public string Homeworld { get; set; }

        /// <summary>
        ///  The languages the species (generally) knows by default
        /// </summary>
        [JsonProperty("defaultLanguages")]
        public string DefaultLanguagesJson { get; set; }

        /// <summary>
        ///  Other proficiencies that the species gets (duplicated from other attributes etc)
        /// </summary>
        /// <remarks>This is used to help tagging of the species</remarks>
        [JsonIgnore]
        public IEnumerable<string> DefaultLanguages
        {
            get => string.IsNullOrEmpty(this.DefaultLanguagesJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(this.DefaultLanguagesJson);
            set => this.DefaultLanguagesJson = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        ///  Companies that make this droid type
        /// </summary>
        [JsonProperty("manufacturers")]
        public string ManufacturersJson { get; set; }

        /// <summary>
        ///  Other proficiencies that the species gets (duplicated from other attributes etc)
        /// </summary>
        /// <remarks>This is used to help tagging of the species</remarks>
        [JsonIgnore]
        public IEnumerable<string> Manufacturers
        {
            get => string.IsNullOrEmpty(this.ManufacturersJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(this.ManufacturersJson);
            set => this.ManufacturersJson = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        ///  Valid droid color schemes
        /// </summary>
        [JsonProperty("colorSchemes")]
        public string ColorSchemesJson { get; set; }

        /// <summary>
        ///  Other proficiencies that the species gets (duplicated from other attributes etc)
        /// </summary>
        /// <remarks>This is used to help tagging of the species</remarks>
        [JsonIgnore]
        public IEnumerable<string> ColorSchemes
        {
            get => string.IsNullOrEmpty(this.ColorSchemesJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(this.ColorSchemesJson);
            set => this.ColorSchemesJson = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        ///  The biology and appearance description of the species
        /// </summary>
        [JsonProperty("biologyMarkdown")]
        public string BiologyMarkdown { get; set; }
        
        /// <summary>
        ///  Name of the Biology section differs (based on droids etc)
        /// </summary>
        [JsonProperty("biologyName")]
        public string BiologyName { get; set; }

        /// <summary>
        ///  The society and culture markdown of the species
        /// </summary>
        [JsonProperty("societyMarkdown")]
        public string SocietyMarkdown { get; set; }

        /// <summary>
        ///  The short description that is displayed before the common names
        /// </summary>
        [JsonProperty("nameMarkdown")]
        public string NameMarkdown { get; set; }

        /// <summary>
        ///  Common female names of the species
        /// </summary>
        [JsonProperty("femaleNames")]
        public string FemaleNamesJson { get; set; } = "";

        /// <summary>
        ///  Other proficiencies that the species gets (duplicated from other attributes etc)
        /// </summary>
        /// <remarks>This is used to help tagging of the species</remarks>
        [JsonIgnore]
        public IEnumerable<string> FemaleNames
        {
            get => string.IsNullOrEmpty(this.FemaleNamesJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(this.FemaleNamesJson);
            set => this.FemaleNamesJson = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        ///  Common male names of the species
        /// </summary>
        [JsonProperty("maleNames")]
        public string MaleNamesJson { get; set; }

        /// <summary>
        ///  Other proficiencies that the species gets (duplicated from other attributes etc)
        /// </summary>
        /// <remarks>This is used to help tagging of the species</remarks>
        [JsonIgnore]
        public IEnumerable<string> MaleNames
        {
            get => string.IsNullOrEmpty(this.MaleNamesJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(this.MaleNamesJson);
            set => this.MaleNamesJson = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        ///  Common surnames for the species
        /// </summary>
        [JsonProperty("surnames")]
        public string SurnamesJson { get; set; }

        /// <summary>
        ///  Other proficiencies that the species gets (duplicated from other attributes etc)
        /// </summary>
        /// <remarks>This is used to help tagging of the species</remarks>
        [JsonIgnore]
        public IEnumerable<string> Surnames
        {
            get => string.IsNullOrEmpty(this.SurnamesJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(this.SurnamesJson);
            set => this.SurnamesJson = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        ///  Common age milestones for the species
        /// </summary>
        [JsonProperty("age")]
        public string Age { get; set; }

        /// <summary>
        ///  Alignment tendencies for the species
        /// </summary>
        [JsonProperty("alignment")]
        public string Alignment { get; set; }

        /// <summary>
        ///  The size for the species
        /// </summary>
        [JsonProperty("size")]
        public Size Size { get; set; }

        /// <summary>
        ///  The size description of the species
        /// </summary>
        [JsonProperty("sizeMarkdown")]
        public string SizeMarkdown { get; set; }

        /// <summary>
        ///  The speed description of the species
        /// </summary>
        [JsonProperty("speedMarkdown")]
        public string SpeedMarkdown { get; set; }


        /// <summary>
        ///  Other attribute KvPair for the species
        /// </summary>
        [JsonProperty("otherAttributes")]
        public string OtherAttributesJson { get; set; } = "";

        /// <summary>
        ///  The JSON representation of all statistic increases this species gets (used for tagging)
        /// </summary>
        [JsonProperty("statisticIncreases")]
        public string StatisticIncreasesJson { get; set; } = "";

        /// <summary>
        ///  Other proficiencies that the species gets (duplicated from other attributes etc)
        /// </summary>
        /// <remarks>This is used to help tagging of the species</remarks>
        [JsonProperty("proficienciesGained")]
        public string ProficienciesGainedJson { get; set; } = "";

        /// <summary>
        /// The value for statistic increases that will be displayed in the text
        /// </summary>
        [JsonProperty("statisticIncreaseMarkdown")]
        public string StatisticIncreaseMarkdown { get; set; }

        /// <summary>
        ///  Indicates that this species has darkvision
        /// </summary>
        [JsonProperty("darkvision")]
        public bool Darkvision { get; set; }

        /// <summary>
        /// Markdown for the utility section of droids
        /// </summary>
        [JsonProperty("utilityMarkdown")]
        public string UtilityMarkdown { get; set; }

        /// <summary>
        /// The name of the note that is provided to GMs
        /// </summary>
        [JsonProperty("gmNoteName")]
        public string GmNoteName { get; set; }

        /// <summary>
        ///  The contents of the note to the GM
        /// </summary>
        [JsonProperty("gmNote")]
        public string GmNote { get; set; }

        /// <summary>
        /// List of common first names for the species
        /// </summary>
        [JsonProperty("firstNames")]
        public string FirstNamesJson { get; set; } = "";

        /// <summary>
        ///  Other proficiencies that the species gets (duplicated from other attributes etc)
        /// </summary>
        /// <remarks>This is used to help tagging of the species</remarks>
        [JsonIgnore]
        public IEnumerable<string> FirstNames
        {
            get => string.IsNullOrEmpty(this.FirstNamesJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(this.FirstNamesJson);
            set => this.FirstNamesJson = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// List of common first names for the species
        /// </summary>
        [JsonProperty("names")]
        public string NameJson { get; set; }

        /// <summary>
        ///  Other proficiencies that the species gets (duplicated from other attributes etc)
        /// </summary>
        /// <remarks>This is used to help tagging of the species</remarks>
        [JsonIgnore]
        public IEnumerable<string> Names
        {
            get => string.IsNullOrEmpty(this.NameJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(this.NameJson);
            set => this.NameJson = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// List of common nicknames for the species
        /// </summary>
        [JsonProperty("nicknames")]
        public string NicknamesJson { get; set; } = "";

        /// <summary>
        ///  Other proficiencies that the species gets (duplicated from other attributes etc)
        /// </summary>
        /// <remarks>This is used to help tagging of the species</remarks>
        [JsonIgnore]
        public IEnumerable<string> Nicknames
        {
            get => string.IsNullOrEmpty(this.NicknamesJson) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(this.NicknamesJson);
            set => this.NicknamesJson = JsonConvert.SerializeObject(value);
        }
    }
}