using System;
using System.Collections.Generic;
using StarWars5e.Models.Enums;

namespace StarWars5e.Parser.Localization
{
    public interface ILocalization
    {
        public Language Language { get; }

        #region Common
        public string Quick { get; }
        public string Build { get; }
        public string Creating { get; }
        public string The { get; }
        public string or { get; }
        public string from { get; }
        public string and { get; }
        public string Armor { get; }
        public string Tools { get;  }
        public string Weapons { get; }
        public string Skills { get; }
        public string one { get; }
        public string two { get; }
        public string three { get; }
        public string four { get; }
        public string five { get; }
        public string six { get; }
        public string seven { get; }
        public string eight { get; }
        public string nine { get; }
        public string Equipment { get; }
        public string Level { get; }
        public string Prerequisite { get; }
        public string Feats { get; }
        public string Any { get; }
        public string ability { get; }
        public string Stun { get; }
        public string Range { get; }
        public string Duration { get; }
        public string Concentration { get; }
        public string CastingTime { get; }
        public string bonus { get; }
        public string reaction { get; }
        public string action { get; }
        public string minute { get; }
        public string hour { get; }
        public string universal { get; }
        public string dark { get; }
        public string light { get; }
        public string tech { get; }
        public string Traits { get; }
        public string Size { get; }
        public string choice { get; }
        public string reload { get; }
        public string range { get; }
        public string PrimaryWeapons { get; }
        public string SecondaryWeapons { get; }
        public string TertiaryWeapons { get; }
        public string QuaternaryWeapons { get; }
        public string feature { get; }
        public string a { get; }
        public string Tier { get; }
        public string Features { get; }
        public string standard { get; }
        public string premium { get; }
        public string prototype { get; }
        public string advanced { get; }
        public string legendary { get; }
        public string artifact { get; }
        public string adventuringgear { get; }
        public string AdventuringGear { get; }
        public string body { get; }
        public string Feet { get; }
        public string Finger { get; }
        public string Hands { get; }
        public string Head { get; }
        public string Neck { get; }
        public string Legs { get; }
        public string Shoulders { get; }
        public string Waist { get; }
        public string Wrists { get; }
        public string armormodification { get; }
        public string reinforcement { get; }
        public string shielding { get; }
        public string overlay { get; }
        public string underlay { get; }
        public string armoring { get; }
        public string armor { get; }
        public string anyheavy { get; }
        public string anymedium { get; }
        public string anylight { get; }
        public string any { get; }
        public string consumable { get; }
        public string adrenal { get; }
        public string explosive { get; }
        public string poison { get; }
        public string stimpac { get; }
        public string barrier { get; }
        public string cyberneticaugmentation { get; }
        public string enhancement { get; }
        public string replacement { get; }
        public string droidcustomization { get; }
        public string part { get; }
        public string protocol { get; }
        public string focusgeneratormodification { get; }
        public string cycler { get; }
        public string emitter { get; }
        public string conductor { get; }
        public string energychannel { get; }
        public string focus { get; }
        public string force { get; }
        public string itemmodification { get; }
        public string augment { get; }
        public string wristpadmodification { get; }
        public string dataport { get; }
        public string storage { get; }
        public string motherboard { get; }
        public string processor { get; }
        public string lightweaponmodification { get; }
        public string lens { get; }
        public string stabilizer { get; }
        public string crystal { get; }
        public string powercell { get; }
        public string vibroweaponmodification { get; }
        public string edge { get; }
        public string vibratorcell { get; }
        public string projector { get; }
        public string grip { get; }
        public string clothingmodification { get; }
        public string inlay { get; }
        public string weave { get; }
        public string blastermodification { get; }
        public string barrel { get; }
        public string targeting { get; }
        public string matrix { get; }
        public string energycore { get; }
        public string shield { get; }
        public string heavy { get; }
        public string medium { get; }
        public string weapon { get; }
        public string anyblaster { get; }
        public string anyvibroweapon { get; }
        public string anylightweapon { get; }
        public string lightweapon { get; }
        public string blaster { get; }
        public string vibroweapon { get; }
        public string valuable { get; }
        public string art { get; }
        public string jewel { get; }
        public string relic { get; }
        public string sculpture { get; }
        public string shiparmor { get; }
        public string shipshield { get; }
        public string shipweapon { get; }
        public string property { get; }
        public string Berserker { get; }
        public string SuggestedCharacteristics { get; }
        public string IWS { get; }
        public string Bulky { get; }
        public string ranged { get; }
        public string melee { get; }
        public string damage { get; }
        public string Engineer { get; }
        public string Scholar { get; }
        public string Engineering { get; }
        public string Pursuit { get; }
        public string EnhancedItems { get; }
        public string Monsters { get; }
        public string Classes { get; }
        public string Species { get; }
        public string Archetypes { get; }
        public string Backgrounds { get; }
        public string ForcePowers { get; }
        public string TechPowers { get; }
        public string StarshipModifications { get; }
        public string StarshipEquipment { get; }
        public string StarshipWeapons { get; }
        public string Ventures { get; }
        public string AdditionalVariantRules { get; }
        public string MonsterManual { get; }
        public string WretchedHives { get; }
        public string WretchedHivesChangelog { get; }
        public string StarshipsOfTheGalaxy { get; }
        public string StarshipsOfTheGalaxyChangelog { get; }
        public string PlayersHandbook { get; }
        public string PlayersHandbookChangelog { get; }
        public string WeaponProperties { get; }
        public string ArmorProperties { get; }
        public string Fighter { get; }
        public string ChooseAny { get; }
        #endregion

