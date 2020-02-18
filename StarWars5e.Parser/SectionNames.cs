using System.Collections.Generic;
using StarWars5e.Models.Enums;
// ReSharper disable InconsistentNaming

namespace StarWars5e.Parser
{
    public static class SectionNames
    {
        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> PHBChapterNames { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Player's Handbook", GlobalSearchTermType.Book, "/rules/handbook"),
                ( "Introduction", GlobalSearchTermType.HandbookChapter, "/rules/handbook"),
                ( "What's Different?", GlobalSearchTermType.HandbookChapter, "/rules/handbook/whatsDifferent"),
                ( "Chapter 1: Step-By-Step Characters", GlobalSearchTermType.HandbookChapter, "/rules/handbook/stepByStep"),
                ( "Chapter 2: Species", GlobalSearchTermType.HandbookChapter, "/rules/handbook/species"),
                ( "Chapter 3: Classes", GlobalSearchTermType.HandbookChapter, "/rules/handbook/classes"),
                ( "Chapter 4: Backgrounds", GlobalSearchTermType.HandbookChapter, "/rules/handbook/backgrounds"),
                ( "Chapter 5: Equipment", GlobalSearchTermType.HandbookChapter, "/rules/handbook/equipment"),
                ( "Chapter 6: Customization Options", GlobalSearchTermType.HandbookChapter, "/rules/handbook/customization"),
                ( "Chapter 7: Using Ability Scores", GlobalSearchTermType.HandbookChapter, "/rules/handbook/abilityScores"),
                ( "Chapter 8: Adventuring", GlobalSearchTermType.HandbookChapter, "/rules/handbook/adventuring"),
                ( "Chapter 9: Combat", GlobalSearchTermType.HandbookChapter, "/rules/handbook/combat"),
                ( "Chapter 10: Force- And Tech- Casting", GlobalSearchTermType.HandbookChapter, "/rules/handbook/casting"),
                ( "Appendix A: Conditions", GlobalSearchTermType.HandbookChapter, "/rules/handbook/conditions"),
                ( "Appendix B: Recommended Variant Rules", GlobalSearchTermType.HandbookChapter, "/rules/handbook/variantRules"),
                ( "Handbook Changelog", GlobalSearchTermType.Changelog, "/rules/handbook/changelog")
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> SOTGChapterNames { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Starships Of The Galaxy", GlobalSearchTermType.Book, "/rules/starships"),
                ( "Introduction", GlobalSearchTermType.StarshipChapter, "/rules/starships"),
                ( "Chapter 1: Step-By-Step Starships", GlobalSearchTermType.StarshipChapter, "/rules/starships/stepByStep"),
                ( "Chapter 2: Deployments", GlobalSearchTermType.StarshipChapter, "/rules/starships/deployments"),
                ( "Chapter 3: Starships", GlobalSearchTermType.StarshipChapter, "/rules/starships/starshipsizes"),
                ( "Chapter 4: Modifications", GlobalSearchTermType.StarshipChapter, "/rules/starships/modifications"),
                ( "Chapter 5: Equipment", GlobalSearchTermType.StarshipChapter, "/rules/starships/equipment"),
                ( "Chapter 6: Customization Options", GlobalSearchTermType.StarshipChapter, "/rules/starships/customization"),
                ( "Chapter 7: Using Ability Scores", GlobalSearchTermType.StarshipChapter, "/rules/starships/abilityScores"),
                ( "Chapter 8: Adventuring", GlobalSearchTermType.StarshipChapter, "/rules/starships/adventuring"),
                ( "Chapter 9: Combat", GlobalSearchTermType.StarshipChapter, "/rules/starships/combat"),
                ( "Chapter 10: Generating Encounters", GlobalSearchTermType.StarshipChapter, "/rules/starships/generatingEncounters"),
                ( "Appendix A: Conditions", GlobalSearchTermType.StarshipChapter, "/rules/starships/conditions"),
                ( "Starship Changelog", GlobalSearchTermType.Changelog, "/rules/starships/changelog")
            };
        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> WHChapterNames { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Wretched Hives", GlobalSearchTermType.Book, "/rules/hives"),
                //( "Introduction", GlobalSearchTermType.StarshipChapter, "/rules/starships"),
                ( "Chapter 1: Step-By-Step Factions", GlobalSearchTermType.WretchedHivesChapter, "/rules/hives/stepByStep"),
                ( "Chapter 2: Entertainment and Downtime", GlobalSearchTermType.WretchedHivesChapter, "/rules/hives/downtime"),
                ( "Chapter 3: Factions and Membership", GlobalSearchTermType.WretchedHivesChapter, "/rules/hives/factions"),
                ( "Chapter 4: Using Ability Scores", GlobalSearchTermType.WretchedHivesChapter, "/rules/hives/abilityScores"),
                ( "Chapter 5: Enhanced Items", GlobalSearchTermType.WretchedHivesChapter, "/rules/hives/enhancedItems"),
                ( "Chapter 6: Modifiable Items", GlobalSearchTermType.WretchedHivesChapter, "/rules/hives/modifiableItems"),
                ( "Chapter 7: Cybernetic Augmentations", GlobalSearchTermType.WretchedHivesChapter, "/rules/hives/cyberneticAugmentations"),
                ( "Chapter 8: Droid Customizations", GlobalSearchTermType.WretchedHivesChapter, "/rules/hives/droidCustomizations"),
                ( "Chapter 9: Tool Proficiencies", GlobalSearchTermType.WretchedHivesChapter, "/rules/hives/toolProficiencies"),
                //( "Chapter 10: Generating Encounters", GlobalSearchTermType.StarshipChapter, "/rules/starships/generatingEncounters"),
                ( "Appendix A: Enhanced Items", GlobalSearchTermType.WretchedHivesChapter, "/rules/hives/enhancedItems"),
                ( "Starship Changelog", GlobalSearchTermType.Changelog, "/rules/hives/changelog")
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> MonsterChapterNames { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Monster Manual", GlobalSearchTermType.Book, "/rules/monsters")
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> ReferenceNames { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Monsters", GlobalSearchTermType.Reference, "/rules/monsters/monsters"),
                ( "Classes", GlobalSearchTermType.Reference, "/characters/classes"),
                ( "Species", GlobalSearchTermType.Reference, "/characters/species"),
                ( "Archetypes", GlobalSearchTermType.Reference, "/characters/archetypes"),
                ( "Backgrounds", GlobalSearchTermType.Reference, "/characters/backgrounds"),
                ( "Armor", GlobalSearchTermType.Reference, "/loot/armor"),
                ( "Weapons", GlobalSearchTermType.Reference, "/loot/weapons"),
                ( "Adventuring Gear", GlobalSearchTermType.Reference, "/reference/adventuringGear"),
                ( "Enhanced Items", GlobalSearchTermType.Reference, "/reference/enhancedItems"),
                ( "Feats", GlobalSearchTermType.Reference, "/characters/feats"),
                ( "Force Powers", GlobalSearchTermType.Reference, "/characters/forcePowers"),
                ( "Tech Powers", GlobalSearchTermType.Reference, "/characters/techPowers"),
                ( "Starship Modifications", GlobalSearchTermType.Reference, "/starships/modifications"),
                ( "Starship Equipment", GlobalSearchTermType.Reference, "/starships/equipment"),
                ( "Starship Weapons", GlobalSearchTermType.Reference, "/starships/weapons"),
                ( "Ventures", GlobalSearchTermType.Reference, "/starships/ventures"),
                ( "Additional Variant Rules", GlobalSearchTermType.Reference, "/characters/additionalVariantRules"),
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> VariantRuleNames { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Destiny", GlobalSearchTermType.VariantRule, "/characters/additionalVariantRules/Destiny"),
                ( "Force Alignment", GlobalSearchTermType.VariantRule, "/characters/additionalVariantRules/Force%20Alignment"),
                ( "Starship Destiny", GlobalSearchTermType.VariantRule, "/characters/additionalVariantRules/Starship%20Destiny"),
                ( "Weapon Sundering", GlobalSearchTermType.VariantRule, "/characters/additionalVariantRules/Weapon%20Sundering")
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> PHBChapterOneSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Level", GlobalSearchTermType.Rule, null),
                ( "Hit Points and Hit Dice", GlobalSearchTermType.Rule, null),
                ( "Proficiency Bonus", GlobalSearchTermType.Rule, null),
                ( "Variant: Customizing Ability Scores", GlobalSearchTermType.VariantRule, null),
                ( "Ability Score Point Cost", GlobalSearchTermType.Table, null),
                ( "Ability Scores and Modifiers", GlobalSearchTermType.Table, null),
                ( "Armor Class", GlobalSearchTermType.Rule, null),
                ( "Weapons", GlobalSearchTermType.Rule, null ),
                ( "Character Advancement", GlobalSearchTermType.Table, null )
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> PHBChapterTwoSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Choosing A Species", GlobalSearchTermType.Rule, null),
                ( "Ability Score Increase", GlobalSearchTermType.Rule, null),
                ( "Age", GlobalSearchTermType.Rule, null),
                ( "Alignment", GlobalSearchTermType.Rule, null),
                ( "Size", GlobalSearchTermType.Rule, null),
                ( "Speed", GlobalSearchTermType.Rule, null),
                ( "Languages", GlobalSearchTermType.Rule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> PHBChapterThreeSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>();

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> PHBChapterFourSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Name", GlobalSearchTermType.Rule, null),
                ( "Sex", GlobalSearchTermType.Rule, null),
                ( "Height And Weight", GlobalSearchTermType.Rule, null),
                ( "Other Physical Characteristics", GlobalSearchTermType.Rule, null),
                ( "Alignment", GlobalSearchTermType.Rule, null),
                ( "Languages", GlobalSearchTermType.Rule, null),
                ( "Personal Characteristics", GlobalSearchTermType.Rule, null),
                ( "Personality Traits", GlobalSearchTermType.Rule, null),
                ( "Ideals", GlobalSearchTermType.Rule, null),
                ( "Bonds", GlobalSearchTermType.Rule, null),
                ( "Flaws", GlobalSearchTermType.Rule, null),
                ( "Inspiration", GlobalSearchTermType.Rule, null),
                ( "Backgrounds", GlobalSearchTermType.Rule, null),
                ( "Customizing A Background", GlobalSearchTermType.Rule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> PHBChapterFiveSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Variant: Starting Wealth By Class", GlobalSearchTermType.Table, null),
                ( "Wealth", GlobalSearchTermType.Rule, null),
                ( "Currency", GlobalSearchTermType.Rule, null),
                ( "Selling Treasure", GlobalSearchTermType.Rule, null),
                ( "Armor and Shields", GlobalSearchTermType.Rule, null),
                ( "Variant: Equipment Sizes", GlobalSearchTermType.VariantRule, null),
                ( "Armor And Shield Proficiency", GlobalSearchTermType.Rule, null),
                ( "Armor Class (AC)", GlobalSearchTermType.EquipmentProperty, null),
                ( "Strength", GlobalSearchTermType.EquipmentProperty, null),
                ( "Stealth", GlobalSearchTermType.EquipmentProperty, null),
                ( "Light Armor", GlobalSearchTermType.Rule, null),
                ( "Medium Armor", GlobalSearchTermType.Rule, null),
                ( "Powered Battle Armor", GlobalSearchTermType.Rule, null),
                ( "Heavy Armor", GlobalSearchTermType.Rule, null),
                ( "Shield Generators", GlobalSearchTermType.Rule, null),
                ( "Getting Into And Out Of Armor", GlobalSearchTermType.Rule, null),
                ( "Donning And Doffing Armor", GlobalSearchTermType.Rule, null),
                ( "Weapons", GlobalSearchTermType.Rule, null),
                ( "Ammunition", GlobalSearchTermType.WeaponProperty, null),
                ( "Burst", GlobalSearchTermType.WeaponProperty, null),
                ( "Double", GlobalSearchTermType.WeaponProperty, null),
                ( "Finesse", GlobalSearchTermType.WeaponProperty, null),
                ( "Heavy", GlobalSearchTermType.WeaponProperty, null),
                ( "Hidden", GlobalSearchTermType.WeaponProperty, null),
                ( "Light", GlobalSearchTermType.WeaponProperty, null),
                ( "Luminous", GlobalSearchTermType.WeaponProperty, null),
                ( "Range", GlobalSearchTermType.WeaponProperty, null),
                ( "Reach", GlobalSearchTermType.WeaponProperty, null),
                ( "Reload", GlobalSearchTermType.WeaponProperty, null),
                ( "Special", GlobalSearchTermType.WeaponProperty, null),
                ( "Strength", GlobalSearchTermType.WeaponProperty, "strength 2"),
                ( "Thrown", GlobalSearchTermType.WeaponProperty, null),
                ( "Two-Handed", GlobalSearchTermType.WeaponProperty, null),
                ( "Versatile", GlobalSearchTermType.WeaponProperty, null),
                ( "Improvised Weapons", GlobalSearchTermType.Rule, null),
                ( "Special Weapons", GlobalSearchTermType.Rule, null),
                ( "Adventuring Gear", GlobalSearchTermType.Rule, null),
                ( "Burglar's Pack", GlobalSearchTermType.AdventuringGear, null),
                ( "Diplomat's Pack", GlobalSearchTermType.AdventuringGear, null),
                ( "Dungeoneer's Pack", GlobalSearchTermType.AdventuringGear, null),
                ( "Entertainer's Pack", GlobalSearchTermType.AdventuringGear, null),
                ( "Explorer's Pack", GlobalSearchTermType.AdventuringGear, null),
                ( "Priest's Pack", GlobalSearchTermType.AdventuringGear, null),
                ( "Scholar's Pack", GlobalSearchTermType.AdventuringGear, null),
                ( "Technologist's Pack", GlobalSearchTermType.AdventuringGear, null),
                ( "Trade Goods", GlobalSearchTermType.Rule, null ),
                ( "Droids", GlobalSearchTermType.Rule, null ),
                ( "Mounts and Vehicles", GlobalSearchTermType.Rule, null ),
                ( "Mounts and Other Animals", GlobalSearchTermType.Table, null ),
                ( "Tack, Harness, and Drawn Vehicles", GlobalSearchTermType.Table, null ),
                ( "Vehicles", GlobalSearchTermType.Rule, null ),
                ( "Lifestyle Expenses", GlobalSearchTermType.Rule, null ),
                ( "Food, Drink, And Lodging", GlobalSearchTermType.Rule, null ),
                ( "Services", GlobalSearchTermType.Rule, null )
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> PHBChapterSixSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Multiclassing", GlobalSearchTermType.Rule, null),
                ( "Fighting Styles", GlobalSearchTermType.Rule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> PHBChapterSevenSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Ability Scores And Modifiers", GlobalSearchTermType.Rule, null),
                ( "Advantage and Disadvantage", GlobalSearchTermType.Rule, null),
                ( "Proficiency Bonus", GlobalSearchTermType.Rule, null),
                ( "Ability Checks", GlobalSearchTermType.Rule, null),
                ( "Contests", GlobalSearchTermType.Rule, null),
                ( "Passive Checks", GlobalSearchTermType.Rule, null),
                ( "Working Together", GlobalSearchTermType.Rule, null),
                ( "Strength", GlobalSearchTermType.Rule, null),
                ( "Dexterity", GlobalSearchTermType.Rule, null),
                ( "Constitution", GlobalSearchTermType.Rule, null),
                ( "Hiding", GlobalSearchTermType.Rule, null),
                ( "Intelligence", GlobalSearchTermType.Rule, null),
                ( "Wisdom", GlobalSearchTermType.Rule, null),
                ( "Charisma", GlobalSearchTermType.Rule, null),
                ( "Saving Throws", GlobalSearchTermType.Rule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> PHBChapterEightSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Movement", GlobalSearchTermType.Rule, null),
                ( "Speed", GlobalSearchTermType.Rule, null),
                ( "Difficult Terrain", GlobalSearchTermType.Rule, null),
                ( "Climbing, Swimming, and Crawling", GlobalSearchTermType.Rule, null),
                ( "Jumping", GlobalSearchTermType.Rule, null),
                ( "Activity While Traveling", GlobalSearchTermType.Rule, null),
                ( "Marching Order", GlobalSearchTermType.Rule, null),
                ( "Stealth", GlobalSearchTermType.Rule, null),
                ( "Noticing Threats", GlobalSearchTermType.Rule, null),
                ( "Splitting Up The Party", GlobalSearchTermType.Rule, null),
                ( "The Environment", GlobalSearchTermType.Rule, null),
                ( "Falling", GlobalSearchTermType.Rule, null),
                ( "Suffocating", GlobalSearchTermType.Rule, null),
                ( "Vision And Light", GlobalSearchTermType.Rule, null),
                ( "Blindsight", GlobalSearchTermType.Rule, null),
                ( "Darkvision", GlobalSearchTermType.Rule, null),
                ( "Truesight", GlobalSearchTermType.Rule, null),
                ( "Food And Water", GlobalSearchTermType.Rule, null),
                ( "Interacting With Objects", GlobalSearchTermType.Rule, null),
                ( "Social Interaction", GlobalSearchTermType.Rule, null),
                ( "Roleplaying", GlobalSearchTermType.Rule, null),
                ( "Resting", GlobalSearchTermType.Rule, null),
                ( "Short Rest", GlobalSearchTermType.Rule, null),
                ( "Long Rest", GlobalSearchTermType.Rule, null),
                ( "Between Adventures", GlobalSearchTermType.Rule, null),
                ( "Downtime Activities", GlobalSearchTermType.Rule, null),
                ( "Crafting", GlobalSearchTermType.Rule, null),
                ( "Practicing A Profession", GlobalSearchTermType.Rule, null),
                ( "Recuperating", GlobalSearchTermType.Rule, null),
                ( "Researching", GlobalSearchTermType.Rule, null),
                ( "Training", GlobalSearchTermType.Rule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> PHBChapterNineSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "The Order Of Combat", GlobalSearchTermType.Rule, null),
                ( "Surprise", GlobalSearchTermType.Rule, null),
                ( "Your Turn", GlobalSearchTermType.Rule, null),
                ( "Bonus Actions", GlobalSearchTermType.Rule, null),
                ( "Other Activity On Your Turn", GlobalSearchTermType.Rule, null),
                ( "Interacting with Objects Around You", GlobalSearchTermType.Rule, null),
                ( "Reactions", GlobalSearchTermType.Rule, null),
                ( "Movement And Position", GlobalSearchTermType.Rule, null),
                ( "Breaking Up Your Move", GlobalSearchTermType.Rule, null),
                ( "Difficult Terrain", GlobalSearchTermType.Rule, null),
                ( "Moving Between Attacks", GlobalSearchTermType.Rule, null),
                ( "Being Prone", GlobalSearchTermType.Rule, null),
                ( "Moving Around Other Creatures", GlobalSearchTermType.Rule, null),
                ( "Flying Movement", GlobalSearchTermType.Rule, null),
                ( "Creature Size", GlobalSearchTermType.Rule, null),
                ( "Squeezing Into A Smaller Space", GlobalSearchTermType.Rule, null),
                ( "Variant: Playing On A Grid", GlobalSearchTermType.VariantRule, null),
                ( "Action In Combat", GlobalSearchTermType.Rule, null),
                ( "Attack", GlobalSearchTermType.Rule, null),
                ( "Cast A Power", GlobalSearchTermType.Rule, null),
                ( "Dash", GlobalSearchTermType.Rule, null),
                ( "Disengage", GlobalSearchTermType.Rule, null),
                ( "Dodge", GlobalSearchTermType.Rule, null),
                ( "Help", GlobalSearchTermType.Rule, null),
                ( "Hide", GlobalSearchTermType.Rule, null),
                ( "Ready", GlobalSearchTermType.Rule, null),
                ( "Search", GlobalSearchTermType.Rule, null),
                ( "Use An Object", GlobalSearchTermType.Rule, null),
                ( "Making An Attack", GlobalSearchTermType.Rule, null),
                ( "Attack Rolls", GlobalSearchTermType.Rule, null),
                ( "Unseen Attackers And Targets", GlobalSearchTermType.Rule, null),
                ( "Ranged Attacks", GlobalSearchTermType.Rule, null),
                ( "Opportunity Attacks", GlobalSearchTermType.Rule, null),
                ( "Two-Weapon Fighting", GlobalSearchTermType.Rule, null),
                ( "Range", GlobalSearchTermType.Rule, null),
                ( "Ranged Attacks In Close Combat", GlobalSearchTermType.Rule, null),
                ( "Melee Attacks", GlobalSearchTermType.Rule, null),
                ( "Grappling", GlobalSearchTermType.Rule, null),
                ( "Shoving", GlobalSearchTermType.Rule, null),
                ( "Cover", GlobalSearchTermType.Rule, null),
                ( "Damage And Healing", GlobalSearchTermType.Rule, null),
                ( "Hit Points", GlobalSearchTermType.Rule, null),
                ( "Damage Rolls", GlobalSearchTermType.Rule, null),
                ( "Critical Hits", GlobalSearchTermType.Rule, null),
                ( "Damage Types", GlobalSearchTermType.Rule, null),
                ( "Damage Resistance And Vulnerability", GlobalSearchTermType.Rule, null),
                ( "Healing", GlobalSearchTermType.Rule, null),
                ( "Dropping To 0 Hit Points", GlobalSearchTermType.Rule, null),
                ( "Instant Death", GlobalSearchTermType.Rule, null),
                ( "Falling Unconscious", GlobalSearchTermType.Rule, null),
                ( "Death Saving Throws", GlobalSearchTermType.Rule, null),
                ( "Stabilizing A Creature", GlobalSearchTermType.Rule, null),
                ( "Monsters And Death", GlobalSearchTermType.Rule, null),
                ( "Knocking A Creature Out", GlobalSearchTermType.Rule, null),
                ( "Temporary Hit Points", GlobalSearchTermType.Rule, null),
                ( "Mounted Combat", GlobalSearchTermType.Rule, null),
                ( "Underwater Combat", GlobalSearchTermType.Rule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> PHBChapterTenSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Power Level", GlobalSearchTermType.Rule, null),
                ( "Known Powers", GlobalSearchTermType.Rule, null),
                ( "Force and Tech Points", GlobalSearchTermType.Rule, null),
                ( "Casting A Power At A Higher Level", GlobalSearchTermType.Rule, null),
                ( "At-Will Powers", GlobalSearchTermType.Rule, null),
                ( "Casting A Power", GlobalSearchTermType.Rule, null),
                ( "Power Alignments", GlobalSearchTermType.Rule, null),
                ( "Casting Time", GlobalSearchTermType.Rule, null),
                ( "Range", GlobalSearchTermType.Rule, null),
                ( "Duration", GlobalSearchTermType.Rule, null),
                ( "Concentration", GlobalSearchTermType.Rule, null),
                ( "Targets", GlobalSearchTermType.Rule, null),
                ( "Areas Of Effect", GlobalSearchTermType.Rule, null),
                ( "Cone", GlobalSearchTermType.Rule, null),
                ( "Cylinder", GlobalSearchTermType.Rule, null),
                ( "Cube", GlobalSearchTermType.Rule, null),
                ( "Line", GlobalSearchTermType.Rule, null),
                ( "Sphere", GlobalSearchTermType.Rule, null),
                ( "Saving Throws", GlobalSearchTermType.Rule, null),
                ( "Attack Rolls", GlobalSearchTermType.Rule, null),
                ( "Combining Effects", GlobalSearchTermType.Rule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> PHBAppendixAConditionsSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Blinded", GlobalSearchTermType.Condition, null),
                ( "Charmed", GlobalSearchTermType.Condition, null),
                ( "Deafened", GlobalSearchTermType.Condition, null),
                ( "Exhaustion", GlobalSearchTermType.Condition, null),
                ( "Frightened", GlobalSearchTermType.Condition, null),
                ( "Grappled", GlobalSearchTermType.Condition, null),
                ( "Incapacitated", GlobalSearchTermType.Condition, null),
                ( "Invisible", GlobalSearchTermType.Condition, null),
                ( "Paralyzed", GlobalSearchTermType.Condition, null),
                ( "Petrified", GlobalSearchTermType.Condition, null),
                ( "Poisoned", GlobalSearchTermType.Condition, null),
                ( "Prone", GlobalSearchTermType.Condition, null),
                ( "Restrained", GlobalSearchTermType.Condition, null),
                ( "Shocked", GlobalSearchTermType.Condition, null),
                ( "Stunned", GlobalSearchTermType.Condition, null),
                ( "Unconscious", GlobalSearchTermType.Condition, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> PHBAppendixBVariantRulesSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Defense Rolls", GlobalSearchTermType.VariantRule, null),
                ( "Saving Throw Checks", GlobalSearchTermType.VariantRule, null),
                ( "Simplified Forcecasting", GlobalSearchTermType.VariantRule, null),
                ( "ASI And a Feat", GlobalSearchTermType.VariantRule, null),
                ( "Milestone Leveling", GlobalSearchTermType.VariantRule, null),
                ( "Hunted", GlobalSearchTermType.VariantRule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> SOTGChapterZeroSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Take To The Stars", GlobalSearchTermType.StarshipRule, null),
                ( "Using This Book", GlobalSearchTermType.StarshipRule, null),
                ( "Game Dice", GlobalSearchTermType.StarshipRule, null),
                ( "The D20", GlobalSearchTermType.StarshipRule, null),
                ( "Advantage And Disadvantage", GlobalSearchTermType.StarshipRule, null),
                ( "Specific Beats General", GlobalSearchTermType.StarshipRule, null),
                ( "Round Down", GlobalSearchTermType.StarshipRule, null),
                ( "Tier", GlobalSearchTermType.StarshipRule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> SOTGChapterOneSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "1. Choose A Deployment", GlobalSearchTermType.StarshipRule, null),
                ( "2. Acquire A Starship", GlobalSearchTermType.StarshipRule, null),
                ( "Tier", GlobalSearchTermType.StarshipRule, null),
                ( "Hit Points And Hit Dice", GlobalSearchTermType.StarshipRule, null),
                ( "3. Installing Modifications", GlobalSearchTermType.StarshipRule, null),
                ( "4. Choose Equipment", GlobalSearchTermType.StarshipRule, null),
                ( "Armor Class", GlobalSearchTermType.StarshipRule, null),
                ( "Weapons", GlobalSearchTermType.StarshipRule, null),
                ( "5. Come Together", GlobalSearchTermType.StarshipRule, null),
                ( "Beyond The Basics", GlobalSearchTermType.StarshipRule, null),
                ( "Feature And Hit Dice", GlobalSearchTermType.StarshipRule, null),
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> SOTGChapterThreeSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Acquiring A Ship", GlobalSearchTermType.StarshipRule, null),
                ( "Joining A Faction", GlobalSearchTermType.StarshipRule, null),
                ( "Shipjacking", GlobalSearchTermType.StarshipRule, null),
                ( "Purchasing", GlobalSearchTermType.StarshipRule, null),
                ( "Building A Ship", GlobalSearchTermType.StarshipRule, null),
                ( "Upgrading A Starship", GlobalSearchTermType.StarshipRule, null),
                ( "Cost Modifiers", GlobalSearchTermType.StarshipRule, null),
                ( "Upgrade Workforce", GlobalSearchTermType.StarshipRule, null),
                ( "Upgrade Time", GlobalSearchTermType.StarshipRule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> SOTGChapterFourSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Modifying A Starship", GlobalSearchTermType.StarshipRule, null),
                ( "Cost Modifiers", GlobalSearchTermType.StarshipRule, null),
                ( "Modification Workforce", GlobalSearchTermType.StarshipRule, null),
                ( "Purchasing", GlobalSearchTermType.StarshipRule, null),
                ( "Building A Ship", GlobalSearchTermType.StarshipRule, null),
                ( "Upgrading A Starship", GlobalSearchTermType.StarshipRule, null),
                ( "Upgrade Workforce", GlobalSearchTermType.StarshipRule, null),
                ( "Modification Time", GlobalSearchTermType.StarshipRule, null),
                ( "Modification Tier Requirements", GlobalSearchTermType.StarshipRule, null),
                ( "Prerequisites", GlobalSearchTermType.StarshipRule, null),
                ( "Changing Saving Throw Proficiency", GlobalSearchTermType.StarshipRule, null),
                ( "Removing Modifications", GlobalSearchTermType.StarshipRule, null),
                ( "Starship Tier Features", GlobalSearchTermType.StarshipRule, null),
                ( "Modification Slots At Tier 0", GlobalSearchTermType.StarshipRule, null),
                ( "Stock Modifications", GlobalSearchTermType.StarshipRule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> SOTGChapterFiveSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Cost Modifiers", GlobalSearchTermType.StarshipRule, null),
                ( "Equipment Workforce", GlobalSearchTermType.StarshipRule, null),
                ( "Installation Time", GlobalSearchTermType.StarshipRule, null),
                ( "Cargo", GlobalSearchTermType.StarshipRule, null),
                ( "Cargo Capacity", GlobalSearchTermType.StarshipRule, null),
                ( "Armor And Shields", GlobalSearchTermType.StarshipRule, null),
                ( "Armor", GlobalSearchTermType.StarshipRule, null),
                ( "Armor Class", GlobalSearchTermType.StarshipRule, null),
                ( "Shields", GlobalSearchTermType.StarshipRule, null),
                ( "Weapons", GlobalSearchTermType.StarshipRule, null),
                ( "Primary Weapons", GlobalSearchTermType.StarshipRule, null),
                ( "Secondary Weapons", GlobalSearchTermType.StarshipRule, null),
                ( "Tertiary Weapons", GlobalSearchTermType.StarshipRule, null),
                ( "Quaternary Weapons", GlobalSearchTermType.StarshipRule, null),
                ( "Attack Bonus", GlobalSearchTermType.StarshipWeaponProperty, null),
                ( "Ammunition", GlobalSearchTermType.StarshipWeaponProperty, null),
                ( "Attacks Per Round", GlobalSearchTermType.StarshipWeaponProperty, null),
                ( "Power", GlobalSearchTermType.StarshipWeaponProperty, null),
                ( "Reload", GlobalSearchTermType.StarshipWeaponProperty, null),
                ( "Weapons By Size", GlobalSearchTermType.StarshipRule, null),
                ( "Tiny To Medium", GlobalSearchTermType.StarshipRule, null),
                ( "Large To Gargantuan", GlobalSearchTermType.StarshipRule, null),
                ( "Ammunition", GlobalSearchTermType.StarshipRule, "ammunition 2"),
                ( "Tertiary Ammunition", GlobalSearchTermType.StarshipRule, null),
                ( "Quaternary Ammunition", GlobalSearchTermType.StarshipRule, null),
                ( "Hyperdrive", GlobalSearchTermType.StarshipRule, null),
                ( "Navcomputer", GlobalSearchTermType.StarshipRule, null),
                ( "Docking", GlobalSearchTermType.StarshipRule, null),
                ( "Docking Fees", GlobalSearchTermType.StarshipRule, null),
                ( "Long Term Storage", GlobalSearchTermType.StarshipRule, null),
                ( "Refueling And Restocking", GlobalSearchTermType.StarshipRule, null),
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> SOTGChapterSixSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Multispecializing", GlobalSearchTermType.StarshipRule, null),
                ( "Gunning Styles", GlobalSearchTermType.StarshipRule, null),
                ( "Gunning Masteries", GlobalSearchTermType.StarshipRule, null),
                ( "Ventures", GlobalSearchTermType.StarshipRule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> SOTGChapterSevenSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Ability Scores And Modifiers", GlobalSearchTermType.StarshipRule, null),
                ( "Advantage And Disadvantage", GlobalSearchTermType.StarshipRule, null),
                ( "Proficiency Bonus", GlobalSearchTermType.StarshipRule, null),
                ( "Expertise", GlobalSearchTermType.StarshipRule, null),
                ( "Ability Checks", GlobalSearchTermType.StarshipRule, null),
                ( "Contests", GlobalSearchTermType.StarshipRule, null),
                ( "Strength", GlobalSearchTermType.StarshipRule, null),
                ( "Strength Checks", GlobalSearchTermType.StarshipRule, null),
                ( "Strength Saving Throws", GlobalSearchTermType.StarshipRule, null),
                ( "Flying Speed", GlobalSearchTermType.StarshipRule, null),
                ( "Weapon Hardpoints", GlobalSearchTermType.StarshipRule, null),
                ( "Dexterity", GlobalSearchTermType.StarshipRule, null),
                ( "Dexterity Checks", GlobalSearchTermType.StarshipRule, null),
                ( "Dexterity Saving Throws", GlobalSearchTermType.StarshipRule, null),
                ( "Attack And Damage Rolls", GlobalSearchTermType.StarshipRule, null),
                ( "Turning Speed", GlobalSearchTermType.StarshipRule, null),
                ( "Constitution", GlobalSearchTermType.StarshipRule, null),
                ( "Constitution Checks", GlobalSearchTermType.StarshipRule, null),
                ( "Constitution Saving Throws", GlobalSearchTermType.StarshipRule, null),
                ( "Hit Points", GlobalSearchTermType.StarshipRule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> SOTGChapterEightSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Ship Registration", GlobalSearchTermType.StarshipRule, null),
                ( "Transponder Codes", GlobalSearchTermType.StarshipRule, null),
                ( "Communications", GlobalSearchTermType.StarshipRule, null),
                ( "Metosp (Message to Spacers)", GlobalSearchTermType.StarshipRule, null),
                ( "Planetary Information Channels", GlobalSearchTermType.StarshipRule, null),
                ( "Time", GlobalSearchTermType.StarshipRule, null),
                ( "Movement and Travel", GlobalSearchTermType.StarshipRule, null),
                ( "Travel In Realspace", GlobalSearchTermType.StarshipRule, null),
                ( "Flying Speed", GlobalSearchTermType.StarshipRule, null),
                ( "Turning Speed", GlobalSearchTermType.StarshipRule, null),
                ( "Travel Pace", GlobalSearchTermType.StarshipRule, null),
                ( "Difficult Terrain", GlobalSearchTermType.StarshipRule, null),
                ( "Activity While Traveling", GlobalSearchTermType.StarshipRule, null),
                ( "Deployment Order", GlobalSearchTermType.StarshipRule, null),
                ( "Stealth", GlobalSearchTermType.StarshipRule, null),
                ( "Noticing Threats", GlobalSearchTermType.StarshipRule, null),
                ( "Other Activities", GlobalSearchTermType.StarshipRule, null),
                ( "Travel In Hyperspace", GlobalSearchTermType.StarshipRule, null),
                ( "Travel Pace", GlobalSearchTermType.StarshipRule, "travel pace 2"),
                ( "Detecting Hyperspace Travel", GlobalSearchTermType.StarshipRule, null),
                ( "Astrogation", GlobalSearchTermType.StarshipRule, null),
                ( "Hyperspace Hazards", GlobalSearchTermType.StarshipRule, null),
                ( "Landing Gear", GlobalSearchTermType.StarshipRule, null),
                ( "Movement In Zero Gravity", GlobalSearchTermType.StarshipRule, null),
                ( "Crew Capacity", GlobalSearchTermType.StarshipRule, null),
                ( "Deployed", GlobalSearchTermType.StarshipRule, null),
                ( "The Environment", GlobalSearchTermType.StarshipRule, null),
                ( "Vision And Light", GlobalSearchTermType.StarshipRule, null),
                ( "Blindsight", GlobalSearchTermType.StarshipRule, null),
                ( "Truesight", GlobalSearchTermType.StarshipRule, null),
                ( "Repairs And Maintenance", GlobalSearchTermType.StarshipRule, null),
                ( "Repairs", GlobalSearchTermType.StarshipRule, null),
                ( "Maintenance", GlobalSearchTermType.StarshipRule, null),
                ( "Repairing the \"Used\" Condition", GlobalSearchTermType.StarshipRule, null),
                ( "Primary System Failure", GlobalSearchTermType.StarshipRule, null),
                ( "Repairing Primary Systems", GlobalSearchTermType.StarshipRule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> SOTGChapterNineSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "The Order Of Combat", GlobalSearchTermType.StarshipRule, null),
                ( "Surprise", GlobalSearchTermType.StarshipRule, null),
                ( "Your Role On Your Turn", GlobalSearchTermType.StarshipRule, null),
                ( "Actions", GlobalSearchTermType.StarshipRule, null),
                ( "Bonus Actions", GlobalSearchTermType.StarshipRule, null),
                ( "Other Activity On Your Turn", GlobalSearchTermType.StarshipRule, null),
                ( "Reactions", GlobalSearchTermType.StarshipRule, null),
                ( "Movement And Position", GlobalSearchTermType.StarshipRule, null),
                ( "Flying Speed", GlobalSearchTermType.StarshipRule, null),
                ( "Turning Speed", GlobalSearchTermType.StarshipRule, null),
                ( "Variant: Playing On A Grid", GlobalSearchTermType.StarshipRule, null),
                ( "Breaking Up Your Move", GlobalSearchTermType.StarshipRule, null),
                ( "Moving Between Attacks", GlobalSearchTermType.StarshipRule, null),
                ( "Difficult Terrain", GlobalSearchTermType.StarshipRule, null),
                ( "Moving Around Other Ships", GlobalSearchTermType.StarshipRule, null),
                ( "Ship Size", GlobalSearchTermType.StarshipRule, null),
                ( "Space", GlobalSearchTermType.StarshipRule, null),
                ( "Actions In Combat", GlobalSearchTermType.StarshipRule, null),
                ( "Attack", GlobalSearchTermType.StarshipRule, null),
                ( "Cast A Power", GlobalSearchTermType.StarshipRule, null),
                ( "Direct", GlobalSearchTermType.StarshipRule, null),
                ( "Evade", GlobalSearchTermType.StarshipRule, null),
                ( "Hide", GlobalSearchTermType.StarshipRule, null),
                ( "Interfere", GlobalSearchTermType.StarshipRule, null),
                ( "Patch", GlobalSearchTermType.StarshipRule, null),
                ( "Ram", GlobalSearchTermType.StarshipRule, null),
                ( "Ready", GlobalSearchTermType.StarshipRule, null),
                ( "Search", GlobalSearchTermType.StarshipRule, null),
                ( "Use An Object", GlobalSearchTermType.StarshipRule, null),
                ( "Making An Attack", GlobalSearchTermType.StarshipRule, null),
                ( "Attack Rolls", GlobalSearchTermType.StarshipRule, null),
                ( "Rolling 1 Or 20", GlobalSearchTermType.StarshipRule, null),
                ( "Range", GlobalSearchTermType.StarshipRule, null),
                ( "Ranged Attacks In Close Combat", GlobalSearchTermType.StarshipRule, null),
                ( "Firing Arc", GlobalSearchTermType.StarshipRule, null),
                ( "Limited Firing Arc", GlobalSearchTermType.StarshipRule, null),
                ( "Unlimited Firing Arc", GlobalSearchTermType.StarshipRule, null),
                ( "Saving Throws", GlobalSearchTermType.StarshipRule, null),
                ( "Damage Rolls", GlobalSearchTermType.StarshipRule, null),
                ( "Critical Hits", GlobalSearchTermType.StarshipRule, null),
                ( "Damage Types", GlobalSearchTermType.StarshipRule, null),
                ( "Damage Resistance And Vulnerability", GlobalSearchTermType.StarshipRule, null),
                ( "Cover", GlobalSearchTermType.StarshipRule, null),
                ( "Damage And Repairs", GlobalSearchTermType.StarshipRule, null),
                ( "Hit Points", GlobalSearchTermType.StarshipRule, null),
                ( "Shield Points", GlobalSearchTermType.StarshipRule, null),
                ( "Repairs", GlobalSearchTermType.StarshipRule, null),
                ( "Dropping To 0 Hit Points", GlobalSearchTermType.StarshipRule, null),
                ( "Describing The Effects Of Damage", GlobalSearchTermType.StarshipRule, null),
                ( "Instantly Destroyed", GlobalSearchTermType.StarshipRule, null),
                ( "Destruction Saving Throws", GlobalSearchTermType.StarshipRule, null),
                ( "Stabilizing A Ship", GlobalSearchTermType.StarshipRule, null),
                ( "Ships And Destruction", GlobalSearchTermType.StarshipRule, null),
                ( "Disabling A Ship", GlobalSearchTermType.StarshipRule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> SOTGChapterTenSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Escort Missions", GlobalSearchTermType.StarshipRule, null),
                ( "Blockade Run", GlobalSearchTermType.StarshipRule, null),
                ( "Base Assault", GlobalSearchTermType.StarshipRule, null),
                ( "Retrieval Mission", GlobalSearchTermType.StarshipRule, null),
                ( "Build Interesting Battlefields", GlobalSearchTermType.StarshipRule, null),
                ( "Asteroids, Debris, And Enclosed Terrain", GlobalSearchTermType.StarshipRule, null),
                ( "Damaging Environment", GlobalSearchTermType.StarshipRule, null),
                ( "Corrosive Gases", GlobalSearchTermType.StarshipRule, null),
                ( "Dust Clouds", GlobalSearchTermType.StarshipRule, null),
                ( "Ionic Discharges", GlobalSearchTermType.StarshipRule, null),
                ( "Radiation", GlobalSearchTermType.StarshipRule, null),
                ( "Create Exciting Scenarios With Complications", GlobalSearchTermType.StarshipRule, null),
                ( "Creating An Encounter", GlobalSearchTermType.StarshipRule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> SOTGAppendixAConditionsSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Blinded", GlobalSearchTermType.StarshipCondition, null),
                ( "Disabled", GlobalSearchTermType.StarshipCondition, null),
                ( "Ionized", GlobalSearchTermType.StarshipCondition, null),
                ( "Invisible", GlobalSearchTermType.StarshipCondition, null),
                ( "Shocked", GlobalSearchTermType.StarshipCondition, null),
                ( "Stalled", GlobalSearchTermType.StarshipCondition, null),
                ( "Stunned", GlobalSearchTermType.StarshipCondition, null),
                ( "System Damage", GlobalSearchTermType.StarshipCondition, null),
                ( "Tractored", GlobalSearchTermType.StarshipCondition, null),
                ( "Used", GlobalSearchTermType.StarshipCondition, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> WHChapterOneSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Building the Mandalorians", GlobalSearchTermType.Rule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> WHChapterZeroSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Introduction", GlobalSearchTermType.Rule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> WHChapterTwoSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Rivals", GlobalSearchTermType.Rule, null),
                ( "Bounty Hunting", GlobalSearchTermType.Rule, null),
                ( "Buying Enhanced Items", GlobalSearchTermType.Rule, null),
                ( "Carousing", GlobalSearchTermType.Rule, null),
                ( "Crafting", GlobalSearchTermType.Rule, null),
                ( "Reverse Engineering", GlobalSearchTermType.Rule, null),
                ( "Crime", GlobalSearchTermType.Rule, null),
                ( "Gambling", GlobalSearchTermType.Rule, null),
                ( "Mercenary Contracting", GlobalSearchTermType.Rule, null),
                ( "Pit Fighting", GlobalSearchTermType.Rule, null),
                ( "Racing", GlobalSearchTermType.Rule, null),
                ( "Research", GlobalSearchTermType.Rule, null),
                ( "Selling Enhanced Items", GlobalSearchTermType.Rule, null),
                ( "Work", GlobalSearchTermType.Rule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> WHChapterThreeSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Joining a Faction", GlobalSearchTermType.Rule, null),
                ( "Faction Ranks", GlobalSearchTermType.Rule, null),
                ( "Membership in Multiple Factions", GlobalSearchTermType.Rule, null),
                ( "Factions", GlobalSearchTermType.Rule, null),
                ( "Generating a Faction", GlobalSearchTermType.Rule, null),
                ( "Membership Ranks CR and Renown", GlobalSearchTermType.Table, null),
                ( "Faction Benefits", GlobalSearchTermType.Table, null),
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> WHChapterFourSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Strength", GlobalSearchTermType.Rule, null),
                ( "Dexterity", GlobalSearchTermType.Rule, null),
                ( "Constitution", GlobalSearchTermType.Rule, null),
                ( "Intelligence", GlobalSearchTermType.Rule, null),
                ( "Wisdom", GlobalSearchTermType.Rule, null),
                ( "Charisma", GlobalSearchTermType.Rule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> WHChapterFiveSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Rarity", GlobalSearchTermType.Rule, null),
                ( "Enhanced Item Rarity and Identification", GlobalSearchTermType.Table, null),
                ( "Identifying Enhanced Items", GlobalSearchTermType.Rule, null),
                ( "Attunement", GlobalSearchTermType.Rule, null),
                ( "Cursed Items", GlobalSearchTermType.Rule, null),
                ( "Enhanced Item Categories", GlobalSearchTermType.Rule, null),
                ( "Adventuring Gear", GlobalSearchTermType.Rule, null),
                ( "Armor", GlobalSearchTermType.Rule, null),
                ( "Consumables", GlobalSearchTermType.Rule, null),
                ( "Cybernetic Augmentations", GlobalSearchTermType.Rule, null),
                ( "Droid Customizations", GlobalSearchTermType.Rule, null),
                ( "Item Modifications", GlobalSearchTermType.Rule, null),
                ( "Shields", GlobalSearchTermType.Rule, null),
                ( "Valuables", GlobalSearchTermType.Rule, null),
                ( "Weapons", GlobalSearchTermType.Rule, null),
                ( "Focuses", GlobalSearchTermType.Rule, null),
                ( "Awarding Enhanced Items", GlobalSearchTermType.Rule, null),
                ( "Minor Enhanced Items by Rarity", GlobalSearchTermType.Table, null),
                ( "Major Enhanced Items by Rarity", GlobalSearchTermType.Table, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> WHChapterSixSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Modifying Equipment", GlobalSearchTermType.Rule, null),
                ( "Modifiable Item Chassis", GlobalSearchTermType.Rule, null),
                ( "Installing Modifications", GlobalSearchTermType.Rule, null),
                ( "Modification Slots by Rarity", GlobalSearchTermType.Table, null),
                ( "Installation and Removal DC by Rarity", GlobalSearchTermType.Table, null),
                ( "Available Modification Slots", GlobalSearchTermType.Table, null),
                ( "Removing Modifications", GlobalSearchTermType.Rule, null),
                ( "Modifications by Item Type", GlobalSearchTermType.Rule, null),
                ( "Blasters", GlobalSearchTermType.Rule, null),
                ( "Vibroweapons", GlobalSearchTermType.Rule, null),
                ( "Lightweapons", GlobalSearchTermType.Rule, null),
                ( "Forcecasting Color Crystal Modifier", GlobalSearchTermType.Table, null),
                ( "Focus Generators", GlobalSearchTermType.Rule, null),
                ( "Armor and Shields", GlobalSearchTermType.Rule, null),
                ( "Augments", GlobalSearchTermType.Rule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> WHChapterSevenSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Augmenting Creatures", GlobalSearchTermType.Rule, null),
                ( "Creature Size Time Modifier", GlobalSearchTermType.Table, null),
                ( "Cybernetic Augmentation Side Effects", GlobalSearchTermType.Table, null),
                ( "Installing Augmentations", GlobalSearchTermType.Rule, null),
                ( "Installation and Removal DC by Rarity", GlobalSearchTermType.Table, null),
                ( "Removing Augmentations", GlobalSearchTermType.Rule, null),
                ( "Enhancements", GlobalSearchTermType.Rule, null),
                ( "Replacements", GlobalSearchTermType.Rule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> WHChapterEightSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Customizing Droids", GlobalSearchTermType.Rule, null),
                ( "Droid Size Time Modifier", GlobalSearchTermType.Table, null),
                ( "Total Slots Upgrade Cost", GlobalSearchTermType.Table, null),
                ( "Installation and Removal DC by Rarity", GlobalSearchTermType.Table, null),
                ( "Removing Customizations", GlobalSearchTermType.Rule, null),
                ( "Parts", GlobalSearchTermType.Rule, null),
                ( "Protocols", GlobalSearchTermType.Rule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> WHChapterNineSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Tool Descriptions", GlobalSearchTermType.Rule, null),
                ( "Item Specific Tools", GlobalSearchTermType.Table, null),
                ( "Armormech's Tools", GlobalSearchTermType.Rule, null),
                ( "Armstech's Tools", GlobalSearchTermType.Rule, null),
                ( "Artificer's Tools", GlobalSearchTermType.Rule, null),
                ( "Biochemist's Kit", GlobalSearchTermType.Rule, null),
                ( "Brewer's Tools", GlobalSearchTermType.Rule, null),
                ( "Chef's Kit", GlobalSearchTermType.Rule, null),
                ( "Cybertech's Tools", GlobalSearchTermType.Rule, null),
                ( "Disguise Kit", GlobalSearchTermType.Rule, null),
                ( "Forgery Kit", GlobalSearchTermType.Rule, null),
                ( "Gaming Set", GlobalSearchTermType.Rule, null),
                ( "Herbalism Kit", GlobalSearchTermType.Rule, null),
                ( "Jeweler's Tools", GlobalSearchTermType.Rule, null),
                ( "Mason's Tools", GlobalSearchTermType.Rule, null),
                ( "Mechanic's Kit", GlobalSearchTermType.Rule, null),
                ( "Musical Instruments", GlobalSearchTermType.Rule, null),
                ( "Painter's Tools", GlobalSearchTermType.Rule, null),
                ( "Poisoner's Kit", GlobalSearchTermType.Rule, null),
                ( "Security Kit", GlobalSearchTermType.Rule, null),
                ( "Slicer's Kit", GlobalSearchTermType.Rule, null),
                ( "Surveyor's Tools", GlobalSearchTermType.Rule, null),
                ( "Synthweaver's Tools", GlobalSearchTermType.Rule, null),
                ( "Tinker's Tools", GlobalSearchTermType.Rule, null)
            };
    }
}
