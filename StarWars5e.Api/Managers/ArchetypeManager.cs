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
    public class ArchetypeManager : IArchetypeManager
    {
        private readonly IAzureTableStorage _tableStorage;

        public ArchetypeManager(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }
        public async Task<PagedSearchResult<Archetype>> SearchArchetypes(ArchetypeSearch archetypeSearch, Language language)
        {
            var filter = "";
            if (!string.IsNullOrEmpty(archetypeSearch.Class))
            {
                filter = $"ClassName eq '{archetypeSearch.Class}'";
            }
            if (!string.IsNullOrEmpty(archetypeSearch.Name))
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} Name eq '{archetypeSearch.Name}'";
            }
            if (archetypeSearch.ContentType.HasValue && archetypeSearch.ContentType != ContentType.None)
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} ContentType eq '{archetypeSearch.ContentType}'";
            }

            var query = new TableQuery<Archetype>().Where(filter);
            var archetypes = await _tableStorage.QueryAsync($"archetypes{language}", query);

            switch (archetypeSearch.ArchetypeSearchOrdering)
            {
                case ArchetypeSearchOrdering.NameAscending:
                    archetypes = archetypes.OrderBy(p => p.Name);
                    break;
                case ArchetypeSearchOrdering.NameDescending:
                    archetypes = archetypes.OrderByDescending(p => p.Name);
                    break;
                case ArchetypeSearchOrdering.ContentTypeAscending:
                    archetypes = archetypes.OrderBy(p => p.ContentType);
                    break;
                case ArchetypeSearchOrdering.ContentTypeDescending:
                    archetypes = archetypes.OrderByDescending(p => p.ContentType);
                    break;
                case ArchetypeSearchOrdering.ClassAscending:
                    archetypes = archetypes.OrderBy(p => p.ClassName);
                    break;
                case ArchetypeSearchOrdering.ClassDescending:
                    archetypes = archetypes.OrderByDescending(p => p.ClassName);
                    break;
            }

            return new PagedSearchResult<Archetype>(archetypes.ToList(), archetypeSearch.PageSize, archetypeSearch.CurrentPage); 
        }
    }
}
