using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;
using StarWars5e.Models.Utils;

namespace StarWars.MonsterManual.Parser.SectionProcessors
{
    class ActionsSectionProcessor : IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\s###\sAction");
        private readonly Regex titleRegex = new Regex(@"\*\*\*(?'Title'.*)\*\*\*");

        public Monster Process(Monster monster, string input)
        {
            // find the thing we care about....
            return monster;
        }

        public MonsterSections IsMatch(string input)
        {
            return this.regex.IsMatch(input)
                ? MonsterSections.Actions
                : MonsterSections.Unknown;
        }

        public Monster Process(Monster monster, List<string> actionLines)
        {
            //TODO: Long multi line things ain't working
            /**
             * Orbalisk
             * "Actions": [
      {
        "Name": "Attach.",
        "Value": " *Melee Weapon Attack:* +6 to hit, reach 5 ft., one creature. Hit: 7 (1d8 + 3) kinetic damage, and the orbalisk attaches to the target. Once attached to a target, every day that passes the orbalisk will reproduce through fragmentation, so their numbers will be multiplied by 2. As the orbalisk covers the host�s body, the following effects will take place (percentages relative to HP):"
      },
      {
        "Name": "",
        "Value": "0-25%: Host�s max HP is reduced by the number of orbalisk."
      },
      {
        "Name": "",
        "Value": "25-50%: Host�s max HP is reduced by the number of orbalisk. The host gains a +2 bonus to AC, and Wisdom is decreased by 1."
      },
      {
        "Name": "",
        "Value": "50-70%: Host�s max HP is reduced by the number of orbalisk. The host gains a +3 bonus to AC, Wisdom is decreased by 2, and max force points are reduced by one-third (rounded down)."
      },
      {
        "Name": "",
        "Value": "70-90%: Host�s max HP is reduced by the number of orbalisk. The host gains a +4 bonus to AC, Wisdom is decreased by 3, and max force points are reduced by two-thirds (rounded up)."
      },
      {
        "Name": "",
        "Value": "90-99%: Host�s max HP is reduced by the number of orbalisk. The host gains a +5 bonus to AC, Wisdom is decreased by 4, and max force points are reduced to 0."
      },
      {
        "Name": "",
        "Value": "100%: Host dies."
      },
             */
            var list = new List<KvPair>();
            var currentAction = string.Empty;
            foreach (var actionLine in actionLines)
            {
                var getMore = false;
                var line = actionLine.Replace("> ", "");
                if (line.EndsWith("<br>"))
                {
                    line = line.Replace("<br>", Environment.NewLine);
                    getMore = true;
                }
                currentAction += line;

                if (!getMore)
                {
                    var title = this.ExtractTitle(currentAction);
                    if (!string.IsNullOrEmpty(title))
                    {
                        // there can be invalid items like a random div
                        var value = this.titleRegex.Replace(currentAction, "").Trim().Trim('*').Trim();
                        var act = new KvPair { Name = title, Value = value.Trim('>').Trim() };
                        list.Add(act);
                    }
                    
                    currentAction = string.Empty;
                }
            }

            monster.Actions = list;
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