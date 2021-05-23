using System;
using System.Collections.Generic;
using StarWars5e.Models.Enums;
using Attribute = StarWars5e.Models.Enums.Attribute;

namespace StarWars5e.Parser.Localization
{
    public class LocalizationGu : ILocalization
    {
        public Language Language => Language.gu;

        #region Common
        public string Quick => "Quick";
        public string Build => "Build";
        public string Creating => "Creating";
        public string The => "The";
        public string or => "or";
        public string from => "from";
        public string and => "and";
        public string Armor => "Armor";
        public string Tools => "Tools";
        public string Weapons => "Weapons";
        public string Skills => "Skills";
        public string one => "one";
        public string two => "two";
        public string three => "three";
        public string four => "four";
        public string five => "five";
        public string six => "six";
        public string seven => "seven";
        public string eight => "eight";
        public string nine => "nine";
        public string Equipment => "Equipment";
        public string Level => "Level";
        public string Prerequisite => "Prerequisite";
        public string Feats => "Feats";
        public string Any => "Any";
        public string ability => "ability";
        public string Stun => "Stun";
        public string Range => "Range";
        public string Duration => "Duration";
        public string Concentration => "Concentration";
        public string CastingTime => "Casting Time";
        public string bonus => "bonus";
        public string reaction => "reaction";
        public string action => "action";
        public string minute => "minute";
        public string hour => "hour";
        public string universal => "universal";
        public string dark => "dark";
        public string light => "light";
        public string tech => "tech";
        public string Traits => "Traits";
        public string Size => "Size";
        public string choice => "choice"; 
        public string reload => "reload";
        public string range => "range";
        public string PrimaryWeapons => "Primary Weapons";
        public string SecondaryWeapons => "Secondary Weapons";
        public string TertiaryWeapons => "Tertiary Weapons";
        public string QuaternaryWeapons => "Quaternary Weapons";
        public string feature => "feature";
        public string a => "a";
        public string Tier => "Tier";
        public string Features => "Features";
        public string standard => "standard";
        public string premium => "premium";
        public string prototype => "prototype";
        public string advanced => "advanced";
        public string legendary => "legendary";
        public string artifact => "artifact";
        public string adventuringgear => "adventuring gear";
        public string AdventuringGear => "Adventuring Gear";
        public string body => "body";
        public string Feet => "Feet";
        public string Finger => "Finger";
        public string Hands => "Hands";
        public string Head =>"Head";
        public string Neck => "Neck";
        public string Legs => "Legs";
        public string Shoulders => "Shoulders";
        public string Waist => "Waist";
        public string Wrists => "Wrists";
        public string armormodification => "armor modification";
        public string reinforcement => "reinforcement";
        public string shielding => "shielding";
        public string overlay => "overlay";
        public string underlay => "underlay";
        public string armoring => "armoring";
        public string armor => "armor";
        public string anyheavy => "any heavy";
        public string anymedium => "any medium";
        public string anylight => "any light";
        public string any => "any";
        public string consumable => "consumable";
        public string adrenal => "adrenal";
        public string explosive => "explosive";
        public string poison => "poison";
        public string stimpac => "stimpac";
        public string barrier => "barrier";
        public string cyberneticaugmentation => "cybernetic augmentation";
        public string enhancement => "enhancement";
        public string replacement => "replacement";
        public string droidcustomization => "droid customization";
        public string part => "part";
        public string protocol => "protocol";
        public string focusgeneratormodification => "focus generator modification";
        public string cycler => "cycler";
        public string emitter => "emitter";
        public string conductor => "conductor";
        public string energychannel => "energy channel";
        public string focus => "focus";
        public string force => "force";
        public string itemmodification => "item modification";
        public string augment => "augment";
        public string wristpadmodification => "wristpad modification";
        public string dataport => "dataport";
        public string storage => "storage";
        public string motherboard => "motherboard";
        public string processor => "processor";
        public string lightweaponmodification => "lightweapon modification";
        public string lens => "lens";
        public string stabilizer => "stabilizer";
        public string crystal => "crystal";
        public string powercell => "power cell";
        public string vibroweaponmodification => "vibroweapon modification";
        public string edge => "edge";
        public string vibratorcell => "vibrator cell";
        public string projector => "projector";
        public string grip => "grip";
        public string clothingmodification => "clothing modification";
        public string inlay => "inlay";
        public string weave => "weave";
        public string blastermodification => "blaster modification";
        public string barrel => "barrel";
        public string targeting => "targeting";
        public string matrix => "matrix";
        public string energycore => "energy core";
        public string shield => "shield";
        public string heavy => "heavy";
        public string medium => "medium";
        public string weapon => "weapon";
        public string anyblaster => "any blaster";
        public string anyvibroweapon => "any vibroweapon";
        public string anylightweapon => "any lightweapon";
        public string lightweapon => "lightweapon";
        public string blaster => "blaster";
        public string vibroweapon => "vibroweapon";
        public string valuable => "valuable";
        public string art => "art";
        public string jewel => "jewel";
        public string relic => "relic";
        public string sculpture => "sculpture";
        public string shiparmor => "ship armor";
        public string shipshield => "ship shield";
        public string shipweapon => "ship weapon";
        public string property => "property";
        public string Berserker => "Berserker";
        public string SuggestedCharacteristics => "Suggested Characteristics";
        public string IWS => "IWS";
        public string Bulky => "Bulky";
        public string ranged => "ranged";
        public string melee => "melee";
        public string damage => "damage";
        public string Engineer => "Engineer";
        public string Scholar => "Scholar";
        public string Engineering => "Engineering";
        public string Pursuit => "Pursuit";
        public string EnhancedItems => "Enhanced Items";
        public string Monsters => "Monsters";
        public string Classes => "Classes";
        public string Species => "Species";
        public string Archetypes => "Archetypes";
        public string Backgrounds => "Backgrounds";
        public string ForcePowers => "Force Powers";
        public string TechPowers => "Tech Powers";
        public string StarshipModifications => "Starship Modifications";
        public string StarshipEquipment => "Starship Equipment";
        public string StarshipWeapons => "Starship Weapons";
        public string Ventures => "Ventures";
        public string AdditionalVariantRules => "Additional Variant Rules";
        public string MonsterManual => "Monster Manual";
        public string WretchedHives => "Wretched Hives";
        public string WretchedHivesChangelog => "Wretched Hives Changelog";
        public string StarshipsOfTheGalaxy => "Starships Of The Galaxy";
        public string StarshipsOfTheGalaxyChangelog => "Starships Of The Galaxy Changelog";
        public string PlayersHandbook => "Player's Handbook";
        public string PlayersHandbookChangelog => "Player's Handbook Changelog";
        public string WeaponProperties => "Weapon Properties";
        public string ArmorProperties => "Armor Properties";
        public string Fighter => "Fighter";
        public string ChooseAny => "Choose any";
        public Tuple<string, int> FirstLevelNum => Tuple.Create("1st level", 1);
        public Tuple<string, int>  SecondLevelNum => Tuple.Create("2nd level", 2);
        public Tuple<string, int>  ThirdLevelNum => Tuple.Create("3rd level", 3);
        public Tuple<string, int>  FourthLevelNum => Tuple.Create("4th level", 4);
        public Tuple<string, int>  FifthLevelNum => Tuple.Create("5th level", 5);
        public Tuple<string, int>  SixthLevelNum => Tuple.Create("6th level", 6);
        public Tuple<string, int>  SeventhLevelNum => Tuple.Create("7th level", 7);
        public Tuple<string, int>  EighthLevelNum => Tuple.Create("8th level", 8);
        public Tuple<string, int>  NinthLevelNum => Tuple.Create("9th level", 9);
        public Tuple<string, int>  TenthLevelNum => Tuple.Create("10th level", 10);
        public Tuple<string, int>  EleventhLevelNum => Tuple.Create("11th level", 11);
        public Tuple<string, int>  TwelfthLevelNum => Tuple.Create("12th level", 12);
        public Tuple<string, int>  ThirteenthLevelNum => Tuple.Create("13th level", 13);
        public Tuple<string, int>  FourteenthLevelNum => Tuple.Create("14th level", 14);
        public Tuple<string, int>  FifteenthLevelNum => Tuple.Create("15th level", 15);
        public Tuple<string, int>  SixteenthLevelNum => Tuple.Create("16th level", 16);
        public Tuple<string, int>  SeventeenthLevelNum => Tuple.Create("17th level", 17);
        public Tuple<string, int>  EighteenthLevelNum => Tuple.Create("18th level", 18);
        public Tuple<string, int>  NineteenthLevelNum => Tuple.Create("19th level", 19);
        public Tuple<string, int> TwentiethLevelNum => Tuple.Create("20th level", 20);
        public Tuple<string, int> FirstNum => Tuple.Create("1st", 1);
        public Tuple<string, int> SecondNum => Tuple.Create("2nd", 2);
        public Tuple<string, int> ThirdNum => Tuple.Create("3rd", 3);
        public Tuple<string, int> FourthNum => Tuple.Create("4th", 4);
        public Tuple<string, int> FifthNum => Tuple.Create("5th", 5);
        public Tuple<string, int> SixthNum => Tuple.Create("6th", 6);
        public Tuple<string, int> SeventhNum => Tuple.Create("7th", 7);
        public Tuple<string, int> EighthNum => Tuple.Create("8th", 8);
        public Tuple<string, int> NinthNum => Tuple.Create("9th", 9);
        public Tuple<string, int> TenthNum => Tuple.Create("10th", 10);
        public Tuple<string, int> EleventhNum => Tuple.Create("11th", 11);
        public Tuple<string, int> TwelfthNum => Tuple.Create("12th", 12);
        public Tuple<string, int> ThirteenthNum => Tuple.Create("13th", 13);
        public Tuple<string, int> FourteenthNum => Tuple.Create("14th", 14);
        public Tuple<string, int> FifteenthNum => Tuple.Create("15th", 15);
        public Tuple<string, int> SixteenthNum => Tuple.Create("16th", 16);
        public Tuple<string, int> SeventeenthNum => Tuple.Create("17th", 17);
        public Tuple<string, int> EighteenthNum => Tuple.Create("18th", 18);
        public Tuple<string, int> NineteenthNum => Tuple.Create("19th", 19);
        public Tuple<string, int> TwentiethNum => Tuple.Create("20th", 20);

