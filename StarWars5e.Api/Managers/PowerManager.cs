using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Api.Storage;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Managers
{
    public class PowerManager : IPowerManager
    {
        private readonly IAzureTableStorage _tableStorage;

        public PowerManager(IAzureTableStorage tableStorage)
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

            switch (powerSearch.PowerSearchOrdering)
            {
                case PowerSearchOrdering.NameAscending:
                    powers = powers.OrderBy(p => p.Name);
                    break;
                case PowerSearchOrdering.NameDescending:
                    powers = powers.OrderByDescending(p => p.Name);
                    break;
                case PowerSearchOrdering.ContentTypeAscending:
                    powers = powers.OrderBy(p => p.ContentType);
                    break;
                case PowerSearchOrdering.ContentTypeDescending:
                    powers = powers.OrderByDescending(p => p.ContentType);
                    break;
                case PowerSearchOrdering.LevelAscending:
                    powers = powers.OrderBy(p => p.Level);
                    break;
                case PowerSearchOrdering.LevelDescending:
                    powers = powers.OrderByDescending(p => p.Level);
                    break;
                case PowerSearchOrdering.PowerTypeAscending:
                    powers = powers.OrderBy(p => p.PowerType);
                    break;
                case PowerSearchOrdering.PowerTypeDescending:
                    powers = powers.OrderByDescending(p => p.PowerType);
                    break;
                case PowerSearchOrdering.ForceAlignmentAscending:
                    powers = powers.OrderBy(p => p.ForceAlignment);
                    break;
                case PowerSearchOrdering.ForceAlignmentDescending:
                    powers = powers.OrderByDescending(p => p.ForceAlignment);
                    break;
                case PowerSearchOrdering.IsConcentrationAscending:
                    powers = powers.OrderBy(p => p.Concentration);
                    break;
                case PowerSearchOrdering.IsConcentrationDescending:
                    powers = powers.OrderByDescending(p => p.Concentration);
                    break;
            }

            return new PagedSearchResult<Power>(powers.ToList(), powerSearch.PageSize, powerSearch.CurrentPage);
        }
    }
}
