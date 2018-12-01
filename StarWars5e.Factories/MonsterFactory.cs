using StarWars5e.Models.Monster;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Factories
{
    /// <summary>
    /// Factory to convert between monster VM and Entity Object
    /// </summary>
    public class MonsterFactory
    {
        /// <summary>
        /// Convert a View model to an entity object
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Monster ConvertFromViewModel(MonsterViewModel input)
        {
            var model = new Monster();
            model.Name = input.Basics.Name;
            model.Size = input.Basics.Size;
            model.Actions = input.Actions;
            model.HitPoints = input.Basics.HitPoints;
            model.Alignment = input.Basics.Alignment;
            model.ArmorClass = input.Basics.ArmorClass;
            model.ArmorType = input.Basics.ArmorType;
            model.Challenge = input.Challenge;
            model.ExperiencePoints = input.ExperiencePoints;

            model.Charisma = input.Stats.Charisma;
            model.CharismaModifier = input.Stats.CharismaModifier;
            model.Dexterity = input.Stats.Dexterity;
            model.DexterityModifier = input.Stats.DexterityModifier;
            model.Intelligence = input.Stats.Intelligence;
            model.IntelligenceModifier = input.Stats.IntelligenceModifier;
            model.Constitution = input.Stats.Constitution;
            model.ConstitutionModifer = input.Stats.ConstitutionModifer;
            model.Strength = input.Stats.Strength;
            model.StrengthModifier = input.Stats.StrengthModifier;
            model.Wisdom = input.Stats.Wisdom;
            model.WisdomModifier = input.Stats.WisdomModifier;

            model.ClimbingSpeed = input.Speed.Climbing;
            model.FlyingSpeed = input.Speed.Flying;
            model.SwimmingSpeed = input.Speed.Swimming;
            model.WalkingSpeed = input.Speed.Walking;

            model.ConditionImmunities = input.Resistances.ConditionImmunities;
            model.DamageResistances = input.Resistances.DamageResistances;
            model.DamageVulnerabilities = input.Resistances.DamageVulnerabilities;
            model.DamageImmunities = input.Resistances.DamageImmunities;

            model.Features = input.Features;
            model.Languages = input.Languages;
            model.LegendaryActions = input.LegendaryActions;
            model.LegendaryActionsDescription = input.LegendaryActionsDescription;
            model.Reactions = input.Reactions;
            model.Skills = input.Skills;
            model.Senses = input.Senses;

            return model;
        }

        /// <summary>
        /// Convert a Monster entity object to a ViewModel
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MonsterViewModel ConvertFromViewModel(Monster input)
        {
            var vm = new MonsterViewModel();
            vm.Basics.Name = input.Name;
            vm.Basics.Size = input.Size;
            vm.Actions = input.Actions;
            vm.Basics.HitPoints = input.HitPoints;
            vm.Basics.Alignment = input.Alignment;
            vm.Basics.ArmorClass = input.ArmorClass;
            vm.Basics.ArmorType = input.ArmorType;
            vm.Challenge = input.Challenge;
            vm.ExperiencePoints = input.ExperiencePoints;

            vm.Stats.Charisma = input.Charisma;
            vm.Stats.CharismaModifier = input.CharismaModifier;
            vm.Stats.Dexterity = input.Dexterity;
            vm.Stats.DexterityModifier = input.DexterityModifier;
            vm.Stats.Intelligence = input.Intelligence;
            vm.Stats.IntelligenceModifier = input.IntelligenceModifier;
            vm.Stats.Constitution = input.Constitution;
            vm.Stats.ConstitutionModifer = input.ConstitutionModifer;
            vm.Stats.Strength = input.Strength;
            vm.Stats.StrengthModifier = input.StrengthModifier;
            vm.Stats.Wisdom = input.Wisdom;
            vm.Stats.WisdomModifier = input.WisdomModifier;

            vm.Speed.Climbing = input.ClimbingSpeed;
            vm.Speed.Flying = input.FlyingSpeed;
            vm.Speed.Swimming = input.SwimmingSpeed;
            vm.Speed.Walking = input.WalkingSpeed;

            vm.Resistances.ConditionImmunities = input.ConditionImmunities;
            vm.Resistances.DamageResistances = input.DamageResistances;
            vm.Resistances.DamageVulnerabilities = input.DamageVulnerabilities;
            vm.Resistances.DamageImmunities = input.DamageImmunities;

            vm.Features = input.Features;
            vm.Languages = input.Languages;
            vm.LegendaryActions = input.LegendaryActions;
            vm.LegendaryActionsDescription = input.LegendaryActionsDescription;
            vm.Reactions = input.Reactions;
            vm.Skills = input.Skills;
            vm.Senses = input.Senses;

            return vm;
        }
    }
}