        #endregion

        #region WretchedHivesManager
        public List<(string name, string startLine, int occurence)> WretchedHivesWeaponProperties => new List<(string name, string startLine, int occurence)>
        {
            ("Autotarget", "#### Autotarget", 1),
            ("Brutal", "#### Brutal", 1),
            ("Defensive", "#### Defensive", 1),
            ("Dire", "#### Dire", 1),
            ("Disarming", "#### Disarming", 1),
            ("Disintegrate", "#### Disintegrate", 1),
            ("Disruptive", "#### Disruptive", 1),
            ("Keen", "#### Keen", 1),
            ("Mighty", "#### Mighty", 1),
            ("Neuralizing", "#### Neuralizing", 1),
            ("Piercing", "#### Piercing", 1),
            ("Shocking", "#### Shocking", 1),
            ("Silent", "#### Silent", 2),
            ("Sonorous", "#### Sonorous", 1),
            ("Switch", "#### Switch", 1),
            ("Vicious", "#### Vicious", 1)
        };
        public List<(string name, string startLine, int occurence)> WretchedHivesArmorProperties => new List<(string name, string startLine, int occurence)>
        {
            ("Absorptive", "#### Absorptive", 1),
            ("Agile", "#### Agile", 1),
            ("Anchor", "#### Anchor", 1),
            ("Avoidant", "#### Avoidant", 1),
            ("Barbed", "#### Barbed", 1),
            ("Charging", "#### Charging", 1),
            ("Concealing", "#### Concealing", 1),
            ("Cumbersome", "#### Cumbersome", 1),
            ("Gauntleted", "#### Gauntleted", 1),
            ("Imbalanced", "#### Imbalanced", 1),
            ("Impermeable", "#### Impermeable", 1),
            ("Insulated", "#### Insulated", 1),
            ("Interlocking", "#### Interlocking", 1),
            ("Lambent", "#### Lambent", 1),
            ("Lightweight", "#### Lightweight", 1),
            ("Magnetic", "#### Magnetic", 1),
            ("Obscured", "#### Obscured", 1),
            ("Powered", "#### Powered", 1),
            ("Reactive", "#### Reactive", 1),
            ("Regulated", "#### Regulated", 1),
            ("Reinforced", "#### Reinforced", 1),
            ("Responsive", "#### Responsive", 1),
            ("Rigid", "#### Rigid", 1),
            ("Silent", "#### Silent", 1),
            ("Spiked", "#### Spiked", 1),
            ("Steadfast", "#### Steadfast", 1),
            ("Versatile", "#### Versatile", 1)
        };
        #endregion

