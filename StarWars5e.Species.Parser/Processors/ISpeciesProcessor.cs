using System.Collections.Generic;
using StarWars5e.Models.ViewModels;

namespace StarWars.Species.Parser.Processors
{
    public interface ISpeciesProcessor
    {
        bool IsMatch(string line);

        SpeciesViewModel ProcessSection(List<string> input, SpeciesViewModel species);
        SpeciesSection SectionName { get; }
    }
}