using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Background.Parser.Processors
{
    public class ToolProficiencyProcessor : IBackgroundProcessor
    {
        private Regex regex = new Regex(@"-\s\*\*Tool\sProficiencies:\*\*");
        private string toRemove = "- **Tool Proficiencies:**";

        public BackgroundSection Section { get; } = BackgroundSection.ToolProficiency;

        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public BackgroundViewModel Process(List<string> lines, BackgroundViewModel background)
        {
            if (lines.Count > 1)
            {
                throw new ArgumentException("The Tool proficiency line should only be a single line");
            }
            var trimmed = lines[0].Substring(this.toRemove.Length).Trim();
            background.ToolProficiencyMarkdown = trimmed;
            return background;
        }
    }
}