        #region WretchedHivesManager
        public List<(string name, string startLine, int occurence)> WretchedHivesWeaponProperties { get; }
        public List<(string name, string startLine, int occurence)> WretchedHivesArmorProperties { get; }
        #endregion

        #region PlayerHandbookManager
        public List<(string name, string startLine, int occurence)> PlayerHandbookWeaponProperties { get; }
        public List<(string name, string startLine, int occurence)> PlayerHandbookArmorProperties { get; }
        #endregion

        #region PlayerHandbookBackgroundsProcessor
        public string StartingBackground { get; }
        public string Changelog { get; }
        #endregion

        #region PlayerHandbookChapterRulesProcessor
        public string PHBPrefaceStartLine { get; }
        public string PHBWhatsDifferentStartLine { get; }
        public string PHBIntroductionStartLine { get; }
        public string PHBChapter1StartLine { get; }
        public string PHBChapter2StartLine { get; }
        public string PHBChapter3StartLine { get; }
        public string PHBChapter4StartLine { get; }
        public string PHBChapter5StartLine { get; }
        public string PHBChapter6StartLine { get; }
        public string PHBChapter7StartLine { get; }
        public string PHBChapter8StartLine { get; }
        public string PHBChapter9StartLine { get; }
        public string PHBChapter10StartLine { get; }
        public string PHBChapter11StartLine { get; }
        public string PHBAppendixAStartLine { get; }
        public string PHBAppendixBStartLine { get; }
        public string PHBChangelogStartLine { get; }
        public string PHBPrefaceTitle { get; }
        public string PHBWhatsDifferentTitle { get; }
        public string PHBIntroductionTitle { get; }
        public string PHBChapter1Title { get; }
        public string PHBChapter2Title { get; }
        public string PHBChapter3Title { get; }
        public string PHBChapter4Title { get; }
        public string PHBChapter5Title { get; }
        public string PHBChapter6Title { get; }
        public string PHBChapter7Title { get; }
        public string PHBChapter8Title { get; }
        public string PHBChapter9Title { get; }
        public string PHBChapter10Title { get; }
        public string PHBAppendixATitle { get; }
        public string PHBAppendixBTitle { get; }
        public string PHBChangelogTitle { get; }
        public string PHBClassesStartLine { get; }
        public string PHBBackgroundsStartLine { get; }
        public string PHBFeatsStartLine { get; }

        #endregion

        #region PlayerHandbookClassProcessor
        public string PHBClassesTableStart { get; }
        public string PHBClassHitPointsAtFirstLevel { get; }
        public string PHBClassHitPointsAtHigherLevel { get; }
        public string PHBClassToolProficiencyChoice { get; }
        public string PHBClassBuildStartPattern { get; }
        #endregion

        #region PlayerHandbookEquipmentProcessor
        public string PHBSimpleBlastersTableStart { get; }
        public string PHBMartialBlastersTableStart { get; }

