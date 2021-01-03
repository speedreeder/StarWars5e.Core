using System;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models
{
    public class FeatureOption : TableEntity
    {
        public string Feature { get; set; }
        public string Metadata { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
    }
}
