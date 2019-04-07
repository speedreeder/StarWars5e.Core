using System;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models
{
    public class Power : BaseEntity
    {
        public string Name { get; set; }
        public PowerType PowerTypeEnum { get; set; }
        public string PowerType
        {
            get => PowerTypeEnum.ToString();
            set => PowerTypeEnum = Enum.Parse<PowerType>(value);
        }
        public string Prerequisite { get; set; }
        public int Level { get; set; }
        public CastingPeriod CastingPeriodEnum { get; set; }
        public string CastingPeriod
        {
            get => CastingPeriodEnum.ToString();
            set => CastingPeriodEnum = Enum.Parse<CastingPeriod>(value);
        }
        public string CastingPeriodText { get; set; }
        public string Range { get; set; }
        public string Duration { get; set; }
        public bool Concentration { get; set; }
        public ForceAlignment ForceAlignmentEnum { get; set; }
        public string ForceAlignment
        {
            get => ForceAlignmentEnum.ToString();
            set => ForceAlignmentEnum = Enum.Parse<ForceAlignment>(value);
        }
        public string Description { get; set; }
        public string HigherLevelDescription { get; set; }
    }
}
