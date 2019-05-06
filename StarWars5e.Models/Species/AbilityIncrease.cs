using System.Collections.Generic;
using Newtonsoft.Json;

namespace StarWars5e.Models.Species
{
    public class AbilityIncrease
    {
        public AbilityIncrease()
        {
            Abilities = new List<string>();
        }
        public List<string> Abilities { get; set; }
        public string AbilitiesJson
        {
            get => Abilities == null ? "" : JsonConvert.SerializeObject(Abilities);
            set => Abilities = JsonConvert.DeserializeObject<List<string>>(value);
        }
        public int? Amount { get; set; }
    }
}
