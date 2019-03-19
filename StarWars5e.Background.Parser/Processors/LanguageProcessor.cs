using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Background.Parser.Processors
{
    public class LanguageProcessor : IBackgroundProcessor
    {
        private Regex regex = new Regex(@"-\s\*\*Languages:\*\*");
        private string toRemove = "- **Languages:**";

        public BackgroundSection Section { get; } = BackgroundSection.Language;

        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public BackgroundViewModel Process(List<string> lines, BackgroundViewModel background)
        {
            if (lines.Count > 1)
            {
                throw new ArgumentException("The languages line should only be a single line");
            }
            background.LanguageMarkdown = lines[0].Substring(this.toRemove.Length).Trim();
            return background;
        }
    }
}