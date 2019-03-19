using System.Collections.Generic;

namespace StarWars5e.MonsterManual.Parser.SectionProcessors
{
    class SectionMatchers
    {
        public static FeaturesSectionProcessor FeatureProc = new FeaturesSectionProcessor();
        public static ActionsSectionProcessor ActionProc = new ActionsSectionProcessor();
        public static ReactionsSectionProcess ReactionProc = new ReactionsSectionProcess();
        public static LegendaryActionsProcessor LegendaryActionProc = new LegendaryActionsProcessor();
        public static List<IMonsterSectionProcessor> Matchers = new List<IMonsterSectionProcessor>
        {
            new NameSectionProcessor(),
            new SizeSectionProcessor(),
            new ArmorClassSectionProcessor(),
            new HitPointSectionProcessor(),
            new SpeedSectionProcessor(),
            new AttributeHeaderSectionProcessor(),
            new AttributeSectionProcessor(),
            new SkillsSectionProcessor(),
            new SavingThrowProcessor(),
            new SensesSectionProcessor(),
            new LanguagesSectionProcessor(),
            new ChallengeSectionProcessor(),
            new FeaturesSectionProcessor(),
            new ActionsSectionProcessor(),
            new ReactionsSectionProcess(),
            new DamageImmunitiesProcessor(),
            new ConditionImmunityProcessor(),
            new DamageResistanceProcessor(),
            new DamageVulnerabilitiesProcessor(),
            new LegendaryActionsProcessor()
        };
    }
}