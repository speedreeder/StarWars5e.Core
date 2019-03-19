using System.Collections.Generic;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Powers.Parser.Processors
{
    public interface IPowerProcessor
    {
        PowerSection Section { get; }

        PowerViewModel Process(List<string> sectionContent, PowerViewModel vm);
    }
}