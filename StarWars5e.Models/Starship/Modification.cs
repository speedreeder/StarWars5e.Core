using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Starship
{
    public class Modification : TableEntity
    {
        public Modification(ModificationType modificationType)
        {
            Type = (int) modificationType;
        }

        public int Type { get; set; }
        public string Name { get; set; }
        public List<string> Prerequisites { get; set; }
        public string PrerequisitesString
        {
            get => Prerequisites == null ? "" : JsonConvert.SerializeObject(Prerequisites);
            set => Prerequisites = JsonConvert.DeserializeObject<List<string>>(value);

        }
        public string Content { get; set; }
    }
}
