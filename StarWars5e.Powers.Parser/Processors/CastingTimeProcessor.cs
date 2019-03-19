using System;
using System.Collections.Generic;
using System.Linq;
using StarWars5e.Models.Enums;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Powers.Parser.Processors
{
    /// <summary>
    /// This populates the type and number of units of time that it takes to cast this power.
    /// It also could populate the reaction limits
    /// </summary>
    public class CastingTimeProcessor : IPowerProcessor
    {
        public PowerSection Section { get; } = PowerSection.CastingTime;
        public PowerViewModel Process(List<string> sectionContent, PowerViewModel vm)
        {
            if (sectionContent.Count > 1)
            {
                throw new ArgumentException("Casting time should never be more than a single line");
            }

            try
            {

                var content = sectionContent[0];
                vm.CastingLength = this.DetermineCastingTimes(content);
                return vm;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private IEnumerable<CastingLength> DetermineCastingTimes(string input)
        {
            var lengths = new List<CastingLength>();
            var toRemove = "- **Casting Time:** ";
            var lower = input.Substring(toRemove.Length).ToLower();
            var options = lower.Split(" or ");
            foreach (var option in options)
            {
                var length = new CastingLength();
                if (option.Contains("hour"))
                {
                    length.CastingPeriod = CastingPeriod.Hour;
                    var strValueOfInt = new String(input.Where(Char.IsDigit).ToArray());
                    length.CastingTime = int.Parse(strValueOfInt);

                } else if (option.Contains("bonus"))
                {
                    length.CastingPeriod = CastingPeriod.BonusAction;
                    var strValueOfInt = new String(input.Where(Char.IsDigit).ToArray());
                    length.CastingTime = int.Parse(strValueOfInt);
                } else if (option.Contains("reaction"))
                {
                    length.CastingPeriod = CastingPeriod.Reaction;
                    if (option.Contains(','))
                    {
                        var groups = option.Split(',');
                        var constraint = groups[1].Trim();
                        var strValueOfInt = new String(groups[0].Trim().Where(Char.IsDigit).ToArray());
                        length.CastingTime = int.Parse(strValueOfInt);
                        ;
                    }
                } else if (option.Contains("minute"))
                {
                    length.CastingPeriod = CastingPeriod.Minute;
                    var strValueOfInt = new String(input.Where(Char.IsDigit).ToArray());
                    length.CastingTime = int.Parse(strValueOfInt);

                } else if (option.Contains("action"))
                {
                    length.CastingPeriod = CastingPeriod.Action;
                    var strValueOfInt = new String(input.Where(Char.IsDigit).ToArray());
                    length.CastingTime = int.Parse(strValueOfInt);
                }
                lengths.Add(length);
            }
            return lengths;
        }
    }
}