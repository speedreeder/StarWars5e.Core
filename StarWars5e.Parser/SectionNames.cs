using System;
using System.Collections.Generic;
using System.Text;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Search;

namespace StarWars5e.Parser
{
    public static class SectionNames
    {
        public static Dictionary<string, GlobalSearchTermType> PHBChapterOneSections { get; } =
            new Dictionary<string, GlobalSearchTermType>
            {
                { "Chapter 1: Step-By-Step Characters", GlobalSearchTermType.Rule },
                { "Level", GlobalSearchTermType.Rule},
                { "Hit Points and Hit Dice", GlobalSearchTermType.Rule},
                { "Proficiency Bonus", GlobalSearchTermType.Rule},
                { "Variant: Customizing Ability Scores", GlobalSearchTermType.VariantRule},
                { "Ability Score Point Cost", GlobalSearchTermType.Table},
                { "Ability Scores and Modifiers", GlobalSearchTermType.Table},
                { "Armor Class", GlobalSearchTermType.Rule},
                { "Weapons", GlobalSearchTermType.Rule },


            };

        public static Dictionary<string, GlobalSearchTermType> PHBChapterTwoSections { get; } =
            new Dictionary<string, GlobalSearchTermType>
            {
                { "Chapter 2: Species", GlobalSearchTermType.Rule },
                { "Choosing A Species", GlobalSearchTermType.Rule},
                { "Ability Score Increase", GlobalSearchTermType.Rule},
                { "Age", GlobalSearchTermType.Rule},
                { "Alignment", GlobalSearchTermType.Rule},
                { "Size", GlobalSearchTermType.Rule},
                { "Speed", GlobalSearchTermType.Rule},
                { "Languages", GlobalSearchTermType.Rule}
            };

        public static Dictionary<string, GlobalSearchTermType> PHBChapterThreeSections { get; } =
            new Dictionary<string, GlobalSearchTermType>
            {
                { "Chapter 3: Classes", GlobalSearchTermType.Rule}
            };

        public static Dictionary<string, GlobalSearchTermType> PHBChapterFourSections { get; } =
            new Dictionary<string, GlobalSearchTermType>
            {
                { "Chapter 4: Personality And Backgrounds", GlobalSearchTermType.Rule},
                { "Name", GlobalSearchTermType.Rule},
                { "Sex", GlobalSearchTermType.Rule},
                { "Height And Weight", GlobalSearchTermType.Rule},
                { "Other Physical Characteristics", GlobalSearchTermType.Rule},
                { "Alignment", GlobalSearchTermType.Rule},
                { "Languages", GlobalSearchTermType.Rule},
                { "Personal Characteristics", GlobalSearchTermType.Rule},
                { "Personal Characteristics", GlobalSearchTermType.Rule}

            };


    }
}
