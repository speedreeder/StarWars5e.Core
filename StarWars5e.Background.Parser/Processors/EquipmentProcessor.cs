using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Background.Parser.Processors
{
    public class EquipmentProcessor : IBackgroundProcessor
    {
        private Regex regex = new Regex(@"-\s\*\*Equipment:\*\*");
        private string toRemove = "- **Equipment:**";

        public BackgroundSection Section { get; } = BackgroundSection.Equipment;

        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public BackgroundViewModel Process(List<string> lines, BackgroundViewModel background)
        {
            if (lines.Count > 1)
            {
                throw new ArgumentException("The equipment line should only be a single line");
            }
            background.EquipmentMarkdown = lines[0].Substring(this.toRemove.Length).Trim();

            return background;
        }
    }
}