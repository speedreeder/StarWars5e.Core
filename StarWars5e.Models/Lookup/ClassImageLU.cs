using Microsoft.WindowsAzure.Storage.Table;

namespace StarWars5e.Models.Lookup
{
    public class ClassImageLU : TableEntity
    {
        public string Class { get; set; }
        public string URL { get; set; }
    }
}
