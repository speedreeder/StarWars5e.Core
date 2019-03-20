using System.Collections.Generic;

namespace StarWars5e.Models.Starship
{
    public class StarshipDeployment
    {
        public StarshipDeployment()
        {
            Features = new List<StarshipFeature>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string FlavorText { get; set; }
        public List<StarshipFeature> Features { get; set; }
    }
}