        public string PHBVibroweaponsTableStart { get; }
        public string PHBLightweaponsTableStart { get; }
        public string PHBArmorAndShieldsTableStart { get; }
        public string PHBArtisansImplementsTableStart { get; }
        public string PHBAmmunitionTableStart { get; }
        public string PHBMedicalTableStart { get; }
        #endregion

        #region PlayerHandbookFeatProcessor
        public string PHBFeatYourChoice { get; }
        public string PHBAttributeIncreaseIndexPattern(List<string> attributesChoices);
        #endregion

        #region PlayerHandbookPowersProcessor
        public string PHBPowerStartLinesPattern { get; }
        #endregion

        #region ExpandedContentSpeciesProcessor
        public List<string> ECValidAttributeHints { get; }
        public string ECSpeciesColorScheme { get; }
        public string ECSpeciesSkinColor { get; }
        public string ECSpeciesHairColor{ get; }
        public string ECSpeciesEyeColor{ get; }
        public string ECSpeciesDistinctions { get; }
        public string ECSpeciesHeight { get; }
        public string ECSpeciesWeight { get; }
        public string ECSpeciesHomeworld { get; }
        public string ECSpeciesManufacturer { get; }
        public string ECSpeciesLanguage { get; }
        public string ECSpeciesPrimaryLanguage { get; }
        public string ECSpeciesAbilityScoreIncrease { get; }
        public string ECSpeciesHalfHuman { get; }
        #endregion

        #region StarshipChapterRulesProcessor
        public string SOTGChapter0StartLine { get; }
        public string SOTGChapter1StartLine { get; }
        public string SOTGChapter2StartLine { get; }
        public string SOTGChapter3StartLine { get; }
        public string SOTGChapter4StartLine { get; }
        public string SOTGChapter5StartLine { get; }
        public string SOTGChapter6StartLine { get; }
        public string SOTGChapter7StartLine { get; }
        public string SOTGChapter8StartLine { get; }
        public string SOTGChapter9StartLine { get; }
        public string SOTGChapter10StartLine { get; }
        public string SOTGAppendixAStartLine { get; }
        public string SOTGChangelogStartLine { get; }
        public string SOTGChapter0Title { get; }
        public string SOTGChapter1Title { get; }
        public string SOTGChapter2Title { get; }
        public string SOTGChapter3Title { get; }
        public string SOTGChapter4Title { get; }
        public string SOTGChapter5Title { get; }
        public string SOTGChapter6Title { get; }
        public string SOTGChapter7Title { get; }
        public string SOTGChapter8Title { get; }
        public string SOTGChapter9Title { get; }
        public string SOTGChapter10Title { get; }
        public string SOTGAppendixATitle { get; }
        public string SOTGChangelogTitle { get; }
        public string SOTGDeploymentsStartLine { get; }
        public string SOTGShipSizeStartLine { get; }
        public string SOTGVariantStart { get; }
        public string SOTGModificationsStart { get; }
        public string SOTGVenturesStart { get; }
        public string SOTGVariantSpaceStations { get; }
        #endregion

        #region StarshipDeploymentProcessor
        public string DeploymentsStart { get; }
        public string GetDeploymentTableStart(string deploymentName);
        #endregion

        #region StarshipEquipmentProcessor
        public string SOTGArmorTableStartingLineArmorClass { get; }
        public string SOTGArmorTableStartingLineShieldRegeneration { get; }
        public string SOTGSmallWeaponsTableStartingLine { get; }
        public string SOTGHugeWeaponsTableStartingLine { get; }
        public string SOTGTertiaryAmmunitionTableStartingLine { get; }
        public string SOTGQuaternaryAmmunitionTableStartingLine { get; }

        public string SOTGHyperdrivesTableStartingLine { get; }
        public string SOTGNavcomputerTableStartingLine { get; }
        public string SOTGArmorTableArmorStartingLine { get; }
        public string SOTGArmorTableShieldsStartingLine { get; }
        public string SOTGReactorsTableStartingLineFuelCosts { get; }
        public string SOTGReactorsTableReactorsStartingLine { get; }
        public string SOTGReactorsTablePowerCouplingsStartingLine { get; }
        #endregion

        #region StarshipModificationProcessor
        public string SOTGEngineeringLinesStart { get; }
        public string SOTGOperationsLinesStart { get; }
        public string SOTGSuitesLinesStart { get; }
        public string SOTGUniversalLinesStart { get; }
        public string SOTGWeaponsLinesStart { get; }
        #endregion

