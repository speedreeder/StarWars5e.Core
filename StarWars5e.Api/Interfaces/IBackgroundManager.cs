using System.Threading.Tasks;
using StarWars5e.Models.Background;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Interfaces
{
    public interface IBackgroundManager
    {
        Task<PagedSearchResult<Background>> SearchBackgrounds(BackgroundSearch classSearch);
    }
}
