using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Parser.Localization;

namespace StarWars5e.Parser.Processors
{
    public interface IBaseProcessor<T>
    {
        //Task<List<T>> Process(string location, ILocalization strings);
        Task<List<T>> Process(List<string> locations, ILocalization strings);
    }
}
