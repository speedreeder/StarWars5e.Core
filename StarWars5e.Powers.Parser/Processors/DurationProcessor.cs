using System;
using System.Collections.Generic;
using System.Linq;
using StarWars5e.Models.Enums;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Powers.Parser.Processors
{
    /// <summary>
    /// This populates the duration of a power
    /// </summary>
    public class DurationProcessor : IPowerProcessor
    {
        public PowerSection Section { get; } = PowerSection.Duration;
        public PowerViewModel Process(List<string> sectionContent, PowerViewModel vm)
        {
            if (sectionContent.Count > 1)
            {
                throw new ArgumentException("Duration should never exceed one line");
            }

            var trimmed = sectionContent[0].Substring("- **Duration:**".Length).Trim().ToLower();
            vm.RequiresConcentration = this.DetermineConcentration(trimmed);
            vm.DurationLengthModifier = this.DetermineDurationModifier(trimmed);
            if (vm.DurationLengthModifier == Duration.Day
                || vm.DurationLengthModifier == Duration.Minute
                || vm.DurationLengthModifier == Duration.Hour
                || vm.DurationLengthModifier == Duration.Round)
            {
                vm.DurationLength = this.DetermineDurationLength(trimmed);
            }
                return vm;
        }

        private int DetermineDurationLength(string input)
        {
            var strValueOfInt = new String(input.Where(Char.IsDigit).ToArray());
            return int.Parse(strValueOfInt);
        }

        /// <summary>
        /// Determine the type of the duration
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private Duration DetermineDurationModifier(string input)
        {
            if (input.Contains("instant"))
            {
                return Duration.Instantaneous;
            }

            if (input.Contains("minute"))
            {
                return Duration.Minute;
            }

            if (input.Contains("hour"))
            {
                return Duration.Hour;
            }

            if (input.Contains("round"))
            {
                return Duration.Round;
            }

            if (input.Contains("day"))
            {
                return Duration.Day;
            }

            if (input.Contains("dispelled"))
            {
                return Duration.UntilDispelled;
            }

            return Duration.Unknown;
        }

        private bool DetermineConcentration(string input)
        {
            return input.Contains("concentration");
        }
    }
}