using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;
using StarWars5e.Models.Utils;

namespace StarWars.MonsterManual.Parser.SectionProcessors
{
    class ReactionsSectionProcess : IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\s###\sReactions");
        private readonly Regex titleRegex = new Regex(@"\*\*\*(?'Title'.*)\*\*\*");

        public MonsterSections IsMatch(string input)
        {
            return this.regex.IsMatch(input)
                ? MonsterSections.Reactions
                : MonsterSections.Unknown;
        }

        public Monster Process(Monster monster, string input)
        {
            return monster;
        }

        public Monster Process(Monster monster, List<string> reactionLines)
        {

            var list = new List<KvPair>();
            var currentReaction = string.Empty;
            foreach (var reactionLine in reactionLines)
            {
                var getMore = false;
                var line = reactionLine.Replace("> ", "");
                if (line.EndsWith("<br>"))
                {
                    line = line.Replace("<br>", Environment.NewLine);
                    getMore = true;
                }
                currentReaction += line;

                if (!getMore)
                {
                    var title = this.ExtractTitle(currentReaction);
                    var value = this.titleRegex.Replace(currentReaction, "");
                    if (value != "___" && !string.IsNullOrEmpty(title))
                    {
                        var act = new KvPair { Name = title, Value = value };
                        list.Add(act);
                    }
                    currentReaction = string.Empty;
                }
            }

            monster.Reactions = list;
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