        #region PlayerHandbookManager
        public List<(string name, string startLine, int occurence)> PlayerHandbookWeaponProperties => new List<(string name, string startLine, int occurence)>
        {
            ("Ammunition", "#### Ammunition", 1),
            ("Auto", "#### Auto", 1),
            ("Burst", "#### Burst", 1),
            ("Dexterity", "#### Dexterity", 1),
            ("Disguised", "#### Disguised", 1),
            ("Double", "#### Double", 1),
            ("Finesse", "#### Finesse", 1),
            ("Fixed", "#### Fixed", 1),
            ("Heavy", "#### Heavy", 3 ),
            ("Hidden", "#### Hidden", 1),
            ("Light", "#### Light", 2),
            ("Luminous", "#### Luminous", 1),
            ("Range", "#### Range", 1),
            ("Rapid", "#### Rapid", 1),
            ("Reach", "#### Reach", 1),
            ("Reload", "#### Reload", 1),
            ("Returning", "#### Returning", 1),
            ("Special", "#### Special", 1),
            ("Strength", "#### Strength", 2),
            ("Thrown", "#### Thrown", 1),
            ("Two-Handed", "#### Two-Handed", 1),
            ("Versatile", "#### Versatile", 1)
        };
        public List<(string name, string startLine, int occurence)> PlayerHandbookArmorProperties => new List<(string name, string startLine, int occurence)>
        {
            ("Bulky", "#### Bulky", 1),
            ("Obtrusive", "#### Obtrusive", 1),
            ("Strength", "#### Strength", 1)
        };
        #endregion

        #region PlayerHandbookBackgroundsProcessor
        public string StartingBackground => "## Agent";
        public string Changelog => "Changelog";
        #endregion

        #region PlayerHandbookChangerRulesProcessor
        public string PHBPrefaceStartLine => "## Preface";
        public string PHBWhatsDifferentStartLine => "## What's Different?";
        public string PHBIntroductionStartLine => "# Introduction";
        public string PHBChapter1StartLine => "# Chapter 1: Step-By-Step Characters";
        public string PHBChapter2StartLine => "# Chapter 2: Species";
        public string PHBChapter3StartLine => "# Chapter 3: Classes";
        public string PHBChapter4StartLine => "# Chapter 4: Personality and Backgrounds";
        public string PHBChapter5StartLine => "# Chapter 5: Equipment";
        public string PHBChapter6StartLine => "# Chapter 6: Customization Options";
        public string PHBChapter7StartLine => "# Chapter 7: Using Ability Scores";
        public string PHBChapter8StartLine => "# Chapter 8: Adventuring";
        public string PHBChapter9StartLine => "# Chapter 9: Combat";
        public string PHBChapter10StartLine => "# Chapter 10: Force- and Tech-casting";
        public string PHBChapter11StartLine => "# Chapter 11: Force Powers";
        public string PHBAppendixAStartLine => "# Appendix A: Conditions";
        public string PHBAppendixBStartLine => "# Appendix B: Recommended Variant Rules";
        public string PHBChangelogStartLine => "# Changelog";
        public string PHBPrefaceTitle => "Preface";
        public string PHBWhatsDifferentTitle => "Whats Different";
        public string PHBIntroductionTitle => "Introduction";
        public string PHBChapter1Title => "Step-By-Step Characters";
        public string PHBChapter2Title => "Species";
        public string PHBChapter3Title => "Classes";
        public string PHBChapter4Title => "Personality and Backgrounds";
        public string PHBChapter5Title => "Equipment";
        public string PHBChapter6Title => "Customization Options";
        public string PHBChapter7Title => "Using Ability Scores";
        public string PHBChapter8Title => "Adventuring";
        public string PHBChapter9Title => "Combat";
        public string PHBChapter10Title => "Force- and Tech-casting";
        public string PHBAppendixATitle => "Appendix A: Conditions";
        public string PHBAppendixBTitle => "Appendix B: Recommended Variant Rules";
        public string PHBChangelogTitle => "Changelog";
        public string PHBClassesStartLine => "##### Classes";
        public string PHBBackgroundsStartLine => "## Agent";
        public string PHBFeatsStartLine => "## Feats";
        #endregion

