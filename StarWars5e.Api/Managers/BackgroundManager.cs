using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Api.Storage;
using StarWars5e.Models.Background;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Managers
{
    public class BackgroundManager : IBackgroundManager
    {
        private readonly IAzureTableStorage _tableStorage;

        public BackgroundManager(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<PagedSearchResult<Background>> SearchBackgrounds(BackgroundSearch backgroundSearch)
        {
            var filter = "";
            if (!string.IsNullOrEmpty(backgroundSearch.Name))
            {
                filter = $"Name eq '{backgroundSearch.Name}'";
            }
            if (backgroundSearch.ContentType.HasValue && backgroundSearch.ContentType != ContentType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} ContentType eq '{backgroundSearch.ContentType.ToString()}'";
            }

            var query = new TableQuery<Background>().Where(filter);
            var backgrounds = await _tableStorage.QueryAsync("backgrounds", query);

            switch (backgroundSearch.BackgroundSearchOrdering)
            {
                case BackgroundSearchOrdering.NameAscending:
                    backgrounds = backgrounds.OrderBy(p => p.Name);
                    break;
                case BackgroundSearchOrdering.NameDescending:
                    backgrounds = backgrounds.OrderByDescending(p => p.Name);
                    break;
                case BackgroundSearchOrdering.ContentTypeAscending:
                    backgrounds = backgrounds.OrderBy(p => p.ContentType);
                    break;
                case BackgroundSearchOrdering.ContentTypeDescending:
                    backgrounds = backgrounds.OrderByDescending(p => p.ContentType);
                    break;
            }

            return new PagedSearchResult<Background>(backgrounds.ToList(), backgroundSearch.PageSize, backgroundSearch.CurrentPage);
        }
    }
}
