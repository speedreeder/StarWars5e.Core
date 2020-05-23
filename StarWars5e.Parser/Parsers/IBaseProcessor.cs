using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;

namespace StarWars5e.Parser.Parsers
{
    public interface IBaseProcessor<T>
    {
        Task<List<T>> Process(List<string> locations, Language language);
    }
}