        #region StarshipSizeProcessor
        public List<string> ShipSavingThrowOptions { get; }
        public string SOTGStarshipFeatures { get; }
        public string SOTGStrengthAtTierZero { get; }
        public string SOTGDexterityAtTierZero { get; }
        public string SOTGConstitutionAtTierZero { get; }
        public string SOTGHitDiceAtTierZero { get; }
        public string SOTGHitPointsForSubsequentHitDie{ get; }
        public string SOTGMaximumSuiteSystems { get; }
        public string SOTGModificationSlotsAtTierZero { get; }
        public string SOTGStockModifications { get; }
        public string SOTGSavingThrows { get; }
        public string SOTGStartingEquipment{ get; }
        public string SOTGClassTableStart { get; }
        public string SOTGClassTableEnd { get; }
        public string SOTGStarshipImprovements { get; }
        public string SOTGAdditionalModifications { get; }
        public string SOTGYourChoiceOf { get; }
        public string SOTGSuiteChoiceSplitIndex { get; }
        #endregion

        #region EnhancedItemProcessor
        public string SampleEnhancedItems { get; }
        public string RequiresAttunement { get; }
        #endregion

        #region WretchedHivesChapterRulesProcessor
        public string WHChapter0StartLine { get; }
        public string WHChapter1StartLine { get; }
        public string WHChapter2StartLine { get; }
        public string WHChapter3StartLine { get; }
        public string WHChapter4StartLine { get; }
        public string WHChapter5StartLine { get; }
        public string WHChapter6StartLine { get; }
        public string WHChapter7StartLine { get; }
        public string WHChapter8StartLine { get; }
        public string WHAppendixAStartLine { get; }
        public string WHChangelogStartLine { get; }
        public string WHChapter0Title { get; }
        public string WHChapter1Title { get; }
        public string WHChapter2Title { get; }
        public string WHChapter3Title { get; }
        public string WHChapter4Title { get; }
        public string WHChapter5Title { get; }
        public string WHChapter6Title { get; }
        public string WHChapter7Title { get; }
        public string WHChapter8Title { get; }
        public string WHChangelogTitle { get; }
        #endregion

        #region WretchedHivesEquipmentProcessor
        public string WHBlastersStartLine { get; }
        public string WHVibroweaponsStartLine { get; }
        public string WHLightweaponsStartLine { get; }
        public string WHArmorAndShieldsStartLine { get; }
        public string WHSpecialistsKitStartLine { get; }
        public string WHAmmunitionStartLine { get; }
        public string WHMedicalStartLine { get; }
        public string WHAlcoholicBeveragesStartLine { get; }
        public string WHDataRecordingAndStorageStartLine { get; }
        public string WHExplosivesStartLine { get; }
        public string WHArtisansImplementsStartLine { get; }

        #endregion

        #region ExpandedContentArchetypeProcessor
        public string ECTableOfContentsStartLine { get; }
        public string ECFormIXTrakata { get; }
        public string ECFormIXTrakataMangled { get; }
        #endregion

        #region ExpandedContentBackgroundProcessor
        public string ECToolProficienciesStartLine { get; }
        public string ECLanguagesStartLine { get; }
        public string ECEquipmentStartLine { get; }
        public string ECSkillProficienciesPattern { get; }
        public string ECSuggestedCharacteristicsStartLine { get; }
        public string ECFeatureStartLine { get; }
        public string ECFeatStartLine { get; }
        public string ECPersonalityTraitsPattern { get; }
        public string ECIdealsPattern { get; }
        public string ECBondsPattern { get; }
        public string ECFlawsPattern { get; }
        #endregion

