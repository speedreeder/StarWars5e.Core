using System.Collections.Generic;
using StarWars5e.Models;

namespace StarWars5e.Parser.Storage
{
    public class FeatureRepository
    {
        public List<Feature> Features { get; set; } = new();
    }
}
