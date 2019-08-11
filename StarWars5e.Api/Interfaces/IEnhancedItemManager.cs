using System.Threading.Tasks;
using StarWars5e.Models.EnhancedItems;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Interfaces
{
    public interface IEnhancedItemManager
    {
        Task<PagedSearchResult<EnhancedItem>> SearchEnhancedItems(EnhancedItemSearch archetypeSearch);
    }
}
