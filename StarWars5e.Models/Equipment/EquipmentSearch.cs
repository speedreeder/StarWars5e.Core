using System;
using System.Collections.Generic;
using System.Text;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Equipment
{
    public class EquipmentSearch
    {
        public string Name { get; set; }
        public EquipmentCategory? EquipmentCategory { get; set; }
    }
}