        #region ExpandedContentEquipmentProcessor
        public string ECBlastersStartLine { get; }
        public string ECMartialLightweaponsStartLine { get; }
        public string ECMartialVibroweaponsStartLine { get; }
        public string ECAmmunitionStartLine { get; }
        public string ECInterchangeableWeaponsSystemPattern { get; }
        public string ECStrengthRequirementPattern { get; }
        public string ECClassificationMartialBlasters { get; }
        public string ECClassificationSimpleBlasters { get; }
        public string ECClassificationSimpleLightweapons { get; }
        public string ECClassificationMartialLightweapons { get; }
        public string ECClassificationSimpleVibroweapons { get; }
        public string ECClassificationMartialVibroweapons { get; }
        public string ECClassificationUtilities { get; }
        public string ECClassificationWeaponAndArmorAccessories { get; }
        public string ECClassificationAmmunition { get; }
        public string ECClassificationClothing { get; }
        public string ECClassificationCommunication { get; }
        public string ECClassificationDataRecordingAndStorage { get; }
        public string ECClassificationExplosives { get; }
        public string ECClassificationLifeSupport { get; }
        public string ECClassificationMedical { get; }
        public string ECClassificationStorage { get; }
        public string ECClassificationArtisansImplements { get; }
        public string ECClassificationGamingSet { get; }
        public string ECClassificationMusicalInstrument { get; }
        public string ECClassificationSpecialistsKit { get; }
        public string ECClassificationLightArmor { get; }
        public string ECClassificationMediumArmor{ get; }
        public string ECClassificationHeavyArmor { get; }
        public string ECClassificationShield { get; }
        public string ECClassificationAlcoholicBeverages { get; }
        public string ECClassificationSpices { get; }
        #endregion

        #region ExpandedContentForcePowersProcessor
        public string ECAtWillPattern { get; }
        #endregion

        #region ExpandedContentVariantRulesProcessor
        public string ECVariantRuleForceAlignment { get; }
        public string ECVariantRuleForceAlignmentStartingLetter { get; }
        public string ECVariantRuleDestiny { get; }
        public string ECVariantRuleDestinyStartingLetter { get; }
        public string ECVariantRuleStarshipDestiny { get; }
        public string ECVariantRuleStarshipDestinyStartingLetter { get; }
        public string ECVariantRuleWeaponSundering { get; }
        public string ECVariantRuleWeaponSunderingStartingLetter { get; }
        public string ECVariantRuleAdvancedAdvantage { get; }
        public string ECVariantRuleAdvancedAdvantageStartingLetter { get; }
        public string ECVariantRuleLightsaberForms { get; }
        public string ECVariantRuleLightsaberFormsStartingLetter { get; }
        public string ECVariantRuleDismemberment { get; }
        public string ECVariantRuleDismembermentStartingLetter { get; }
        public string ECVariantRuleCompoundAdvantage { get; }
        public string ECVariantRuleCompoundAdvantageStartingLetter { get; }

        #endregion

        #region MonsterManualProcessor
        public string MonsterArmorClass { get; }
        public string MonsterHitPoints{ get; }
        public string MonsterSpeed { get; }
        public string MonsterAttributes { get; }
        public string MonsterSavingThrows { get; }
        public string MonsterSkills { get; }
        public string MonsterDamageVulnerabilities { get; }
        public string MonsterDamageVulnerabilitiesPattern { get; }
        public string MonsterDamageImmunities { get; }
        public string MonsterDamageResistances{ get; }
        public string MonsterConditionImmunities{ get; }
        public string MonsterSenses { get; }
        public string MonsterLanguages { get; }
        public string MonsterChallenge { get; }
        public string MonsterBehaviorLegendary { get; }
        public string MonsterBehaviorActions { get; }
        public string MonsterBehaviorReactions { get; }
        public string MonsterHitSplitPattern { get; }
        #endregion

