using System;
using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;

namespace StarWars5e.MonsterManual.Parser.SectionProcessors
{
    class SizeSectionProcessor : IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\*{1}");

        public MonsterOld Process(MonsterOld monster, string input)
        {
            try
            {
                // TODO: PROBLEM here
                // "Name": "Swarm of<br> Monkey-Lizards",
                // "Type": "beast",
                // "Size": "Medium swarm of Tiny",
                var content = input.Substring(2);
                var groups = content.Split(',');
                monster.MonsterType= this.GetType(groups[0].Trim());
                var size = groups[0];
                if (size.Contains('('))
                {
                    var ix = size.IndexOf('(');
                    size = size.Substring(0, ix).Trim();
                }
                monster.Size = size.Substring(0, size.LastIndexOf(' '));
                if (groups.Length == 1)
                {
                    ;
                }
                monster.Alignment = groups[1].Trim().Trim('*');
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return monster;
        }

        private string GetType(string input)
        {
            input = input.ToLower();
            if (input.Contains("beast"))
            {
                return "beast";
            }

            if (input.Contains("construct"))
            {
                return "construct";
            }

            if (input.Contains("droid"))
            {
                return "droid";
            }
            if (input.Contains("humanoid"))
            {
                return "humanoid";
            }

            if (input.Contains("force-wielder"))
            {
                return "force-wielder";
            }
            return "Unknown";
        }

        public MonsterSections IsMatch(string input)
        {
            return this.regex.IsMatch(input)
                ? MonsterSections.Size
                : MonsterSections.Unknown;
        }
    }
}