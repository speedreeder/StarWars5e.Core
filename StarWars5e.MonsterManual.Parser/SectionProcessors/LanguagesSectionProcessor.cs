using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;

namespace StarWars5e.MonsterManual.Parser.SectionProcessors
{
    class LanguagesSectionProcessor : IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\s-\s\*\*Languages\*\*");
        private readonly Regex andMoreRegex = new Regex(@"and one other");
        public MonsterSections IsMatch(string input)
        {
            return this.regex.IsMatch(input)
                ? MonsterSections.Languages
                : MonsterSections.Unknown;
        }

        public Monster Process(Monster monster, string input)
        {
            var toRemove = "> - **Languages** ";
            var val = input.Substring(toRemove.Length).Trim();
            var languages = val.Split(',');
            var langs = new List<string>();
            foreach (var language in languages)
            {
                if (language.Length > 1)
                {
                    if (andMoreRegex.IsMatch(language))
                    {
                        var final = andMoreRegex.Replace(language, "");
                        langs.Add(final.Trim());
                        langs.Add("and one other");
                    }
                    else
                    {
                        langs.Add(language.Trim());
                    }
                }
            }

            monster.Languages = langs;
            return monster;
        }
    }
}