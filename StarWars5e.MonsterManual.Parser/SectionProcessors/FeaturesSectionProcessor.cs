using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;
using StarWars5e.Models.Utils;

namespace StarWars5e.MonsterManual.Parser.SectionProcessors
{
    class FeaturesSectionProcessor : IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\s\*{3}");
        private readonly Regex dividerRegex = new Regex(@"___");
        private readonly Regex titleRegex = new Regex(@"\*\*\*(?'Title'.*)\*\*\*");

        public Monster Process(Monster monster, string input)
        {
            // find the thing we care about....
            return monster;
        }

        public MonsterSections IsMatch(string input)
        {
            return this.dividerRegex.IsMatch(input)
                ? MonsterSections.Features
                : MonsterSections.Unknown;
        }

        internal Monster Process(Monster monster, List<string> currentSection)
        {
            var list = new List<KvPair>();
            var currentAction = new StringBuilder();
            foreach (var actionLine in currentSection)
            {
                var getMore = false;
                var line = actionLine.Replace("> ", "");
                if (line.EndsWith("<br>"))
                {
                    line = line.Replace("<br>", Environment.NewLine);
                    getMore= true;
                }
                currentAction.AppendLine(line);
                if (!getMore)
                {
                    var title = this.ExtractTitle(currentAction.ToString());
                    var value = this.titleRegex.Replace(currentAction.ToString(), "");
                    var act = new KvPair {Name = title, Value = value.Trim().Trim('>').Trim()};
                    list.Add(act);
                    currentAction = new StringBuilder();
                }
            }

            monster.Features = list;
            return monster;
        }

        private string ExtractTitle(string input)
        {

            string parensValue = string.Empty;
            Match match = this.titleRegex.Match(input);
            if (match.Success)
            {
                parensValue = match.Groups["Title"].Value;
            }

            return parensValue;
        }

        public bool DoubleCheck(string input)
        {
            return this.regex.IsMatch(input);
        }
    }
}