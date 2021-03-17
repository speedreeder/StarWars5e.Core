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
                case Language.en:
                    return new LocalizationEn();
                case Language.ru:
                    return new LocalizationRu();
                case Language.gu:
                    return new LocalizationGu();
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }
    }
}
