using System.Collections.Generic;
using Newtonsoft.Json;

namespace StarWars5e.Models
{
    public class Feat : BaseEntity
    {
        public string Name { get; set; }
        public string Prerequisite { get; set; }
        public string Text { get; set; }
        public List<string> AttributesIncreased { get; set; }
        public string AttributesIncreasedJson
        {
            get => AttributesIncreased == null ? "" : JsonConvert.SerializeObject(AttributesIncreased);
            set => AttributesIncreased = JsonConvert.DeserializeObject<List<string>>(value);
        }
    }
}
