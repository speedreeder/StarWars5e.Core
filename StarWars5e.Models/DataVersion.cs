using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace StarWars5e.Models
{
    public class DataVersion : TableEntity
    {
        public string Name { get; set; }
        public DateTime LastUpdated { get; set; }
        public double Version { get; set; }
    }
}
