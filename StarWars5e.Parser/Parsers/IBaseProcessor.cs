using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Parser.Globalization;

namespace StarWars5e.Parser.Parsers
{
    public interface IBaseProcessor<T>
    {
        Task<List<T>> Process(List<string> locations, IGlobalization strings);
    }
}
