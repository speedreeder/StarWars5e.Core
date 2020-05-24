using System;
using StarWars5e.Models.Enums;

namespace StarWars5e.Parser.Globalization
{
    public static class GlobalizationFactory
    {
        public static IGlobalization Get(Language language)
        {
            switch (language)
            {
                case Language.En:
                    return new GlobalizationEn();
                case Language.Ru:
                    return new GlobalizationRu();
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }
    }
}
