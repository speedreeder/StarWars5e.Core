using System;
using System.Collections.Generic;
using System.Linq;
using StarWars5e.Models.Enums;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Powers.Parser.Processors
{
    /// <summary>
    /// This builds the level and alignment for a power
    /// </summary>
    public class LevelProcessor : IPowerProcessor
    {
        public PowerSection Section { get; } = PowerSection.Level;

        public PowerViewModel Process(List<string> sectionContent, PowerViewModel vm)
        {
            if (sectionContent.Count > 1)
            {
                throw new ArgumentException("Level should never be more than a single line");
            }

            try
            {
                var fullText = sectionContent[0].Trim('_');
                vm.Level = this.GetLevelValue(fullText);
                vm.PowerAlignment = this.GetAlignment(fullText);
                return vm;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private Alignment GetAlignment(string input)
        {
            var lower = input.ToLower();
            if (lower.Contains("dark"))
            {
                return Alignment.DarkSide;
            }

            if (lower.Contains("light"))
            {
                return Alignment.LightSide;
            }

            return Alignment.Neutral;
        }


        private int GetLevelValue(string input)
        {
            if (input.ToLower().Contains("at-will"))
            {
                return -1;
            }

            var strValueOfInt = new String(input.Where(Char.IsDigit).ToArray());
            return int.Parse(strValueOfInt);
        }
    }
}