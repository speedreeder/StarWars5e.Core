using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models.EnhancedItems;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;
using StarWars5e.Parser.Localization;

namespace StarWars5e.Parser.Processors.WH
{
    public class EnhancedItemProcessor : BaseProcessor<EnhancedItem>
    {
        public EnhancedItemProcessor(ILocalization localization)
        {
            Localization = localization;
        }
        public override async Task<List<EnhancedItem>> FindBlocks(List<string> lines)
        {
            var enhancedItems = new List<EnhancedItem>();

            var enhancedItemsStartingIndex = lines.FindIndex(f => f.Equals(Localization.SampleEnhancedItems));
            var enhancedItemsLines = lines.Skip(enhancedItemsStartingIndex).ToList();

            enhancedItems.AddRange(await ParseEnhancedItems(enhancedItemsLines, ContentType.Core));
            return enhancedItems;
        }

        public Task<List<EnhancedItem>> ParseEnhancedItems(List<string> lines, ContentType contentType)
        {
            var enhancedItems = new List<EnhancedItem>();
            var enhancedItemLineStarts = lines.Where(l => l.StartsWith("####")).ToList();
            foreach (var enhancedItemLine in enhancedItemLineStarts)
            {
                var startIndex = lines.IndexOf(enhancedItemLine);
                var endIndex = lines.FindIndex(startIndex + 1, f => f.StartsWith("#"));

                var enhancedItemLines = lines.Skip(startIndex).Take(endIndex - startIndex).CleanListOfStrings().ToList();
                if (endIndex == -1)
                {
                    enhancedItemLines = lines.Skip(startIndex).CleanListOfStrings().ToList();
                }

                var name = enhancedItemLines[0].Split("####")[1].Trim();
                try
                {
                    var enhancedItem = new EnhancedItem
                    {
                        Name = name,
                        ContentTypeEnum = contentType,
                        RowKey = name,
                        PartitionKey = contentType.ToString()
                    };

                    var prerequisiteLine = enhancedItemLines.Find(f => f.StartsWith($"_**{Localization.Prerequisite}"));
                    if (prerequisiteLine != null)
                    {
                        enhancedItem.HasPrerequisite = true;
                        enhancedItem.Prerequisite = prerequisiteLine.Split(':')[1].Trim().RemoveMarkdownCharacters()
                            .RemoveUnderscores();
                    }

                    if (prerequisiteLine != null)
                    {
                        var prerequisiteLineIndex = enhancedItemLines.FindIndex(f => f == prerequisiteLine);
                        enhancedItem.Text = string.Join("\r\n",
                            enhancedItemLines.Skip(prerequisiteLineIndex + 1).CleanListOfStrings());
                    }
                    else
                    {
                        enhancedItem.Text = string.Join("\r\n",
                            enhancedItemLines.Skip(2).CleanListOfStrings());
                    }

                    if (enhancedItemLines.Any(f => f.StartsWith($"_**{Localization.RequiresAttunement}")))
                    {
                        enhancedItem.RequiresAttunement = true;
                    }

                    var raritySplit = enhancedItemLines[1].RemoveMarkdownCharacters().Split(',', 2)
                        .Select(s => s.RemoveUnderscores()).ElementAtOrDefault(1);

                    if (raritySplit != null)
                    {
                        enhancedItem.RarityText = raritySplit.Trim();
                        if (raritySplit.ToLower().Contains(Localization.standard))
                        {
                            enhancedItem.RarityOptionsEnum.Add(EnhancedItemRarity.Standard);
                        }
                        if (raritySplit.ToLower().Contains(Localization.premium))
                        {
                            enhancedItem.RarityOptionsEnum.Add(EnhancedItemRarity.Premium);
                        }
                        if (raritySplit.ToLower().Contains(Localization.prototype))
                        {
                            enhancedItem.RarityOptionsEnum.Add(EnhancedItemRarity.Prototype);
                        }
                        if (raritySplit.ToLower().Contains(Localization.advanced))
                        {
                            enhancedItem.RarityOptionsEnum.Add(EnhancedItemRarity.Advanced);
                        }
                        if (raritySplit.ToLower().Contains(Localization.legendary))
                        {
                            enhancedItem.RarityOptionsEnum.Add(EnhancedItemRarity.Legendary);
                        }
                        if (raritySplit.ToLower().Contains(Localization.artifact))
                        {
                            enhancedItem.RarityOptionsEnum.Add(EnhancedItemRarity.Artifact);
                        }
                    }
                    else
                    {
                        enhancedItem.RarityOptionsEnum.Add(EnhancedItemRarity.None);
                    }

                    var typeSplit = enhancedItemLines[1].Split(',')[0];
                    enhancedItem.Subtype = (typeSplit.Split('(').ElementAtOrDefault(1) ?? "").Trim().Replace("(", string.Empty)
                        .Replace(")", string.Empty).RemoveUnderscores();
                    if (typeSplit.ToLower().Contains(Localization.adventuringgear))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.AdventuringGear;
                        var typeSplitCheck = typeSplit.Split('(').ElementAtOrDefault(1) ?? "";
                        if (typeSplitCheck.ToLower().Contains(Localization.body))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Body;
                        }
                        else if (typeSplitCheck.ToLower().Contains(Localization.Feet))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Feet;
                        }
                        else if (typeSplitCheck.ToLower().Contains(Localization.Finger))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Finger;
                        }
                        else if (typeSplitCheck.ToLower().Contains(Localization.Hands))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Hands;
                        }
                        else if (typeSplitCheck.ToLower().Contains(Localization.Head))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Head;
                        }
                        else if (typeSplitCheck.ToLower().Contains(Localization.Neck))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Neck;
                        }
                        else if (typeSplitCheck.ToLower().Contains(Localization.Legs))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Legs;
                        }
                        else if (typeSplitCheck.ToLower().Contains(Localization.Shoulders))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Shoulders;
                        }
                        else if (typeSplitCheck.ToLower().Contains(Localization.Waist))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Waist;
                        }
                        else if (typeSplitCheck.ToLower().Contains(Localization.Wrists))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Wrists;
                        }
                        else
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Other;
                        }
                    }
                    else if (typeSplit.ToLower().Contains(Localization.armormodification))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.ArmorModification;
                        if (typeSplit.Split('(')[1].ToLower().Contains(Localization.reinforcement))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Reinforcement;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.shielding))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Shielding;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.overlay))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Overlay;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.underlay))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Underlay;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.armoring))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Armoring;
                        }
                    }
                    else if (typeSplit.ToLower().Contains($"**{Localization.armor}**"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.Armor;
                        if (typeSplit.Split('(')[1].ToLower().Contains(Localization.anyheavy))
                        {
                            enhancedItem.EnhancedArmorTypeEnum = EnhancedArmorType.AnyHeavy;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.anyheavy))
                        {
                            enhancedItem.EnhancedArmorTypeEnum = EnhancedArmorType.AnyHeavy;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.anymedium))
                        {
                            enhancedItem.EnhancedArmorTypeEnum = EnhancedArmorType.AnyMedium;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.anylight))
                        {
                            enhancedItem.EnhancedArmorTypeEnum = EnhancedArmorType.AnyLight;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.anyheavy))
                        {
                            enhancedItem.EnhancedArmorTypeEnum = EnhancedArmorType.AnyHeavy;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.any))
                        {
                            enhancedItem.EnhancedArmorTypeEnum = EnhancedArmorType.Any;
                        }
                        else
                        {
                            enhancedItem.EnhancedArmorTypeEnum = EnhancedArmorType.Specific;
                        }
                    }
                    else if (typeSplit.ToLower().Contains(Localization.consumable))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.Consumable;
                        if (typeSplit.Split('(')[1].ToLower().Contains(Localization.adrenal))
                        {
                            enhancedItem.ConsumableTypeEnum = ConsumableType.Adrenals;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.explosive))
                        {
                            enhancedItem.ConsumableTypeEnum = ConsumableType.Explosives;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.poison))
                        {
                            enhancedItem.ConsumableTypeEnum = ConsumableType.Poisons;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.stimpac))
                        {
                            enhancedItem.ConsumableTypeEnum = ConsumableType.Stimpacs;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.barrier))
                        {
                            enhancedItem.ConsumableTypeEnum = ConsumableType.Barriers;
                        }
                        else
                        {
                            enhancedItem.ConsumableTypeEnum = ConsumableType.Other;
                        }
                    }
                    else if (typeSplit.ToLower().Contains(Localization.cyberneticaugmentation))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.CyberneticAugmentation;
                        if (typeSplit.Split('(')[1].ToLower().Contains(Localization.enhancement))
                        {
                            enhancedItem.CyberneticAugmentationTypeEnum = CyberneticAugmentationType.Enhancement;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.replacement))
                        {
                            enhancedItem.CyberneticAugmentationTypeEnum = CyberneticAugmentationType.Replacement;
                        }
                        else
                        {
                            enhancedItem.CyberneticAugmentationTypeEnum = CyberneticAugmentationType.Other;
                        }
                    }
                    else if (typeSplit.ToLower().Contains(Localization.droidcustomization))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.DroidCustomization;
                        if (typeSplit.Split('(')[1].ToLower().Contains(Localization.part))
                        {
                            enhancedItem.DroidCustomizationTypeEnum = DroidCustomizationType.Part;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.protocol))
                        {
                            enhancedItem.DroidCustomizationTypeEnum = DroidCustomizationType.Protocol;
                        }
                        else
                        {
                            enhancedItem.DroidCustomizationTypeEnum = DroidCustomizationType.Other;
                        }
                    }
                    else if (typeSplit.ToLower().Contains(Localization.focusgeneratormodification))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.FocusGeneratorModification;
                        if (typeSplit.Split('(')[1].ToLower().Contains(Localization.cycler))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Cycler;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.emitter))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Emitter;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.conductor))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Conductor;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.energychannel))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.EnergyChannel;
                        }
                    }
                    else if (typeSplit.ToLower().Contains(Localization.focus))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.Focus;
                        if (typeSplit.Split('(')[1].ToLower().Contains(Localization.force))
                        {
                            enhancedItem.FocusTypeEnum = FocusType.Force;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.force))
                        {
                            enhancedItem.FocusTypeEnum = FocusType.Tech;
                        }
                        else
                        {
                            enhancedItem.FocusTypeEnum = FocusType.Other;
                        }
                    }
                    else if (typeSplit.ToLower().Contains(Localization.itemmodification))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.ItemModification;
                        if (typeSplit.Split('(')[1].ToLower().Contains(Localization.augment))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Augment;
                        }
                    }
                    else if (typeSplit.ToLower().Contains(Localization.wristpadmodification))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.WristpadModification;
                        if (typeSplit.Split('(')[1].ToLower().Contains(Localization.dataport))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Dataport;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.storage))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Storage;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.motherboard))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Motherboard;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.processor))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Processor;
                        }
                    }
                    else if (typeSplit.ToLower().Contains(Localization.lightweaponmodification))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.LightweaponModification;
                        if (typeSplit.Split('(')[1].ToLower().Contains(Localization.lens))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Lens;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.stabilizer))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Stabilizer;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.crystal))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Crystal;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.powercell))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.PowerCell;
                        }
                    }
                    else if (typeSplit.ToLower().Contains(Localization.vibroweaponmodification))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.VibroweaponModification;
                        if (typeSplit.Split('(')[1].ToLower().Contains(Localization.edge))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Edge;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.vibratorcell))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.VibratorCell;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.projector))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Projector;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.grip))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Grip;
                        }
                    }
                    else if (typeSplit.ToLower().Contains(Localization.clothingmodification))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.ClothingModification;
                        if (typeSplit.Split('(')[1].ToLower().Contains(Localization.inlay))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Inlay;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.weave))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Weave;
                        }
                    }
                    else if (typeSplit.ToLower().Contains(Localization.blastermodification))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.BlasterModification;
                        if (typeSplit.Split('(')[1].ToLower().Contains(Localization.barrel))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Barrel;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.targeting))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Targeting;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.matrix))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Matrix;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.energycore))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.EnergyCore;
                        }
                    }
                    else if (typeSplit.ToLower().Contains($"**{Localization.shield}**"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.Shield;
                        if (typeSplit.Split('(')[1].ToLower().Contains(Localization.heavy))
                        {
                            enhancedItem.EnhancedShieldTypeEnum = EnhancedShieldType.Heavy;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.light))
                        {
                            enhancedItem.EnhancedShieldTypeEnum = EnhancedShieldType.Light;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.medium))
                        {
                            enhancedItem.EnhancedShieldTypeEnum = EnhancedShieldType.Medium;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.any))
                        {
                            enhancedItem.EnhancedShieldTypeEnum = EnhancedShieldType.Any;
                        }
                        else
                        {
                            enhancedItem.EnhancedShieldTypeEnum = EnhancedShieldType.Specific;
                        }
                    }
                    else if (typeSplit.ToLower().Contains($"**{Localization.weapon}**"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.Weapon;
                        if (typeSplit.Split('(')[1].ToLower().Contains(Localization.anyblaster))
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.AnyBlaster;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.anyvibroweapon))
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.AnyVibroweapon;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.anylightweapon))
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.AnyLightweapon;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.lightweapon) && typeSplit.Split('(')[1].ToLower().Contains(Localization.property))
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.AnyLightweaponWithProperty;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.blaster) && typeSplit.Split('(')[1].ToLower().Contains(Localization.property))
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.AnyBlasterWithProperty;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.vibroweapon) && typeSplit.Split('(')[1].ToLower().Contains(Localization.property))
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.AnyVibroweaponWithProperty;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.any) && typeSplit.Split('(')[1].ToLower().Contains(Localization.property))
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.AnyWithProperty;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.any))
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.Any;
                        }
                        else
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.Specific;
                        }
                        
                    }
                    else if (typeSplit.ToLower().Contains(Localization.valuable))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.Valuable;
                        if (typeSplit.Split('(')[1].ToLower().Contains(Localization.art))
                        {
                            enhancedItem.ValuableTypeEnum = ValuableType.Art;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.jewel))
                        {
                            enhancedItem.ValuableTypeEnum = ValuableType.Jewel;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.relic))
                        {
                            enhancedItem.ValuableTypeEnum = ValuableType.Relic;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains(Localization.sculpture))
                        {
                            enhancedItem.ValuableTypeEnum = ValuableType.Sculpture;
                        }
                        else
                        {
                            enhancedItem.ValuableTypeEnum = ValuableType.Other;
                        }
                    }
                    else if (typeSplit.ToLower().Contains(Localization.shiparmor))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.ShipArmor;
                    }
                    else if (typeSplit.ToLower().Contains(Localization.shipshield))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.ShipShield;
                    }
                    else if (typeSplit.ToLower().Contains(Localization.shipweapon))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.ShipWeapon;
                    }
                    enhancedItems.Add(enhancedItem);
                }
                catch (Exception e)
                {
                    throw new Exception($"Failed while parsing {name}", e);
                }
            }

            return Task.FromResult(enhancedItems);
        }
    }
}
