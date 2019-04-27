using System.Collections.Generic;
using Newtonsoft.Json;

namespace StarWars5e.Models.Class
{
    public class Archetype : BaseEntity
    {
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string Text { get; set; }
        public string Text2 { get; set; }
        public string LeveledTableHeadersJson { get; set; }
        public Dictionary<int, List<KeyValuePair<string, string>>> LeveledTable { get; set; }
        public string LeveledTableJson
        {
            get => LeveledTable == null ? "" : JsonConvert.SerializeObject(LeveledTable);
            set => LeveledTable = JsonConvert.DeserializeObject<Dictionary<int, List<KeyValuePair<string, string>>>>(value);
        }
    }
}
