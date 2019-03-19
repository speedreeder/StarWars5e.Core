using System;
using System.Collections.Generic;
using System.Text;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Powers.Parser.Processors
{
    /// <summary>
    /// This will take care of description and possibly die rolls
    /// </summary>
    public class DescriptionProcessor : IPowerProcessor
    {
        public PowerSection Section { get; } = PowerSection.Description;

        public PowerViewModel Process(List<string> lines, PowerViewModel vm)
        {
            
            var sb = new StringBuilder();
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if ((i + i == lines.Count || i == 0) && string.IsNullOrEmpty(line))
                {
                    // don't append a blank line at the beginning!
                    // don't append a blank line at the end
                    continue;
                }
                if (line == "")
                {
                    sb.AppendLine(Environment.NewLine);
                }
                else
                {
                    //find out if there is a die roll in there
                    sb.AppendLine(line);
                }
            }

            vm.Description = sb.ToString();
            return vm;
        }
    }
}