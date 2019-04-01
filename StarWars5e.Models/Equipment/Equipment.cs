using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Equipment
{
    public class Equipment : BaseEntity
    {
       
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }      
        public int Weight { get; set; }

        public EquipmentCategory EquipmentCategoryEnum { get; set; }
        public string EquipmentCategory
        {
            get => EquipmentCategoryEnum.ToString();
            set => EquipmentCategoryEnum = Enum.Parse<EquipmentCategory>(value);
        }
    }
}
