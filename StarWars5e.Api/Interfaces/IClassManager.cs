using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models.Class;

namespace StarWars5e.Api.Interfaces
{
    public interface IClassManager
    {
        Task<IEnumerable<Class>> SearchClasses();
    }
}
