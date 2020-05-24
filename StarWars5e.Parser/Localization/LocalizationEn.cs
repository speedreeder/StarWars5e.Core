using System.Collections.Generic;
using StarWars5e.Models.Enums;

namespace StarWars5e.Parser.Localization
{
    public class LocalizationEn : ILocalization
    {
        public Language Language => Language.en;

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

        #endregion

        #region WretchedHivesManager
        public List<(string name, string startLine, int occurence)> WretchedHivesWeaponProperties => new List<(string name, string startLine, int occurence)>
        {
            ("Auto", "#### Auto", 1),
            ("Defensive", "#### Defensive", 1),
            ("Dire", "#### Dire", 1),
            ("Disarming", "#### Disarming", 1),
            ("Disguised", "#### Disguised", 1),
            ("Disintegrate", "#### Disintegrate", 1),
            ("Disruptive", "#### Disruptive", 1),
            ("Keen", "#### Keen", 1),
            ("Mighty", "#### Mighty", 1),
            ("Piercing", "#### Piercing", 1),
            ("Rapid", "#### Rapid", 1),
            ("Shocking", "#### Shocking", 1),
            ("Silent", "#### Silent", 2),
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
            ("Burst", "#### Burst", 1),
            ("Dexterity", "#### Dexterity", 1),
            ("Double", "#### Double", 1),
            ("Finesse", "#### Finesse", 1),
            ("Fixed", "#### Fixed", 1),
            ("Heavy", "#### Heavy", 3 ),
            ("Hidden", "#### Hidden", 1),
            ("Light", "#### Light", 2),
            ("Luminous", "#### Luminous", 1),
            ("Range", "#### Range", 1),
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
        public string PHBBlastersTableStart => "##### Blasters";
        public string PHBVibroweaponsTableStart => "##### Vibroweapons";
        public string PHBLightweaponsTableStart => "##### Lightweapons";
        public string PHBArmorAndShieldsTableStart => "##### Armor and Shields";
        public string PHBArtisansToolsTableStart => "_Artisan's tools_";
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
        public List<string> ValidAttributeHints => new List<string>
        {
            Attribute.Charisma.ToString(), Attribute.Constitution.ToString(), Attribute.Dexterity.ToString(),
            Attribute.Intelligence.ToString(), Attribute.Strength.ToString(), Attribute.Wisdom.ToString(), "choice"
        };

        public string SpeciesColorScheme => "***Color Scheme***";
        public string SpeciesSkinColor => "***Skin Color***";
        public string SpeciesHairColor => "***Hair Color***";
        public string SpeciesEyeColor => "***Eye Color***";
        public string SpeciesDistinctions => "***Distinctions***";
        public string SpeciesHeight => "***Height***";
        public string SpeciesWeight => "***Weight***";
        public string SpeciesHomeworld => "***Homeworld***";
        public string SpeciesManufacturer => "***Manufacturer***";
        public string SpeciesLanguage => "***Language***";
        public string SpeciesPrimaryLanguage => "***Primary Language***";
        public string SpeciesAbilityScoreIncrease => "Ability Score Increase";
        public string SpeciesHalfHuman => "Half-Human";
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
    }
}
