using System;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Factories
{
    /// <summary>
    /// Convert between internal and external versions of powers
    /// </summary>
    public class PowerFactory
    {
        /// <summary>
        /// Convert an API model to a View Model
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static PowerViewModel ConvertoViewModel(Power input)
        {
            var vm = new PowerViewModel();
            vm.Name = input.Name;
            vm.PowerType = input.PowerType;
            vm.Description = input.Description;
            vm.Level = input.Level;
            vm.Prerequisite = input.Prerequisite;
            vm.CastingLength = input.CastingLength;
            vm.ReactionLimit = input.ReactionLimit;
            vm.Range = input.Range;
            vm.RangeType = (RangeType)input.RangeType;
            vm.PowerShape = (Shape) input.PowerShape;
            vm.ShapeSize = input.ShapeSize;
            vm.ShapeSizeType = input.ShapeSizeType;
            vm.RequiresConcentration = input.RequiresConcentration;
            vm.DurationLengthModifier = (Duration) input.DurationLengthModifier;
            vm.DurationLength = input.DurationLength;
            vm.PotencyPower = input.PotencyPower;
            vm.PowerAlignment = (Alignment) input.PowerAlignment;
            vm.Tags = input.Tags;
            return vm;
        }

        /// <summary>
        /// Covnert from a View model into an API model. This will preform validation as well
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Power ConvertToApiModel(PowerViewModel input)
        {
            if (input.PowerType != "force" && input.PowerType != "tech")
            {
                throw new ArgumentException("Unable to create powers that are not aligned to tech or force");
            }
            var model = new Power();
            model.Name = input.Name;
            model.PowerType = input.PowerType;
            model.Description = input.Description;
            model.Level = input.Level;
            model.Prerequisite = input.Prerequisite;
            model.CastingLength = input.CastingLength;
            model.ReactionLimit = input.ReactionLimit;
            model.Range = input.Range;
            model.RangeType = (int) input.RangeType;
            model.PowerShape = (int) input.PowerShape;
            model.ShapeSize = input.ShapeSize;
            model.ShapeSizeType = input.ShapeSizeType;
            model.RequiresConcentration = input.RequiresConcentration;
            model.DurationLengthModifier = (int) input.DurationLengthModifier;
            model.DurationLength = input.DurationLength;
            model.PotencyPower = input.PotencyPower;
            model.PowerAlignment = (int) input.PowerAlignment;
            model.Tags = input.Tags;
            return model;
        }
    }
}