        #region PlayerHandbookClassProcessor
        public string PHBClassesTableStart => "##### Classes";
        public string PHBClassHitPointsAtFirstLevel => "**Hit Points at 1st Level:**";
        public string PHBClassHitPointsAtHigherLevel => "**Hit Points at Higher Level";
        public string PHBClassToolProficiencyChoice => "Your choice of";
        public string PHBClassBuildStartPattern => @"[#]+\s*Quick\s*Build";
        #endregion

        #region PlayerHandbookEquipmentProcessor
        public string PHBSimpleBlastersTableStart => "_Simple Blasters_";
        public string PHBMartialBlastersTableStart => "_Martial Blasters_";

        public string PHBVibroweaponsTableStart => "##### Vibroweapons";
        public string PHBLightweaponsTableStart => "##### Lightweapons";
        public string PHBArmorAndShieldsTableStart => "##### Armor and Shields";
        public string PHBArtisansImplementsTableStart => "_Artisan's implements_";
        public string PHBAmmunitionTableStart => "_Ammunition_";
        public string PHBMedicalTableStart => "_Medical_";
        #endregion

        #region PlayerHandbookFeatProcessor
        public string PHBFeatYourChoice => "your choice";
        public string PHBPowerStartLinesPattern => @"^\#\#\#\#\s+At-Will";
        public string PHBAttributeIncreaseIndexPattern(List<string> attributesChoices) =>
            $@"[Ii]ncrease.*({string.Join("|", attributesChoices)}).*score";
        #endregion

        #region ExpandedContentSpeciesProcessor
        public List<string> ECValidAttributeHints => new List<string>
        {
            Attribute.Charisma.ToString(), Attribute.Constitution.ToString(), Attribute.Dexterity.ToString(),
            Attribute.Intelligence.ToString(), Attribute.Strength.ToString(), Attribute.Wisdom.ToString(), "choice"
        };

        public string ECSpeciesColorScheme => "***Color Scheme***";
        public string ECSpeciesSkinColor => "***Skin Color***";
        public string ECSpeciesHairColor => "***Hair Color***";
        public string ECSpeciesEyeColor => "***Eye Color***";
        public string ECSpeciesDistinctions => "***Distinctions***";
        public string ECSpeciesHeight => "***Height***";
        public string ECSpeciesWeight => "***Weight***";
        public string ECSpeciesHomeworld => "***Homeworld***";
        public string ECSpeciesManufacturer => "***Manufacturer***";
        public string ECSpeciesLanguage => "***Language***";
        public string ECSpeciesPrimaryLanguage => "***Primary Language***";
        public string ECSpeciesAbilityScoreIncrease => "Ability Score Increase";
        public string ECSpeciesHalfHuman => "Half-human";
        #endregion

        #region StarshipChapterRulesProcessor
        public string SOTGChapter0StartLine => "# Introduction";
        public string SOTGChapter1StartLine => "# Chapter 1: Step-By-Step Starships";
        public string SOTGChapter2StartLine => "# Chapter 2: Deployments";
        public string SOTGChapter3StartLine => "# Chapter 3: Starships";
        public string SOTGChapter4StartLine => "# Chapter 4: Modifications";
        public string SOTGChapter5StartLine => "# Chapter 5: Equipment";
        public string SOTGChapter6StartLine => "# Chapter 6: Customization Options";
        public string SOTGChapter7StartLine => "# Chapter 7: Using Ability Scores";
        public string SOTGChapter8StartLine => "# Chapter 8: Adventuring";
        public string SOTGChapter9StartLine => "# Chapter 9: Combat";
        public string SOTGChapter10StartLine => "# Chapter 10: Generating Encounters";
        public string SOTGAppendixAStartLine => "# Appendix A: Conditions";
        public string SOTGChangelogStartLine => "# Changelog";
        public string SOTGChapter0Title => "Introduction";
        public string SOTGChapter1Title => "Step-By-Step Starships";
        public string SOTGChapter2Title => "Deployments";
        public string SOTGChapter3Title => "Starships";
        public string SOTGChapter4Title => "Modifications";
        public string SOTGChapter5Title => "Equipment";
        public string SOTGChapter6Title => "Customization Options";
        public string SOTGChapter7Title => "Using Ability Scores";
        public string SOTGChapter8Title => "Adventuring";
        public string SOTGChapter9Title => "Combat";
        public string SOTGChapter10Title => "Generating Encounters";
        public string SOTGAppendixATitle => "Appendix A: Conditions";
        public string SOTGChangelogTitle => "Changelog";
        public string SOTGDeploymentsStartLine => "##### Deployments";
        public string SOTGShipSizeStartLine => "## Tiny Ships";
        public string SOTGVariantStart => "## Variant: Space Stations";
        public string SOTGModificationsStart => "## Engineering Systems";
        public string SOTGVenturesStart => "## Ventures";
        public string SOTGVariantSpaceStations => "## Variant: Space Stations";
        #endregion

        #region StarshipDeploymentProcessor
        public string DeploymentsStart => "##### Deployments";
        public string GetDeploymentTableStart(string deploymentName) => $"##### The {deploymentName}";
        #endregion

