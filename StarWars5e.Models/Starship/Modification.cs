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

        public string Name { get; set; }

        [IgnoreProperty]
        public ModificationType TypeEnum { get; set; }
        public int Type
        {
            get => (int)TypeEnum;
            set => TypeEnum = (ModificationType)value;
        }

        [IgnoreProperty]
        public List<string> Prerequisites { get; set; }
        public string PrerequisitesJson
        {
            get => Prerequisites == null ? "" : JsonConvert.SerializeObject(Prerequisites);
            set => Prerequisites = JsonConvert.DeserializeObject<List<string>>(value);

        }

        public string Content { get; set; }
    }
}