        #region ReferenceTableProcessor
        public string ReferenceTableNameAbilityScorePointCost { get; }
        public string ReferenceTableNameAbilityScoresAndModifiers { get; }
        public string ReferenceTableNameXPByLevel { get; }
        public string ReferenceTableNameStartingWealthByClass { get; }
        public string ReferenceTableNameLifestyleExpenses { get; }
        public string ReferenceTableNameMulticlassingPrerequisites { get; }
        public string ReferenceTableNameMulticlassingProficiencies { get; }
        public string ReferenceTableNameStarshipSizeStockCost { get; }
        public string ReferenceTableNameStarshipSizeBuildingWorkforce { get; }
        public string ReferenceTableNameBaseUpgradeCostByTier{ get; }
        public string ReferenceTableNameStarshipSizeUpgradeCost { get; }
        public string ReferenceTableNameStarshipSizeUpgradeWorkforce { get; }
        public string ReferenceTableNameModificationCategoryBaseCost { get; }
        public string ReferenceTableNameStarshipSizeModificationCost { get; }
        public string ReferenceTableNameStarshipSizeModificationWorkforce { get; }
        public string ReferenceTableNameModificationTierRequirementDC { get; }
        public string ReferenceTableNameModificationSlotsAtTierZero { get; }
        public string ReferenceTableNameStarshipSizeEquipmentCost { get; }
        public string ReferenceTableNameStarshipSizeEquipmentWorkforce { get; }
        public string ReferenceTableNameStarshipSizeCargoCapacity { get; }
        public string ReferenceTableNameStarshipSizeBaseArmorClass { get; }
        public string ReferenceTableNameStarshipSizeFuelCost { get; }
        public string ReferenceTableNameStarshipSizeFuelCapacity { get; }
        public string ReferenceTableNameStarshipSizeFoodCapacity { get; }
        public string ReferenceTableNameStarshipSizeBaseFlyingSpeed { get; }
        public string ReferenceTableNameStarshipSizeBaseTurningSpeed { get; }
        public string ReferenceTableNameSampleRealspaceTravelTimes{ get; }
        public string ReferenceTableNameSampleHyperspaceTravelTimes { get; }
        public string ReferenceTableNameAstrogationTimeTaken { get; }
        public string ReferenceTableNameHyperspaceMishaps { get; }
        public string ReferenceTableNameStarshipSizeMinimumCrew { get; }
        public string ReferenceTableNameStarshipSizeRepairTime { get; }
        public string ReferenceTableNameStarshipSizeMaintenanceTime { get; }
        public string ReferenceTableNameStarshipSizeCategories { get; }
        public string ReferenceTableNameSystemDamage { get; }
        public string ReferenceTableNameStarshipSizeMaximumSuites { get; }
        public string ReferenceTableNameStarshipSizeSuiteCapacity { get; }
        public string ReferenceTableNameCyberneticAugmentationSideEffects { get; }
        public string ReferenceTableNameDeploymentRankPrestige { get; }
        public string ReferenceTableNameModificationCapacityByShipSize { get; }
        public string ReferenceTableNameModificationGradeInstallationByShipTier { get; }
        public string ReferenceTableNameBaseHyperspaceTravelTimes { get; }
        public string ReferenceTableNameStarshipSizeRefittingTime { get; }
        public string ReferenceTableNameStarshipSlowedLevel { get; }


        public string ReferenceTableStartingLineAbilityScorePointCost { get; }
        public string ReferenceTableStartingLineAbilityScoresAndModifiers { get; }
        public string ReferenceTableStartingLineXPByLevel { get; }
        public string ReferenceTableStartingLineStartingWealthByClass { get; }
        public string ReferenceTableStartingLineLifestyleExpenses { get; }
        public string ReferenceTableStartingLineMulticlassingPrerequisites { get; }
        public string ReferenceTableStartingLineMulticlassingProficiencies { get; }
        public string ReferenceTableStartingLineStarshipSizeStockCost { get; }
        public string ReferenceTableStartingLineStarshipSizeBuildingWorkforce { get; }
        public string ReferenceTableStartingLineBaseUpgradeCostByTier { get; }
        public string ReferenceTableStartingLineStarshipSizeUpgradeCost { get; }
        public string ReferenceTableStartingLineStarshipSizeUpgradeWorkforce { get; }
        public string ReferenceTableStartingLineModificationCategoryBaseCost { get; }
        public string ReferenceTableStartingLineStarshipSizeModificationCost { get; }
        public string ReferenceTableStartingLineStarshipSizeModificationWorkforce { get; }
        public string ReferenceTableStartingLineModificationTierRequirementDC { get; }
        public string ReferenceTableStartingLineModificationSlotsAtTierZero { get; }
        public string ReferenceTableStartingLineStarshipSizeEquipmentCost { get; }
        public string ReferenceTableStartingLineStarshipSizeEquipmentWorkforce { get; }
        public string ReferenceTableStartingLineStarshipSizeCargoCapacity { get; }
        public string ReferenceTableStartingLineStarshipSizeBaseArmorClass { get; }
        public string ReferenceTableStartingLineStarshipSizeFuelCost { get; }
        public string ReferenceTableStartingLineStarshipSizeFuelCapacity { get; }
        public string ReferenceTableStartingLineStarshipSizeFoodCapacity { get; }
        public string ReferenceTableStartingLineStarshipSizeBaseFlyingSpeed { get; }
        public string ReferenceTableStartingLineStarshipSizeBaseTurningSpeed { get; }
        public string ReferenceTableStartingLineSampleRealspaceTravelTimes { get; }
        public string ReferenceTableStartingLineSampleHyperspaceTravelTimes { get; }
        public string ReferenceTableStartingLineAstrogationTimeTaken { get; }
        public string ReferenceTableStartingLineHyperspaceMishaps { get; }
        public string ReferenceTableStartingLineStarshipSizeMinimumCrew { get; }
        public string ReferenceTableStartingLineStarshipSizeRepairTime { get; }
        public string ReferenceTableStartingLineStarshipSizeMaintenanceTime { get; }
        public string ReferenceTableStartingLineStarshipSizeCategories { get; }
        public string ReferenceTableStartingLineSystemDamage { get; }
        public string ReferenceTableStartingLineStarshipSizeMaximumSuites { get; }
        public string ReferenceTableStartingLineStarshipSizeSuiteCapacity { get; }
        public string ReferenceTableStartingLineCyberneticAugmentationSideEffects { get; }
        public string ReferenceTableStartingLineDeploymentRankPrestige { get; }
        public string ReferenceTableStartingLineModificationCapacityByShipSize { get; }
        public string ReferenceTableStartingLineModificationGradeInstallationByShipTier { get; }
        public string ReferenceTableStartingLineBaseHyperspaceTravelTimes { get; }
        public string ReferenceTableStartingLineStarshipSizeRefittingTime { get; }
        public string ReferenceTableStartingLineStarshipSlowedLevel { get; }