        #region StarshipEquipmentProcessor
        public string SOTGArmorTableStartingLineArmorClass => "|Armor Class";
        public string SOTGArmorTableStartingLineShieldRegeneration => "|Shield Regeneration";
        public string SOTGSmallWeaponsTableStartingLine => "##### Ship Weapons (Small)";
        public string SOTGHugeWeaponsTableStartingLine => "##### Ship Weapons (Huge)";
        public string SOTGAmmunitionTableStartingLine => "##### Ammunition";
        public string SOTGHyperdrivesTableStartingLine => "##### Hyperdrives";
        public string SOTGNavcomputerTableStartingLine => "##### Navcomputer";
        public string SOTGArmorTableArmorStartingLine => "_armor_";
        public string SOTGArmorTableShieldsStartingLine => "_shields_";
        #endregion

        #region StarshipModificationProcessor
        public string SOTGEngineeringLinesStart => "## Engineering Systems";
        public string SOTGOperationsLinesStart => "## Operation Systems";
        public string SOTGSuitesLinesStart => "## Suite Systems";
        public string SOTGUniversalLinesStart => "## Universal Systems";
        public string SOTGWeaponsLinesStart => "## Weapon Systems";
        #endregion

        #region StarshipSizeProcessor
        public List<string> ShipSavingThrowOptions => new List<string> {"Strength", "Dexterity", "Constitution"};
        public string SOTGStarshipFeatures => "## Starship Features";
        public string SOTGStrengthAtTierZero => "Strength at Tier 0";
        public string SOTGDexterityAtTierZero => "Dexterity at Tier 0";
        public string SOTGConstitutionAtTierZero => "Constitution at Tier 0";
        public string SOTGHitDiceAtTierZero => "Hit Dice at Tier 0";
        public string SOTGHitPointsForSubsequentHitDie => "Hit Points for subsequent Hit Die:";
        public string SOTGMaximumSuiteSystems => "Maximum Suite Systems:";
        public string SOTGModificationSlotsAtTierZero => "Modification Slots at Tier 0:";
        public string SOTGStockModifications => "Stock Modifications:";
        public string SOTGSavingThrows => "Saving Throws:";
        public string SOTGStartingEquipment => "**Starting Equipment:**";
        public string SOTGClassTableStart => "<div class='classTable'>";
        public string SOTGClassTableEnd => "</div>";
        public string SOTGStarshipImprovements => "Starship Improvements";
        public string SOTGAdditionalModifications => "#### Additional Modifications";
        public string SOTGYourChoiceOf => " Your choice of ";
        public string SOTGSuiteChoiceSplitIndex => ", and";
        #endregion

        #region EnhancedItemProcessor
        public string SampleEnhancedItems => "### Sample Enhanced Items";
        public string RequiresAttunement => "Requires attunement";

        #endregion

        #region WretchedHivesChapterRulesProcessor
        public string WHChapter0StartLine => "# Introduction";
        public string WHChapter1StartLine => "# Chapter 1: Step-By-Step Factions";
        public string WHChapter2StartLine => "# Chapter 2: Entertainment and Downtime";
        public string WHChapter3StartLine => "# Chapter 3: Factions and Membership";
        public string WHChapter4StartLine => "# Chapter 4: Using Ability Scores";
        public string WHChapter5StartLine => "# Chapter 5: Equipment";
        public string WHChapter6StartLine => "# Chapter 6: Customization Options";
        public string WHChapter7StartLine => "# Chapter 7: Enhanced Items";
        public string WHChapter8StartLine => "# Chapter 8: Tool Proficiencies";
        public string WHAppendixAStartLine => "# Appendix A: Enhanced Items";
        public string WHChangelogStartLine => "# Changelog";
        public string WHChapter0Title => "Introduction";
        public string WHChapter1Title => "Step-By-Step Factions";
        public string WHChapter2Title => "Entertainment and Downtime";
        public string WHChapter3Title => "Factions and Membership";
        public string WHChapter4Title => "Using Ability Scores";
        public string WHChapter5Title => "Equipment";
        public string WHChapter6Title => "Customization Options";
        public string WHChapter7Title => "Enhanced Items";
        public string WHChapter8Title => "Tool Proficiencies";
        public string WHChangelogTitle => "Changelog";
        #endregion

        #region WretchedHivesEquipmentProcessor
        public string WHBlastersStartLine => "##### Blasters";
        public string WHVibroweaponsStartLine => "##### Vibroweapons";
        public string WHLightweaponsStartLine => "##### Lightweapons";
        public string WHArmorAndShieldsStartLine => "##### Armor and Shields";
        public string WHSpecialistsKitStartLine => "_Specialist's kit_";
        public string WHAmmunitionStartLine => "_Ammunition_";
        public string WHMedicalStartLine => "_Medical_";
        public string WHAlcoholicBeveragesStartLine => "_Alcoholic beverages_";
        public string WHDataRecordingAndStorageStartLine => "_Data Recording And Storage_";
        public string WHExplosivesStartLine => "_Explosives_";
        public string WHArtisansImplementsStartLine => "_Artisan's implements_";

        #endregion

        #region ExpandedContentArchetypeProcessor
        public string ECTableOfContentsStartLine => "# Table of Contents";
        public string ECFormIXTrakata => "Form IX: Trakata";
        public string ECFormIXTrakataMangled => "Form IX: Tr�kata";
        #endregion

