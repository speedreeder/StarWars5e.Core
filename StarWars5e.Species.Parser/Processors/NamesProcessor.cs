using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Species.Parser.Processors
{
    public class NamesProcessor : ISpeciesProcessor
    {
        private Regex regex = new Regex(@"###\sNames");
        private string maleNamesLine = "**Male Names.** ";
        private string femaleNamesLine = "**Female Names.** ";
        private string surnameLine = "**Surnames.** ";
        private string firstNameLine = "**First Names.** ";
        private string namesLine = "**Names.** ";
        private string nicknameLine = "**Nicknames.** ";
        public bool IsMatch(string line)
        {
            return this.regex.IsMatch(line);
        }

        public SpeciesViewModel ProcessSection(List<string> input, SpeciesViewModel species)
        {
            species.FemaleNames = new List<string>();
            species.MaleNames = new List<string>();
            species.Surnames = new List<string>();
            species.FirstNames = new List<string>();
            species.Nicknames = new List<string>();
            foreach (var line in input)
            {
                if (line.StartsWith(this.maleNamesLine))
                {
                    var trimmed = line.Substring(this.maleNamesLine.Length).Trim();
                    var names = trimmed.Split(',').ToList();
                    names = names.Select(l => l.Trim()).ToList();
                    species.MaleNames = names;
                }
                else if (line.StartsWith(this.femaleNamesLine))
                {
                    var trimmed = line.Substring(this.femaleNamesLine.Length).Trim();
                    var names = trimmed.Split(',').ToList();
                    names = names.Select(l => l.Trim()).ToList();
                    species.FemaleNames= names;
                }
                else if (line.StartsWith(this.surnameLine))
                {
                    var trimmed = line.Substring(this.surnameLine.Length).Trim();
                    var names = trimmed.Split(',').ToList();
                    names = names.Select(l => l.Trim()).ToList();
                    species.Surnames= names;
                }
                else if (line.StartsWith(this.firstNameLine))
                {
                    var trimmed = line.Substring(this.firstNameLine.Length).Trim();
                    var names = trimmed.Split(',').ToList();
                    names = names.Select(l => l.Trim()).ToList();
                    species.FirstNames= names;
                }
                else if (line.StartsWith(this.namesLine))
                {
                    var trimmed = line.Substring(this.firstNameLine.Length).Trim();
                    var names = trimmed.Split(',').ToList();
                    names = names.Select(l => l.Trim()).ToList();
                    species.Names = names;
                }
                else if (line.StartsWith(this.nicknameLine))
                {
                    var trimmed = line.Substring(this.nicknameLine.Length).Trim();
                    var names = trimmed.Split(',').ToList();
                    names = names.Select(l => l.Trim()).ToList();
                    species.Nicknames = names;
                }
                else if (this.regex.IsMatch(line))
                {
                    continue;
                }
                else
                {
                    species.NameMarkdown= line;
                }
            }
            return species;
        }

        public SpeciesSection SectionName { get; } = SpeciesSection.CommonNames;
    }
}