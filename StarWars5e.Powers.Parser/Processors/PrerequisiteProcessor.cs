using System;
using System.Collections.Generic;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Powers.Parser.Processors
{
    /// <summary>
    /// This creates a single string that lays out all of the requirements to learn this power
    /// </summary>
    public class PrerequisiteProcessor : IPowerProcessor
    {
        public PowerSection Section { get; } = PowerSection.Prerequisite;
        public PowerViewModel Process(List<string> sectionContent, PowerViewModel vm)
        {
            if (sectionContent.Count > 1)
            {
                throw new ArgumentException("Prereqs should never exceed one line");
            }
            vm.Prerequisite = sectionContent[0].Substring("- **Prerequisite:** ".Length).Trim();
            return vm;
        }
    }
}