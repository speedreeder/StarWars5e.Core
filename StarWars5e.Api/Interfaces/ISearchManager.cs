using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Interfaces
{
    public interface ISearchManager
    {
        Task<IEnumerable<GlobalSearchTerm>> RunGlobalSearch(string searchText, Language language);
    }
}
