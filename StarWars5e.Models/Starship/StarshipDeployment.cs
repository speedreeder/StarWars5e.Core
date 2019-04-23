using System.Collections.Generic;
using Newtonsoft.Json;

namespace StarWars5e.Models.Starship
{
    public class StarshipDeployment: BaseEntity
    {
        public StarshipDeployment()
        {
            Features = new List<StarshipFeature>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string FlavorText { get; set; }
        public string FeatureText { get; set; }
        public List<StarshipFeature> Features { get; set; }
        public string FeaturesJson
        {
            get => Features == null ? "" : JsonConvert.SerializeObject(Features);
            set => Features = JsonConvert.DeserializeObject<List<StarshipFeature>>(value);

        }
    }
}
