using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models
{
    public class Modification : TableEntity
    {
        public ModificationType Type { get; set; }
        public string Name { get; set; }
        public List<string> Prerequisites { get; set; }
        public string Content { get; set; }
    }
}