        string AtWill { get; }
        string FirstLevel { get; }
        string SecondLevel { get; }
        string FourthLevel { get; }
        string FifthLevel { get; }
        string ThirdLevel { get; }
        string SixthLevel { get; }
        string SeventhLevel { get; }
        string EighthLevel { get; }
        string NinthLevel { get; }
        Tuple<string, int> FirstLevelNum { get; }
        Tuple<string, int> SecondLevelNum { get; }
        Tuple<string, int> ThirdLevelNum { get; }
        Tuple<string, int> FourthLevelNum { get; }
        Tuple<string, int> FifthLevelNum { get; }
        Tuple<string, int> SixthLevelNum { get; }
        Tuple<string, int> SeventhLevelNum { get; }
        Tuple<string, int> EighthLevelNum { get; }
        Tuple<string, int> NinthLevelNum { get; }
        Tuple<string, int> TenthLevelNum { get; }
        Tuple<string, int> EleventhLevelNum { get; }
        Tuple<string, int> TwelfthLevelNum { get; }
        Tuple<string, int> ThirteenthLevelNum { get; }
        Tuple<string, int> FourteenthLevelNum { get; }
        Tuple<string, int> FifteenthLevelNum { get; }
        Tuple<string, int> SixteenthLevelNum { get; }
        Tuple<string, int> SeventeenthLevelNum { get; }
        Tuple<string, int> EighteenthLevelNum { get; }
        Tuple<string, int> NineteenthLevelNum { get; }
        Tuple<string, int> TwentiethLevelNum { get; }
        Tuple<string, int> FirstNum { get; }
        Tuple<string, int> SecondNum { get; }
        Tuple<string, int> ThirdNum { get; }
        Tuple<string, int> FourthNum { get; }
        Tuple<string, int> FifthNum { get; }
        Tuple<string, int> SixthNum { get; }
        Tuple<string, int> SeventhNum { get; }
        Tuple<string, int> EighthNum { get; }
        Tuple<string, int> NinthNum { get; }
        Tuple<string, int> TenthNum { get; }
        Tuple<string, int> EleventhNum { get; }
        Tuple<string, int> TwelfthNum { get; }
        Tuple<string, int> ThirteenthNum { get; }
        Tuple<string, int> FourteenthNum { get; }
        Tuple<string, int> FifteenthNum { get; }
        Tuple<string, int> SixteenthNum { get; }
        Tuple<string, int> SeventeenthNum { get; }
        Tuple<string, int> EighteenthNum { get; }
        Tuple<string, int> NineteenthNum { get; }
        Tuple<string, int> TwentiethNum { get; }
        string ECStorageStartLine { get; }
        #endregion
    }
}
