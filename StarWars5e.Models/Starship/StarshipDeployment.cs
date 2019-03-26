using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

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
        public List<StarshipFeature> Features { get; set; }
    }
}
