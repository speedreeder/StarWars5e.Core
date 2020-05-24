using System;
using StarWars5e.Models.Enums;

namespace StarWars5e.Parser.Localization
{
    public static class LocalizationFactory
    {
        public static ILocalization Get(Language language)
        {
            switch (language)
            {
                case Language.En:
                    return new LocalizationEn();
                case Language.Ru:
                    return new LocalizationRu();
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }
    }
}
