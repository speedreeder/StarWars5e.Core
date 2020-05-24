using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        public string PHBBlastersTableStart { get; }
        public string PHBVibroweaponsTableStart { get; }
        public string PHBLightweaponsTableStart { get; }
        public string PHBArmorAndShieldsTableStart { get; }
        public string PHBArtisansToolsTableStart { get; }
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
        public List<string> ValidAttributeHints { get; }
        public string SpeciesColorScheme { get; }
        public string SpeciesSkinColor { get; }
        public string SpeciesHairColor{ get; }
        public string SpeciesEyeColor{ get; }
        public string SpeciesDistinctions { get; }
        public string SpeciesHeight { get; }
        public string SpeciesWeight { get; }
        public string SpeciesHomeworld { get; }
        public string SpeciesManufacturer { get; }
        public string SpeciesLanguage { get; }
        public string SpeciesPrimaryLanguage { get; }
        public string SpeciesAbilityScoreIncrease { get; }
        public string SpeciesHalfHuman { get; }
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
        #endregion
    }
}
