using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Background.Parser.Processors
{
    public class NameProcessor : IBackgroundProcessor
    {
        private Regex regex = new Regex(@"^##\s");
        private string toRemove = "## ";

        public BackgroundSection Section { get; } = BackgroundSection.Name;

        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public BackgroundViewModel Process(List<string> lines, BackgroundViewModel background)
        {
            if (lines.Count > 2)
            {
                throw new ArgumentException("The name section should only have a title and single line for description");
            }

            var trimmed = lines[0].Substring(this.toRemove.Length).Trim();
            background.Name = trimmed;
            background.Description = lines[1].Trim();
            return background;
        }
    }
}