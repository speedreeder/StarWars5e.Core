using System;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Lookup
{
    public class CasterRatioLU : TableEntity
    {
        public string Name { get; set; }
        public PowerType CasterTypeEnum { get; set; }
        public string CasterType
        {
            get => CasterTypeEnum.ToString();
            set => CasterTypeEnum = Enum.Parse<PowerType>(value);
        }
        public double Ratio { get; set; }
        public CasterRatioType CasterRatioTypeEnum { get; set; }
        public string CasterRatioType
        {
            get => CasterRatioTypeEnum.ToString();
            set => CasterRatioTypeEnum = Enum.Parse<CasterRatioType>(value);
        }
    }
}
