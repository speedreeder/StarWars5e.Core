using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Api.Storage;
using StarWars5e.Models.Class;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Managers
{
    public class ClassManager : IClassManager
    {
        private readonly IAzureTableStorage _tableStorage;

        public ClassManager(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<PagedSearchResult<Class>> SearchClasses(ClassSearch classSearch)
        {
            var filter = "";
            if (!string.IsNullOrEmpty(classSearch.Name))
            {
                filter = $"Name eq '{classSearch.Name}'";
            }
            if (classSearch.ContentType.HasValue && classSearch.ContentType != ContentType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} ContentType eq '{classSearch.ContentType}'";
            }

            var query = new TableQuery<Class>().Where(filter);
            var classes = await _tableStorage.QueryAsync("classes", query);

            switch (classSearch.ClassSearchOrdering)
            {
                case ClassSearchOrdering.NameAscending:
                    classes = classes.OrderBy(p => p.Name);
                    break;
                case ClassSearchOrdering.NameDescending:
                    classes = classes.OrderByDescending(p => p.Name);
                    break;
                case ClassSearchOrdering.ContentTypeAscending:
                    classes = classes.OrderBy(p => p.ContentType);
                    break;
                case ClassSearchOrdering.ContentTypeDescending:
                    classes = classes.OrderByDescending(p => p.ContentType);
                    break;
            }

            return new PagedSearchResult<Class>(classes.ToList(), classSearch.PageSize, classSearch.CurrentPage);
        }
    }
}
