using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Class
{
    public class Archetype : BaseEntity
    {
        public Archetype()
        {
            ImageUrls = new List<string>();
        }
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
        public List<string> ImageUrls { get; set; }
        public string ImageUrlsJson
        {
            get => ImageUrls == null ? "" : JsonConvert.SerializeObject(ImageUrls);
            set => ImageUrls = JsonConvert.DeserializeObject<List<string>>(value);
        }
        public double CasterRatio { get; set; }
        public PowerType CasterTypeEnum { get; set; }
        public string CasterType
        {
            get => CasterTypeEnum.ToString();
            set => CasterTypeEnum = Enum.Parse<PowerType>(value);
        }
    }
}
