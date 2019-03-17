using System;
using System.Collections.Generic;
using StarWars5e.Models.ViewModels;

namespace StarWars.Powers.Parser.Processors
{
    /// <summary>
    /// This builds out the potency description
    /// </summary>
    public class PotencyProcessor : IPowerProcessor
    {
        public PowerSection Section { get; } = PowerSection.Potency;
        public PowerViewModel Process(List<string> sectionContent, PowerViewModel vm)
        {
            if (sectionContent.Count > 1)
            {
                throw new ArgumentException("Potency should not be over one line");
            }

            var workingOnTrimming = sectionContent[0].Trim('*');
            var amForce = "Force Potency.***";
            var amTech = "Overcharge Tech.***";
            if (workingOnTrimming.StartsWith(amForce))
            {
                workingOnTrimming = workingOnTrimming.Substring(amForce.Length);
            } else if (workingOnTrimming.StartsWith(amTech))
            {
                workingOnTrimming = workingOnTrimming.Substring(amTech.Length);
            }
            else
            {
                ;
            }
            var trimmed = workingOnTrimming.Trim();
            vm.PotencyPower = trimmed;
            return vm;
        }
    }
}