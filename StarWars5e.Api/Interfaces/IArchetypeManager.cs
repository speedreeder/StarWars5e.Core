using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models.Class;
using StarWars5e.Models.Search;

namespace StarWars5e.Api.Interfaces
{
    public interface IArchetypeManager
    {
        Task<IEnumerable<Archetype>> SearchArchetypes(ArchetypeSearch archetypeSearch);
    }
}
