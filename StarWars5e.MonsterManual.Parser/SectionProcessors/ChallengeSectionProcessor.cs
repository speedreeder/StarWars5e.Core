using System;
using System.Text.RegularExpressions;
using StarWars5e.Models.Monster;

namespace StarWars5e.MonsterManual.Parser.SectionProcessors
{
    class ChallengeSectionProcessor : IMonsterSectionProcessor
    {
        private readonly Regex regex = new Regex(@">\s-\s\*\*Challenge\*\*");

        public MonsterOld Process(MonsterOld monster, string input)
        {
            try
            {
                var toRemove = "> - **Challenge** ";
                var challengeVal = input.Substring(toRemove.Length);
                var split = challengeVal.Split(' ');
                monster.Challenge = split[0];
                monster.ExperiencePoints = this.ExtractValueFromParenthesis(split[1] + split[2]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            // find the thing we care about....
            return monster;
        }

        private string ExtractValueFromParenthesis(string input)
        {

            Regex regex = new Regex(@"\((?'ParensValue'.*)\)");
            string parensValue = string.Empty;
            Match match = regex.Match(input);
            if (match.Success)
            {
                parensValue = match.Groups["ParensValue"].Value;
            }
            Regex xpRegex = new Regex(@"XP");
            return xpRegex.Replace(parensValue, "");
        }

        public MonsterSections IsMatch(string input)
        {
            return this.regex.IsMatch(input)
                ? MonsterSections.Challenge
                : MonsterSections.Unknown;
        }
    }
}