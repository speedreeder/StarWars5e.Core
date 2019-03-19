using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Background.Parser.Processors
{
    public class FeatureProcessor : IBackgroundProcessor
    {
        private Regex regex = new Regex(@"###\sFeature:");
        private string toRemove = "### Feature:";

        public BackgroundSection Section { get; } = BackgroundSection.Feature;

        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public BackgroundViewModel Process(List<string> lines, BackgroundViewModel background)
        {
            if (lines.Count != 2)
            {
                throw new ArgumentException("The Feature should only be a title line and a single line for description");
            }

            background.FeatureName = lines[0].Substring(this.toRemove.Length).Trim();
            background.FeatureMarkdown = lines[1].Trim();
            return background;
        }
    }
}