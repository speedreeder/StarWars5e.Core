using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;
using StarWars5e.Models.Utils;

namespace StarWars5e.MonsterManual.Parser.SectionProcessors
{
    class LegendaryActionsProcessor : IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\s###\sLeg");
        private readonly Regex titleRegex = new Regex(@"\*\*(?'Title'.*)\*\*");

        public MonsterSections IsMatch(string input)
        {
            return this.regex.IsMatch(input)
                ? MonsterSections.LegendaryActions
                : MonsterSections.Unknown;
        }

        public Monster Process(Monster monster, string input)
        {
            return monster;
        }

        public Monster Process(Monster monster, List<string> legendaryLines)
        {

            var list = new List<KvPair>();
            var legendaryAction = string.Empty;
            foreach (var legendaryLine in legendaryLines)
            {
                if (legendaryLine.Contains("<div"))
                {
                    continue;
                }
                var getMore = false;
                var line = legendaryLine.Replace("> ", "");
                if (line.EndsWith("<br>"))
                {
                    line = line.Replace("<br>", Environment.NewLine);
                    getMore = true;
                }
                legendaryAction += line;

                if (!getMore)
                {
                    var title = this.ExtractTitle(legendaryAction);
                    var value = this.titleRegex.Replace(legendaryAction, "");
                    if (title == "" && string.IsNullOrEmpty(monster.LegendaryActionsDescription))
                    {
                        monster.LegendaryActionsDescription = value;
                    }
                    else
                    {
                        value = value.Trim('-').Trim();
                        var act = new KvPair { Name = title, Value = value };
                        list.Add(act);
                    }
                    legendaryAction = string.Empty;
                }
            }

            monster.LegendaryActions = list;
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
    }
}