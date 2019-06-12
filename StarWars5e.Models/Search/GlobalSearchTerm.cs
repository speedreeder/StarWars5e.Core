using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Search
{
    public class GlobalSearchTerm : TableEntity
    {
        public string Name { get; set; }
        public GlobalSearchTermType GlobalSearchTermType { get; set; }
        public string Path { get; set; }
        public string FullName { get; set; }
    }
}
