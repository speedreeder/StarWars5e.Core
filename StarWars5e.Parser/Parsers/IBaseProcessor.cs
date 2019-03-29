using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarWars5e.Parser.Parsers
{
    public interface IBaseProcessor<T>
    {
        Task<List<T>> Process(string location, bool isRemote = false);
    }
}