        #region ExpandedContentBackgroundProcessor
        public string ECToolProficienciesStartLine => "**Tool Proficiencies";
        public string ECLanguagesStartLine => "**Languages";
        public string ECEquipmentStartLine => "**Equipment";
        public string ECSkillProficienciesPattern => @"\*\*Skill Profic[i]?encies";
        public string ECSuggestedCharacteristicsStartLine => "#### Suggested Characteristics";
        public string ECFeatureStartLine => "### Feature";
        public string ECFeatStartLine => "|Feat";
        public string ECPersonalityTraitsPattern => @"\|\s*Personality\s*Trait[s]?\s*\|";
        public string ECIdealsPattern => @"\|\s*Ideal[s]?\s*\|";
        public string ECBondsPattern => @"\|\s*Bond[s]?\s*\|";
        public string ECFlawsPattern => @"\|\s*Flaw[s]?\s*\|";

        #endregion

        #region ExpandedContentEquipmentProcessor

        public string ECBlastersStartLine => "##### Blasters";
        public string ECMartialLightweaponsStartLine => "_Martial Lightweapons_";
        public string ECMartialVibroweaponsStartLine => "_Martial Vibroweapons_";
        public string ECAmmunitionStartLine => "_Ammunition_";
        public string ECInterchangeableWeaponsSystemPattern => @"####\s+Interchangeable\s+Weapons\s+System";
        public string ECStrengthRequirementPattern => @"[,]*\s+?:([sS]trength\s+\d)";
        public string ECClassificationMartialBlasters => @"_\s*Martial\s*Blaster[s]?\s*_";
        public string ECClassificationSimpleBlasters => @"_\s*Simple\s*Blaster[s]?\s*_";
        public string ECClassificationSimpleLightweapons => @"_\s*Simple\s*Lightweapon[s]?\s*_";
        public string ECClassificationMartialLightweapons => @"_\s*Martial\s*Lightweapon[s]?\s*_";
        public string ECClassificationSimpleVibroweapons => @"_\s*Simple\s*Vibroweapon[s]?\s*_";
        public string ECClassificationMartialVibroweapons => @"_\s*Martial\s*Vibroweapon[s]?\s*_";
        public string ECClassificationUtilities => @"_\s*Utilities\s*_";
        public string ECClassificationWeaponAndArmorAccessories => @"_\s*Weapon\s*and\s*Armor\s*Accessories\s*_";
        public string ECClassificationAmmunition => @"_\s*Ammunition\s*_";
        public string ECClassificationClothing => @"_\s*Clothing\s*_";
        public string ECClassificationCommunication => @"_\s*Communications\s*_";
        public string ECClassificationDataRecordingAndStorage => @"_\s*Data\s*Recording\s*and\s*Storage\s*_";
        public string ECClassificationExplosives => @"_\s*Explosives\s*_";
        public string ECClassificationLifeSupport => @"_\s*Life\s*Support\s*_";
        public string ECClassificationMedical => @"_\s*Medical\s*_";
        public string ECClassificationStorage => @"_\s*Storage\s*_";
        public string ECClassificationArtisansImplements => @"_\s*Artisan's\s*implements\s*_";
        public string ECClassificationGamingSet => @"_\s*Gaming\s*set\s*_";
        public string ECClassificationMusicalInstrument => @"_\s*Musical\s*instrument\s*_";
        public string ECClassificationSpecialistsKit => @"_\s*Specialist's\s*kit\s*_";
        public string ECClassificationLightArmor => @"_\s*Light\s*Armor\s*_";
        public string ECClassificationMediumArmor => @"_\s*Medium\s*Armor\s*_";
        public string ECClassificationHeavyArmor => @"_\s*Heavy\s*Armor\s*_";
        public string ECClassificationShield => @"_\s*Shield\s*_";
        public string ECClassificationAlcoholicBeverages => @"_\s*Alcoholic\s*beverages\s*_";
        public string ECClassificationSpices => @"_\s*Spices\s*_";

        #endregion

        #region ExpandedContentForcePowersProcessor
        public string ECAtWillPattern => @"^\#\#\#\#\s+At-Will";
        #endregion

        #region ExpandedContentVariantRulesProcessor
        public string ECVariantRuleForceAlignment => "Force Alignment";
        public string ECVariantRuleDestiny => "Destiny";
        public string ECVariantRuleStarshipDestiny => "Starship Destiny";
        public string ECVariantRuleWeaponSundering => "Weapon Sundering";
        public string ECVariantRuleAdvancedAdvantage => "Advanced Advantage";
        public string ECVariantRuleLightsaberForms => "Lightsaber Forms";
        public string ECVariantRuleDismemberment => "Dismemberment";
        public string ECVariantRuleCompoundAdvantage => "Compound Advantage";
        public string ECVariantRuleForceAlignmentStartingLetter => "T";
        public string ECVariantRuleDestinyStartingLetter => "D";
        public string ECVariantRuleStarshipDestinyStartingLetter => "S";
        public string ECVariantRuleWeaponSunderingStartingLetter => "Y";
        public string ECVariantRuleAdvancedAdvantageStartingLetter => "T";
        public string ECVariantRuleLightsaberFormsStartingLetter => "T";
        public string ECVariantRuleDismembermentStartingLetter => "S";
        public string ECVariantRuleCompoundAdvantageStartingLetter => "T";
        #endregion

