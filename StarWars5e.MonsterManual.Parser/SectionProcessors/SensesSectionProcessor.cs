using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;
using StarWars5e.Models.Utils;

namespace StarWars5e.MonsterManual.Parser.SectionProcessors
{
    class SensesSectionProcessor : IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\s-\s\*\*Senses\*\*");
        private readonly Regex passiveRegex = new Regex(@"passive Perception");
        private readonly Regex feetRegex = new Regex(@"ft\.");

        public MonsterOld Process(MonsterOld monster, string input)
        {
            try
            {
                var list = new List<KvPair>();
                var toRemove = "> - **Senses** ";
                var sensesVal = input.Substring(toRemove.Length);
                var senses = sensesVal.Split(',');
                if (senses[0].Length == 1)
                {
                    return monster;
                }
                foreach (var sense in senses)
                {
                    var trimmed = sense.Trim();
                    if (this.passiveRegex.IsMatch(trimmed))
                    {
                        var remaining = this.passiveRegex.Replace(trimmed, "");
                        list.Add(new KvPair { Name = "passive Perception", Value = remaining.Trim() });
                    }
                    else
                    {
                        var kv = new KvPair();
                        var remainder = regex.Replace(trimmed, "").Trim();
                        var split = remainder.Split(' ');
                        kv.Name = split[0];
                        kv.Value = split.Length > 1 ? split[1] : "";
                        list.Add(kv);
                    }

                }

                monster.Senses = list;
                return monster;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public MonsterSections IsMatch(string input)
        {
            return this.regex.IsMatch(input)
                ? MonsterSections.Senses
                : MonsterSections.Unknown;
        }
    }
}