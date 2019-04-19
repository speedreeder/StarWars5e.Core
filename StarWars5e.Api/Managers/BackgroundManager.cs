using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Background;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Managers
{
    public class BackgroundManager : IBackgroundManager
    {
        private readonly ITableStorage _tableStorage;

        public BackgroundManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<PagedSearchResult<Background>> SearchBackgrounds(BackgroundSearch classSearch)
        {
            var filter = "";
            if (!string.IsNullOrEmpty(classSearch.Name))
            {
                filter = $"Name eq '{classSearch.Name}'";
            }
            if (classSearch.ContentType.HasValue && classSearch.ContentType != ContentType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} ContentType eq '{classSearch.ContentType.ToString()}'";
            }

            var query = new TableQuery<Background>().Where(filter);
            var backgrounds = await _tableStorage.QueryAsync("backgrounds", query);

            return new PagedSearchResult<Background>(backgrounds.ToList(), classSearch.PageSize, classSearch.CurrentPage);
        }
    }
}