        #region MonsterManualProcessor
        public string MonsterArmorClass => "**Armor Class**";
        public string MonsterHitPoints => "**Hit Points**";
        public string MonsterSpeed => "**Speed**";
        public string MonsterAttributes => "|STR|DEX|CON|INT|WIS|CHA|";
        public string MonsterSavingThrows => "**Saving Throws**";
        public string MonsterSkills => "**Skills**";
        public string MonsterDamageVulnerabilities => "**Damage Vulnerabilities**";
        public string MonsterDamageVulnerabilitiesPattern => @"\*\*Damage Vulnerabilities\*\*";
        public string MonsterDamageImmunities => "**Damage Immunities**";
        public string MonsterDamageResistances => "**Damage Resistances**";
        public string MonsterConditionImmunities => "**Condition Immunities**";
        public string MonsterSenses => "**Senses**";
        public string MonsterLanguages => "**Languages**";
        public string MonsterChallenge => "**Challenge**";
        public string MonsterBehaviorLegendary => "Legendary";
        public string MonsterBehaviorActions => "Actions";
        public string MonsterBehaviorReactions => "Reactions";
        public string MonsterHitSplitPattern => @"\*Hit[:]*\*|Hit:";
        public string AtWill => "At Will";
        public string FirstLevel => "1st Level";
        public string SecondLevel => "2nd Level";
        public string ThirdLevel => "3rd Level";
        public string FourthLevel => "4th Level";
        public string FifthLevel => "5th Level";
        public string SixthLevel => "6th Level";
        public string SeventhLevel => "7th Level";
        public string EighthLevel => "8th Level";
        public string NinthLevel => "9th Level";

        #endregion

        #region ReferenceTableProcessor
        public string ReferenceTableNameAbilityScorePointCost => "Ability Score Point Cost";
        public string ReferenceTableNameAbilityScoresAndModifiers => "Ability Scores and Modifiers";
        public string ReferenceTableNameXPByLevel => "XP and PB by Level";
        public string ReferenceTableNameStartingWealthByClass => "Variant: Starting Wealth by Class";
        public string ReferenceTableNameLifestyleExpenses => "Lifestyle Expenses";
        public string ReferenceTableNameMulticlassingPrerequisites => "Multiclassing Prerequisites";
        public string ReferenceTableNameMulticlassingProficiencies => "Multiclassing Proficiencies";
        public string ReferenceTableNameStarshipSizeStockCost => "Starship Size Stock Cost";
        public string ReferenceTableNameStarshipSizeBuildingWorkforce => "Starship Size Construction Workforce";
        public string ReferenceTableNameBaseUpgradeCostByTier => "Base Upgrade Cost by Tier";
        public string ReferenceTableNameStarshipSizeUpgradeCost => "Starship Size Upgrade Cost";
        public string ReferenceTableNameStarshipSizeUpgradeWorkforce => "Starship Size Upgrade Workforce";
        public string ReferenceTableNameModificationCategoryBaseCost => "Modification Category Base Cost";
        public string ReferenceTableNameStarshipSizeModificationCost => "Starship Size Modification Cost";
        public string ReferenceTableNameStarshipSizeModificationWorkforce => "Starship Size Modification Workforce";
        public string ReferenceTableNameModificationTierRequirementDC => "Modification Tier Requirement DC";
        public string ReferenceTableNameModificationSlotsAtTierZero => "Modification Slots at  Tier 0";
        public string ReferenceTableNameStarshipSizeEquipmentCost => "Starship Size Equipment Cost";
        public string ReferenceTableNameStarshipSizeEquipmentWorkforce => "Starship Size Equipment Workforce";
        public string ReferenceTableNameStarshipSizeCargoCapacity => "Starship Size Cargo Capacity";
        public string ReferenceTableNameStarshipSizeBaseArmorClass => "Starship Size Base Armor Class";
        public string ReferenceTableNameStarshipSizeFuelCost => "Starship Size Fuel Cost";
        public string ReferenceTableNameStarshipSizeFuelCapacity => "Starship Size Fuel Capacity";
        public string ReferenceTableNameStarshipSizeFoodCapacity => "Starship Size Food Capacity";
        public string ReferenceTableNameStarshipSizeBaseFlyingSpeed => "Starship Size Base Flying Speed";
        public string ReferenceTableNameStarshipSizeBaseTurningSpeed => "Starship Size Base Turning Speed";
        public string ReferenceTableNameSampleRealspaceTravelTimes => "Sample Realspace Travel Times";
        public string ReferenceTableNameSampleHyperspaceTravelTimes => "Sample Hyperspace Travel Times";
        public string ReferenceTableNameAstrogationTimeTaken => "Astrogation Time Taken";
        public string ReferenceTableNameHyperspaceMishaps => "Hyperspace Mishaps";
        public string ReferenceTableNameStarshipSizeMinimumCrew => "Starship Size Minimum Crew";
        public string ReferenceTableNameStarshipSizeRepairTime => "Starship Size Repair Time";
        public string ReferenceTableNameStarshipSizeMaintenanceTime => "Starship Size Maintenance Time";
        public string ReferenceTableNameStarshipSizeCategories => "Starship Size Categories";
        public string ReferenceTableNameSystemDamage => "System Damage";
        public string ReferenceTableNameStarshipSizeMaximumSuites => "Starship Size Maximum Suites";
        public string ReferenceTableNameStarshipSizeSuiteCapacity => "Starship Size Suite Capacity";
        public string ReferenceTableNameCyberneticAugmentationSideEffects => "Cybernetic Augmentation Side Effects";
        public string ReferenceTableStartingLineAbilityScorePointCost => "##### Ability Score Point Cost";
        public string ReferenceTableStartingLineAbilityScoresAndModifiers => "##### Ability Scores and Modifiers";
        public string ReferenceTableStartingLineXPByLevel => "|Experience Points|Level|Proficiency Bonus|";
        public string ReferenceTableStartingLineStartingWealthByClass => "#### Variant: Starting Wealth by Class";
        public string ReferenceTableStartingLineLifestyleExpenses => "##### Lifestyle Expenses";
        public string ReferenceTableStartingLineMulticlassingPrerequisites => "#### Multiclassing Prerequisites";
        public string ReferenceTableStartingLineMulticlassingProficiencies => "### Multiclassing Proficiencies";
        public string ReferenceTableStartingLineStarshipSizeStockCost => "#### Starship Size Stock Cost";
        public string ReferenceTableStartingLineStarshipSizeBuildingWorkforce => "#### Starship Size Construction Workforce";
        public string ReferenceTableStartingLineBaseUpgradeCostByTier => "#### Base Upgrade Cost by Tier";
        public string ReferenceTableStartingLineStarshipSizeUpgradeCost => "#### Starship Size Upgrade Cost";
        public string ReferenceTableStartingLineStarshipSizeUpgradeWorkforce => "#### Starship Size Upgrade Workforce";
        public string ReferenceTableStartingLineModificationCategoryBaseCost => "#### Modification Category Base Cost";
        public string ReferenceTableStartingLineStarshipSizeModificationCost => "#### Starship Size Modification Cost";
        public string ReferenceTableStartingLineStarshipSizeModificationWorkforce => "#### Starship Size Modification Workforce";
        public string ReferenceTableStartingLineModificationTierRequirementDC => "#### Modification Tier Requirement DC";
        public string ReferenceTableStartingLineModificationSlotsAtTierZero => "### Modification Slots at  Tier 0";
        public string ReferenceTableStartingLineStarshipSizeEquipmentCost => "#### Starship Size Equipment Cost";
        public string ReferenceTableStartingLineStarshipSizeEquipmentWorkforce => "#### Starship Size Equipment Workforce";
        public string ReferenceTableStartingLineStarshipSizeCargoCapacity => "#### Starship Size Cargo Capacity";
        public string ReferenceTableStartingLineStarshipSizeBaseArmorClass => "#### Starship Size Base Armor Class";
        public string ReferenceTableStartingLineStarshipSizeFuelCost => "#### Starship Size Fuel Cost";
        public string ReferenceTableStartingLineStarshipSizeFuelCapacity => "#### Starship Size Fuel Capacity";
        public string ReferenceTableStartingLineStarshipSizeFoodCapacity => "#### Starship Size Food Capacity";
        public string ReferenceTableStartingLineStarshipSizeBaseFlyingSpeed => "#### Starship Size Base Flying Speed";
        public string ReferenceTableStartingLineStarshipSizeBaseTurningSpeed => "#### Starship Size Base Turning Speed";
        public string ReferenceTableStartingLineSampleRealspaceTravelTimes => "#### Sample Realspace Travel Times";
        public string ReferenceTableStartingLineSampleHyperspaceTravelTimes => "#### Sample Hyperspace Travel Times";
        public string ReferenceTableStartingLineAstrogationTimeTaken => "#### Astrogation Time Taken";
        public string ReferenceTableStartingLineHyperspaceMishaps => "#### Hyperspace Mishaps";
        public string ReferenceTableStartingLineStarshipSizeMinimumCrew => "#### Starship Size Minimum Crew";
        public string ReferenceTableStartingLineStarshipSizeRepairTime => "#### Starship Size Repair Time";
        public string ReferenceTableStartingLineStarshipSizeMaintenanceTime => "#### Starship Size Maintenance Time";
        public string ReferenceTableStartingLineStarshipSizeCategories => "#### Size Categories";
        public string ReferenceTableStartingLineSystemDamage => "#### System Damage";
        public string ReferenceTableStartingLineStarshipSizeMaximumSuites => "#### Starship Size Maximum Suites";
        public string ReferenceTableStartingLineStarshipSizeSuiteCapacity => "#### Starship Size Suite Capacity";
        public string ReferenceTableStartingLineCyberneticAugmentationSideEffects => "#### Cybernetic Augmentation Side Effects";

