using System;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Monster
{
    public class MonsterBehavior
    {
        public string Name { get; set; }
        public MonsterBehaviorType MonsterBehaviorTypeEnum { get; set;}
        public string MonsterBehaviorType
        {
            get => MonsterBehaviorTypeEnum.ToString();
            set => MonsterBehaviorTypeEnum = Enum.Parse<MonsterBehaviorType>(value);
        }
        public string Description { get; set; }
        public string DescriptionWithLinks { get; set; }
        public AttackType AttackTypeEnum { get; set; }
        public string AttackType {
            get => AttackTypeEnum.ToString();
            set => AttackTypeEnum = Enum.Parse<AttackType>(value);
        }
        public string Restrictions { get; set; }
        public int AttackBonus { get; set; }
        public string Range { get; set; }
        public string NumberOfTargets { get; set; }
        public string Damage { get; set; }
        public string DamageRoll { get; set; }
        public DamageType DamageTypeEnum { get; set; }
        public string DamageType {
            get => DamageTypeEnum.ToString();
            set => DamageTypeEnum = Enum.Parse<DamageType>(value);
        }
    }
}
