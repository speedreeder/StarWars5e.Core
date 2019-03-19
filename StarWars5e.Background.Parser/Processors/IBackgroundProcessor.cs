using System.Collections.Generic;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Background.Parser.Processors
{
    public interface IBackgroundProcessor
    {
        BackgroundSection Section { get; }

        bool IsMatch(string line);

        BackgroundViewModel Process(List<string> lines, BackgroundViewModel background);
    }
}