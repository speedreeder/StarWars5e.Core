using System;
using System.Collections.Generic;
using StarWars5e.Models.ViewModels;

namespace StarWars.Powers.Parser.Processors
{
    /// <summary>
    ///  This populates the title of a power
    /// </summary>
    public class TitleProcessor : IPowerProcessor
    {
        public PowerSection Section { get; } = PowerSection.Title;

        public PowerViewModel Process(List<string> sectionContent, PowerViewModel vm)
        {
            if (sectionContent.Count > 1)
            {
                throw new ArgumentException("Title should never be more than a single line");
            }

            var title = sectionContent[0].Trim('#').Trim();
            vm.Name = title;
            return vm;
        }
    }
}