using System.Threading.Tasks;
using StarWars5e.Models.Class;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Interfaces
{
    public interface IClassManager
    {
        Task<PagedSearchResult<Class>> SearchClasses(ClassSearch classSearch);
    }
}
