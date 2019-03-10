using System;
using StarWars5e.Models.Equipment;
using StarWars5e.Models.ViewModels;

namespace StarWars5e.Factories
{
    public static class EquipmentFactory
    {

        public static Armor ConvertArmorViewModel(ArmorViewModel input)
        {
            return new Armor
            {
                PartitionKey = "armor",
                RowKey = string.IsNullOrEmpty(input.Id) ? Guid.NewGuid().ToString() : input.Id,
                ArmorClass = input.ArmorClass,
                ArmorClassification = input.ArmorClassification,
                Category =input.Category,
                Cost = input.Cost,
                Description =  input.Description,
                DexterityMax = input.DexterityMax,
                IsShield = input.IsShield,
                Name = input.Name,
                Rarity = input.Rarity,
                StealthImpaired = input.StealthImpaired,
                StrengthRequirement = input.StrengthRequirement,
                Weight = input.Weight
            };
        }

        public static ArmorViewModel ConvertArmorEntity(Armor input)
        {
            return new ArmorViewModel
            {
                ArmorClass = input.ArmorClass,
                ArmorClassification = input.ArmorClassification,
                Category =input.Category,
                Cost = input.Cost,
                Description =  input.Description,
                DexterityMax = input.DexterityMax,
                IsShield = input.IsShield,
                Name = input.Name,
                Rarity = input.Rarity,
                StealthImpaired = input.StealthImpaired,
                StrengthRequirement = input.StrengthRequirement,
                Weight = input.Weight
            };
        }

        public static Weapon ConvertWeaponViewModel(WeaponViewModel input)
        {
            return new Weapon
            {
                PartitionKey = "weapon",
                RowKey = string.IsNullOrEmpty(input.Id) ? Guid.NewGuid().ToString() : input.Id,
                Category = input.Category,
                Cost = input.Cost,
                Description = input.Description,
                Name = input.Name,
                Rarity = input.Rarity,
                StrengthRequirement = input.StrengthRequirement,
                Weight = input.Weight,
                BurstRoundsUsed = input.BurstRoundsUsed,
                ClipSize = input.ClipSize,
                DamageType = input.DamageType,
                DiceType = input.DiceType,
                IsBurst = input.IsBurst,
                IsDouble = input.IsDouble,
                IsFinesse = input.IsFinesse,
                IsHeavy = input.IsHeavy,
                IsHidden = input.IsHidden,
                IsLight = input.IsLight,
                IsLuminous = input.IsLuminous,
                IsReach = input.IsReach,
                IsThrown = input.IsThrown,
                IsVersatile = input.IsVersatile,
                LongRange = input.LongRange,
                NumberOfDie = input.NumberOfDie,
                RequiredHands = input.RequiredHands,
                ShortRange = input.ShortRange,
                VersatileDiceType = input.VersatileDiceType,
                VersatileNumberOfDie = input.VersatileNumberOfDie,
                WeaponClassification = input.WeaponClassification,
                WeaponProperties = input.WeaponProperties,
            };
        }

        public static WeaponViewModel ConvertWeaponEntity(Weapon input)
        {
            return new WeaponViewModel
            {
                Category = input.Category,
                Cost = input.Cost,
                Description = input.Description,
                Name = input.Name,
                Rarity = input.Rarity,
                StrengthRequirement = input.StrengthRequirement,
                Weight = input.Weight,
                BurstRoundsUsed = input.BurstRoundsUsed,
                ClipSize = input.ClipSize,
                DamageType = input.DamageType,
                DiceType = input.DiceType,
                IsBurst = input.IsBurst,
                IsDouble = input.IsDouble,
                IsFinesse = input.IsFinesse,
                IsHeavy = input.IsHeavy,
                IsHidden = input.IsHidden,
                IsLight = input.IsLight,
                IsLuminous = input.IsLuminous,
                IsReach = input.IsReach,
                IsThrown = input.IsThrown,
                IsVersatile = input.IsVersatile,
                LongRange = input.LongRange,
                NumberOfDie = input.NumberOfDie,
                RequiredHands = input.RequiredHands,
                ShortRange = input.ShortRange,
                VersatileDiceType = input.VersatileDiceType,
                VersatileNumberOfDie = input.VersatileNumberOfDie,
                WeaponClassification = input.WeaponClassification,
                WeaponProperties = input.WeaponProperties,
            };
        }

