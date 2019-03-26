using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace StarWars5e.Models.Starship
{
    public class StarshipVenture: BaseEntity
    {
        public string Name { get; set; }
        public string Content { get; set; }

        [IgnoreProperty]
        public List<string> Prerequisites { get; set; }
        public string PrerequisitesJson
        {
            get => Prerequisites == null ? "" : JsonConvert.SerializeObject(Prerequisites);
            set => Prerequisites = JsonConvert.DeserializeObject<List<string>>(value);

        }
    }
}
