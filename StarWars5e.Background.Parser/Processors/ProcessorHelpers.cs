using System;
using System.Linq;
using StarWars5e.Models.Enums;

namespace StarWars5e.Background.Parser.Processors
{
    public static class ProcessorHelpers
    {
        public static int ExtractNumberFromString(string input)
        {
            var digit = int.Parse(new String(input.Where(Char.IsDigit).ToArray()));
            return digit;
        }

        public static DiceType DetermineDiceType(string line)
        {
            var diceType = ProcessorHelpers.ExtractNumberFromString(line);
            switch (diceType)
            {
                case 6:
                    return DiceType.D6;
                case 8:
                    return DiceType.D8;
                case 10:
                    return DiceType.D10;
                case 12:
                    return DiceType.D12;
                default:
                    ;
                    break;
            }

            return DiceType.D100;
        }
    }
}