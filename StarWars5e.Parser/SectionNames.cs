using System.Collections.Generic;
using StarWars5e.Models.Enums;

namespace StarWars5e.Parser
{
    public static class SectionNames
    {
        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> PHBChapterOneSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Chapter 1: Step-By-Step Characters", GlobalSearchTermType.Rule, null ),
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
                ( "Chapter 2: Species", GlobalSearchTermType.Rule, null ),
                ( "Choosing A Species", GlobalSearchTermType.Rule, null),
                ( "Ability Score Increase", GlobalSearchTermType.Rule, null),
                ( "Age", GlobalSearchTermType.Rule, null),
                ( "Alignment", GlobalSearchTermType.Rule, null),
                ( "Size", GlobalSearchTermType.Rule, null),
                ( "Speed", GlobalSearchTermType.Rule, null),
                ( "Languages", GlobalSearchTermType.Rule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> PHBChapterThreeSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Chapter 3: Classes", GlobalSearchTermType.Rule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> PHBChapterFourSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Chapter 4: Personality And Backgrounds", GlobalSearchTermType.Rule, null),
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
                ( "Chapter 5: Equipment", GlobalSearchTermType.Rule, null),
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
                ( "Chapter 6: Customization Options", GlobalSearchTermType.Rule, null),
                ( "Multiclassing", GlobalSearchTermType.Rule, null),
                ( "Fighting Styles", GlobalSearchTermType.Rule, null)
            };

        public static List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)> PHBChapterSevenSections { get; } =
            new List<(string name, GlobalSearchTermType globalSearchTermType, string pathOverride)>
            {
                ( "Chapter 7: Using Ability Scores", GlobalSearchTermType.Rule, null),
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
                ( "Chapter 8: Adventuring", GlobalSearchTermType.Rule, null),
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
                ( "Chapter 9: Combat", GlobalSearchTermType.Rule, null),
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
                ( "Chapter 10: Force- And Tech-Casting", GlobalSearchTermType.Rule, null),
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
    }
}
