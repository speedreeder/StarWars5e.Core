using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using StarWars5e.Models.Monster;

namespace StarWars5e.Models.Species
{
    public class Species : BaseEntity
    {
        public Species()
        {
            ImageUrls = new List<string>();
            HalfHumanTableEntries = new Dictionary<string, string>();
            Features = new List<Feature>();
        }
        public string Name { get; set; }
        public string SkinColorOptions { get; set; }
        public string HairColorOptions { get; set; }
        public string EyeColorOptions { get; set; }
        public string Distinctions { get; set; }
        public string HeightAverage { get; set; }
        public string HeightRollMod { get; set; }
        public string WeightAverage { get; set; }
        public string WeightRollMod { get; set; }
        public string Homeworld { get; set; }
        public string FlavorText { get; set; }
        public string ColorScheme { get; set; }
        public string Manufacturer { get; set; }
        public string Language { get; set; }
        public List<Trait> Traits { get; set; }
        public string TraitJson {
            get => Traits == null ? "" : JsonConvert.SerializeObject(Traits);
            set => Traits = JsonConvert.DeserializeObject<List<Trait>>(value);
        }
        public List<List<AbilityIncrease>> AbilitiesIncreased { get; set; }
        public string AbilitiesIncreasedJson
        {
            get => AbilitiesIncreased == null ? "" : JsonConvert.SerializeObject(AbilitiesIncreased);
            set => AbilitiesIncreased = JsonConvert.DeserializeObject<List<List<AbilityIncrease>>>(value);
        }
        public List<string> ImageUrls { get; set; }
        public string ImageUrlsJson
        {
            get => ImageUrls == null ? "" : JsonConvert.SerializeObject(ImageUrls);
            set => ImageUrls = JsonConvert.DeserializeObject<List<string>>(value);
        }
        public string Size { get; set; }
        public Dictionary<string,string> HalfHumanTableEntries { get; set; }
        public string HalfHumanTableEntriesJson
        {
            get => HalfHumanTableEntries == null ? "" : JsonConvert.SerializeObject(HalfHumanTableEntries);
            set => HalfHumanTableEntries = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
        }

        [IgnoreProperty]
        public List<Feature> Features { get; set; }
    }
}
