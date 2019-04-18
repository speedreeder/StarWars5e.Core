using System.Collections.Generic;
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
        public async Task<IEnumerable<Archetype>> SearchArchetypes(ArchetypeSearch archetypeSearch)
        {

            var filter = "";
            if (!string.IsNullOrEmpty(archetypeSearch.Class))
            {
                filter = $"{filter} and ClassName eq '${archetypeSearch.Class}'";
            }
            if (!string.IsNullOrEmpty(archetypeSearch.Name))
            {
                filter = $"{filter} and Name eq '${archetypeSearch.Name}'";
            }
            if (!archetypeSearch.ContentType.HasValue && archetypeSearch.ContentType.Value != ContentType.None)
            {
                filter = $"{filter} and ContentType eq '${archetypeSearch.ContentType.ToString()}'";
            }

            var query = new TableQuery<Archetype>().Where(filter);
            var archetypes = await _tableStorage.QueryAsync("archetypes", query);

            return archetypes;
        }
    }
}