        public static MountViewModel ConvertMountEntity(Mount input)
        {
            return new MountViewModel
            {
                Category = input.Category,
                Cost = input.Cost,
                Description = input.Description,
                Name = input.Name,
                Rarity = input.Rarity,
                Weight = input.Weight,
                Speed = input.Speed,
                CarryingCapacity = input.CarryingCapacity
            };
        }

        public static Mount ConvertMountViewModel(MountViewModel input)
        {
            return new Mount
            {
                PartitionKey = "mount",
                RowKey = string.IsNullOrEmpty(input.Id) ? Guid.NewGuid().ToString() : input.Id,
                Category = input.Category,
                Cost = input.Cost,
                Description = input.Description,
                Name = input.Name,
                Rarity = input.Rarity,
                Weight = input.Weight,
                Speed = input.Speed,
                CarryingCapacity = input.CarryingCapacity
            };
        }

        public static VehicleViewModel ConvertVehicleEntity(Vehicle input)
        {
            return new VehicleViewModel
            {
                Category = input.Category,
                Cost = input.Cost,
                Description = input.Description,
                Name = input.Name,
                Rarity = input.Rarity,
                Weight = input.Weight,
                Speed = input.Speed
            };
        }

        public static Vehicle ConvertVehicleViewModel(VehicleViewModel input)
        {
            return new Vehicle
            {
                PartitionKey = "vehicle",
                RowKey = string.IsNullOrEmpty(input.Id) ? Guid.NewGuid().ToString() : input.Id,
                Category = input.Category,
                Cost = input.Cost,
                Description = input.Description,
                Name = input.Name,
                Rarity = input.Rarity,
                Weight = input.Weight,
                Speed = input.Speed
            };
        }

        public static ContainerViewModel ConvertContainerModel(Container input)
        {
            return new ContainerViewModel
            {
                Category = input.Category,
                Cost = input.Cost,
                Description = input.Description,
                Name = input.Name,
                Rarity = input.Rarity,
                Weight = input.Weight,
                Capacity = input.Capacity,
                MaximumWeight = input.MaximumWeight,
                SizeType = input.SizeType
            };
        }

        public static Container ConvertContainerViewModel(ContainerViewModel input)
        {
            return new Container
            {
                PartitionKey = "container",
                RowKey = string.IsNullOrEmpty(input.Id) ? Guid.NewGuid().ToString() : input.Id,
                Category = input.Category,
                Cost = input.Cost,
                Description = input.Description,
                Name = input.Name,
                Rarity = input.Rarity,
                Weight = input.Weight,
                Capacity = input.Capacity,
                MaximumWeight = input.MaximumWeight,
                SizeType = input.SizeType
            };
        }

        public static AdventurePackViewModel ConvertAdventurePackEntity(AdventurePack input)
        {
            return new AdventurePackViewModel
            {
                Category = input.Category,
                Cost = input.Cost,
                Description = input.Description,
                Name = input.Name,
                Rarity = input.Rarity,
                Weight = input.Weight,
                Contents = input.Contents
            };
        }

        public static AdventurePack ConvertAdventurePackViewModel(AdventurePackViewModel input)
        {
            return new AdventurePack
            {
                PartitionKey = "adventure-pack",
                RowKey = string.IsNullOrEmpty(input.Id) ? Guid.NewGuid().ToString() : input.Id,
                Category = input.Category,
                Cost = input.Cost,
                Description = input.Description,
                Name = input.Name,
                Rarity = input.Rarity,
                Weight = input.Weight,
                Contents = input.Contents
            };
        }

        public static ItemViewModel ConvertItemEntity(Item input)
        {
            return new ItemViewModel
            {
                Category = input.Category,
                Cost = input.Cost,
                Description = input.Description,
                Name = input.Name,
                Rarity = input.Rarity,
                Weight = input.Weight
            };
        }

        public static Item ConvertItemViewModel(ItemViewModel input)
        {
            try
            {
                return new Item
                {
                    PartitionKey = "general",
                    RowKey = string.IsNullOrEmpty(input.Id) ? Guid.NewGuid().ToString() : input.Id,
                    Category = input.Category,
                    Cost = input.Cost,
                    Description = input.Description,
                    Name = input.Name,
                    Rarity = input.Rarity,
                    Weight = input.Weight
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
