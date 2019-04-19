using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Managers
{
    public class PowerManager : IPowerManager
    {
        private readonly ITableStorage _tableStorage;

        public PowerManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<PagedSearchResult<Power>> SearchPowers(PowerSearch powerSearch)
        {
            var filter = "";
            if (!string.IsNullOrEmpty(powerSearch.Name))
            {
                filter = $"Name eq '{powerSearch.Name}'";
            }
            if (powerSearch.ContentType.HasValue && powerSearch.ContentType != ContentType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} ContentType eq '{powerSearch.ContentType.ToString()}'";
            }
            if (powerSearch.MaxLevel.HasValue)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} Level le {powerSearch.MaxLevel.Value}";
            }
            if (powerSearch.MinLevel.HasValue)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} Level ge {powerSearch.MinLevel.Value}";
            }
            if (powerSearch.PowerType.HasValue && powerSearch.PowerType != PowerType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} PowerType eq '{powerSearch.PowerType.ToString()}'";
            }
            if (powerSearch.ForceAlignment.HasValue && powerSearch.ForceAlignment != ForceAlignment.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} ForceAlignment eq '{powerSearch.ForceAlignment.ToString()}'";
            }
            if (powerSearch.IsConcentration.HasValue)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} Concentration eq {powerSearch.IsConcentration.ToString().ToLower()}";
            }

            var query = new TableQuery<Power>().Where(filter);
            var powers = await _tableStorage.QueryAsync("powers", query);

            return new PagedSearchResult<Power>(powers.ToList(), powerSearch.PageSize, powerSearch.CurrentPage);
        }
    }
}
