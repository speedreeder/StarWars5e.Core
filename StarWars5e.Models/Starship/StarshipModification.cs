using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Starship
{
    public class StarshipModification : BaseEntity
    {
        public string Name { get; set; }

        [IgnoreProperty]
        public StarshipModificationType TypeEnum { get; set; }
        public string Type
        {
            get => TypeEnum.ToString();
            set => TypeEnum = Enum.Parse<StarshipModificationType>(value);
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