        public string SOTGTertiaryAmmunitionTableStartingLine => throw new NotImplementedException();

        public string SOTGQuaternaryAmmunitionTableStartingLine => throw new NotImplementedException();

        public string SOTGReactorsTableStartingLineFuelCosts => throw new NotImplementedException();

        public string SOTGReactorsTableReactorsStartingLine => throw new NotImplementedException();

        public string SOTGReactorsTablePowerCouplingsStartingLine => throw new NotImplementedException();

        public string ReferenceTableNameDeploymentRankPrestige => throw new NotImplementedException();

        public string ReferenceTableNameModificationCapacityByShipSize => throw new NotImplementedException();

        public string ReferenceTableNameModificationGradeInstallationByShipTier => throw new NotImplementedException();

        public string ReferenceTableNameBaseHyperspaceTravelTimes => throw new NotImplementedException();

        public string ReferenceTableNameStarshipSizeRefittingTime => throw new NotImplementedException();

        public string ReferenceTableNameStarshipSlowedLevel => throw new NotImplementedException();

        public string ReferenceTableStartingLineDeploymentRankPrestige => throw new NotImplementedException();

        public string ReferenceTableStartingLineModificationCapacityByShipSize => throw new NotImplementedException();

        public string ReferenceTableStartingLineModificationGradeInstallationByShipTier => throw new NotImplementedException();

        public string ReferenceTableStartingLineBaseHyperspaceTravelTimes => throw new NotImplementedException();

        public string ReferenceTableStartingLineStarshipSizeRefittingTime => throw new NotImplementedException();

        public string ReferenceTableStartingLineStarshipSlowedLevel => throw new NotImplementedException();

        public string ECStorageStartLine => throw new NotImplementedException();

        #endregion
    }
}
