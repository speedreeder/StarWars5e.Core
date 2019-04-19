using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Models.Class;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;
using Wolnik.Azure.TableStorage.Repository;

namespace StarWars5e.Api.Managers
{
    public class ArchetypeManager : IArchetypeManager
    {
        private readonly ITableStorage _tableStorage;

        public ArchetypeManager(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }
        public async Task<PagedSearchResult<Archetype>> SearchArchetypes(ArchetypeSearch archetypeSearch)
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
                filter = $"{filter} ContentType eq '{archetypeSearch.ContentType.ToString()}'";
            }

            var query = new TableQuery<Archetype>().Where(filter);
            var archetypes = await _tableStorage.QueryAsync("archetypes", query);
            
            return new PagedSearchResult<Archetype>(archetypes.ToList(), archetypeSearch.PageSize, archetypeSearch.CurrentPage); 
        }
    }
}
