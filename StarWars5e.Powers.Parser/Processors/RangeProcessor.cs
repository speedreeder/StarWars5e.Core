using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using StarWars5e.Models.Enums;
using StarWars5e.Models.ViewModels;

namespace StarWars.Powers.Parser.Processors
{
    /// <summary>
    /// This processor will determine Range, Shape
    /// </summary>
    public class RangeProcessor : IPowerProcessor
    {
        public PowerSection Section { get; } = PowerSection.Range;
        public PowerViewModel Process(List<string> sectionContent, PowerViewModel vm)
        {
            if (sectionContent.Count > 1)
            {
                throw new ArgumentException("Range should never exceed one line");
            }

            var trimmed = sectionContent[0].Substring("- **Range:** ".Length).Trim();
            var nonParens = trimmed.Split('(')[0].ToLower();
            vm.RangeType = this.DetermineRangeType(nonParens);
            vm.Range = this.DetermineRange(nonParens);
            if (trimmed.Contains('('))
            {
                var parensValue = this.ExtractValueFromParenthesis(trimmed);
                vm.PowerShape = this.DetermineShape(parensValue);
                vm.ShapeSize = this.DetermineShapeSize(parensValue);
                vm.ShapeSizeType = this.DetermineShapeType(parensValue)
                    ;
            }
            return vm;
        }

        private int DetermineRange(string input)
        {
            var strValueOfInt = new String(input.Where(Char.IsDigit).ToArray());
            if (string.IsNullOrEmpty(strValueOfInt))
            {
                return 0;
            }
            return int.Parse(strValueOfInt);
        }

        private RangeType DetermineRangeType(string input)
        {
            if (input.Contains("self"))
            {
                return RangeType.Self;
            }

            if (input.Contains("feet"))
            {
                return RangeType.Feet;
            }

            if (input.Contains("touch"))
            {
                return RangeType.Touch;
            }

            return RangeType.Unlimited;
        }

        /// <summary>
        /// Determine if the shape size is measured in feet or inches or miles or whatever
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string DetermineShapeType(string input)
        {
            return "foot"; // apparently that's all there is? I don't feel like writing code that is useless
        }

        private int DetermineShapeSize(string input)
        {
            var strValueOfInt = new String(input.Where(Char.IsDigit).ToArray());
            return int.Parse(strValueOfInt);
        }


        private Shape DetermineShape(string input)
        {
            var lower = input.ToLower();
            if (lower.Contains("line"))
            {
                return Shape.Line;
            }

            if (lower.Contains("cube"))
            {
                return Shape.Cube;
            }

            if (lower.Contains("sphere"))
            {
                return Shape.Sphere;
            }

            if (lower.Contains("cylinder"))
            {
                return Shape.Cylinder;
            }
            if (lower.Contains("radius"))
            {
                return Shape.Radius;
            }
            if (lower.Contains("cone"))
            {
                return Shape.Cone;
            }

            return Shape.None;
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

            return parensValue;
        }
    }
}