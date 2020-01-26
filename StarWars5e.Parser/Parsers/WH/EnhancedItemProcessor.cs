using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models.EnhancedItems;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Parsers.WH
{
    public class EnhancedItemProcessor : BaseProcessor<EnhancedItem>
    {
        public override async Task<List<EnhancedItem>> FindBlocks(List<string> lines)
        {
            var enhancedItems = new List<EnhancedItem>();

            var enhancedItemsStartingIndex = lines.FindIndex(f => f.Equals("### Sample Enhanced Items"));
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
                var endIndex = lines.FindIndex(startIndex + 1, f => f.StartsWith("####"));

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

                    var prerequisiteLine = enhancedItemLines.Find(f => f.StartsWith("_**Prerequisite"));
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

                    if (enhancedItemLines.Any(f => f.StartsWith("_**Requires attunement")))
                    {
                        enhancedItem.RequiresAttunement = true;
                    }

                    var raritySplit = enhancedItemLines[1].RemoveMarkdownCharacters().Split(',', 2)
                        .Select(s => s.RemoveUnderscores()).ElementAtOrDefault(1);

                    if (raritySplit != null)
                    {
                        enhancedItem.RarityText = raritySplit.Trim();
                        if (raritySplit.ToLower().Contains("standard"))
                        {
                            enhancedItem.RarityOptionsEnum.Add(EnhancedItemRarity.Standard);
                        }
                        if (raritySplit.ToLower().Contains("premium"))
                        {
                            enhancedItem.RarityOptionsEnum.Add(EnhancedItemRarity.Premium);
                        }
                        if (raritySplit.ToLower().Contains("prototype"))
                        {
                            enhancedItem.RarityOptionsEnum.Add(EnhancedItemRarity.Prototype);
                        }
                        if (raritySplit.ToLower().Contains("advanced"))
                        {
                            enhancedItem.RarityOptionsEnum.Add(EnhancedItemRarity.Advanced);
                        }
                        if (raritySplit.ToLower().Contains("legendary"))
                        {
                            enhancedItem.RarityOptionsEnum.Add(EnhancedItemRarity.Legendary);
                        }
                        if (raritySplit.ToLower().Contains("artifact"))
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
                    if (typeSplit.ToLower().Contains("adventuring gear"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.AdventuringGear;
                        var typeSplitCheck = typeSplit.Split('(').ElementAtOrDefault(1) ?? "";
                        if (typeSplitCheck.ToLower().Contains("body"))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Body;
                        }
                        else if (typeSplitCheck.ToLower().Contains("Feet"))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Feet;
                        }
                        else if (typeSplitCheck.ToLower().Contains("Finger"))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Finger;
                        }
                        else if (typeSplitCheck.ToLower().Contains("Hands"))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Hands;
                        }
                        else if (typeSplitCheck.ToLower().Contains("Head"))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Head;
                        }
                        else if (typeSplitCheck.ToLower().Contains("Neck"))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Neck;
                        }
                        else if (typeSplitCheck.ToLower().Contains("Legs"))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Legs;
                        }
                        else if (typeSplitCheck.ToLower().Contains("Shoulders"))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Shoulders;
                        }
                        else if (typeSplitCheck.ToLower().Contains("Waist"))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Waist;
                        }
                        else if (typeSplitCheck.ToLower().Contains("Wrists"))
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Wrists;
                        }
                        else
                        {
                            enhancedItem.AdventuringGearTypeEnum = AdventuringGearType.Other;
                        }
                    }
                    else if (typeSplit.ToLower().Contains("armor modification"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.ArmorModification;
                        if (typeSplit.Split('(')[1].ToLower().Contains("reinforcement"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Reinforcement;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("shielding"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Shielding;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("overlay"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Overlay;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("underlay"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Underlay;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("armoring"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Armoring;
                        }
                    }
                    else if (typeSplit.ToLower().Contains("armor"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.Armor;
                        if (typeSplit.Split('(')[1].ToLower().Contains("any heavy"))
                        {
                            enhancedItem.EnhancedArmorTypeEnum = EnhancedArmorType.AnyHeavy;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("any heavy"))
                        {
                            enhancedItem.EnhancedArmorTypeEnum = EnhancedArmorType.AnyHeavy;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("any medium"))
                        {
                            enhancedItem.EnhancedArmorTypeEnum = EnhancedArmorType.AnyMedium;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("any light"))
                        {
                            enhancedItem.EnhancedArmorTypeEnum = EnhancedArmorType.AnyLight;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("any heavy"))
                        {
                            enhancedItem.EnhancedArmorTypeEnum = EnhancedArmorType.AnyHeavy;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("any"))
                        {
                            enhancedItem.EnhancedArmorTypeEnum = EnhancedArmorType.Any;
                        }
                        else
                        {
                            enhancedItem.EnhancedArmorTypeEnum = EnhancedArmorType.Specific;
                        }
                    }
                    else if (typeSplit.ToLower().Contains("consumable"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.Consumable;
                        if (typeSplit.Split('(')[1].ToLower().Contains("adrenal"))
                        {
                            enhancedItem.ConsumableTypeEnum = ConsumableType.Adrenals;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("explosive"))
                        {
                            enhancedItem.ConsumableTypeEnum = ConsumableType.Explosives;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("poison"))
                        {
                            enhancedItem.ConsumableTypeEnum = ConsumableType.Poisons;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("stimpac"))
                        {
                            enhancedItem.ConsumableTypeEnum = ConsumableType.Stimpacs;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("barrier"))
                        {
                            enhancedItem.ConsumableTypeEnum = ConsumableType.Barriers;
                        }
                        else
                        {
                            enhancedItem.ConsumableTypeEnum = ConsumableType.Other;
                        }
                    }
                    else if (typeSplit.ToLower().Contains("cybernetic augmentation"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.CyberneticAugmentation;
                        if (typeSplit.Split('(')[1].ToLower().Contains("enhancement"))
                        {
                            enhancedItem.CyberneticAugmentationTypeEnum = CyberneticAugmentationType.Enhancement;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("replacement"))
                        {
                            enhancedItem.CyberneticAugmentationTypeEnum = CyberneticAugmentationType.Replacement;
                        }
                        else
                        {
                            enhancedItem.CyberneticAugmentationTypeEnum = CyberneticAugmentationType.Other;
                        }
                    }
                    else if (typeSplit.ToLower().Contains("droid customization"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.DroidCustomization;
                        if (typeSplit.Split('(')[1].ToLower().Contains("part"))
                        {
                            enhancedItem.DroidCustomizationTypeEnum = DroidCustomizationType.Part;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("protocol"))
                        {
                            enhancedItem.DroidCustomizationTypeEnum = DroidCustomizationType.Protocol;
                        }
                        else
                        {
                            enhancedItem.DroidCustomizationTypeEnum = DroidCustomizationType.Other;
                        }
                    }
                    else if (typeSplit.ToLower().Contains("focus generator modification"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.FocusGeneratorModification;
                        if (typeSplit.Split('(')[1].ToLower().Contains("cycler"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Cycler;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("emitter"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Emitter;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("conductor"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Conductor;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("energy channel"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.EnergyChannel;
                        }
                    }
                    else if (typeSplit.ToLower().Contains("focus"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.Focus;
                        if (typeSplit.Split('(')[1].ToLower().Contains("force"))
                        {
                            enhancedItem.FocusTypeEnum = FocusType.Force;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("tech"))
                        {
                            enhancedItem.FocusTypeEnum = FocusType.Tech;
                        }
                        else
                        {
                            enhancedItem.FocusTypeEnum = FocusType.Other;
                        }
                    }
                    else if (typeSplit.ToLower().Contains("item modification"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.ItemModification;
                        if (typeSplit.Split('(')[1].ToLower().Contains("augment"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Augment;
                        }
                    }
                    else if (typeSplit.ToLower().Contains("wristpad modification"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.WristpadModification;
                        if (typeSplit.Split('(')[1].ToLower().Contains("dataport"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Dataport;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("storage"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Storage;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("motherboard"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Motherboard;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("processor"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Processor;
                        }
                    }
                    else if (typeSplit.ToLower().Contains("lightweapon modification"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.LightweaponModification;
                        if (typeSplit.Split('(')[1].ToLower().Contains("lens"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Lens;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("stabilizer"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Stabilizer;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("crystal"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Crystal;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("power cell"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.PowerCell;
                        }
                    }
                    else if (typeSplit.ToLower().Contains("vibroweapon modification"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.VibroweaponModification;
                        if (typeSplit.Split('(')[1].ToLower().Contains("edge"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Edge;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("vibrator cell"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.VibratorCell;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("projector"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Projector;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("grip"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Grip;
                        }
                    }
                    else if (typeSplit.ToLower().Contains("clothing modification"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.ClothingModification;
                        if (typeSplit.Split('(')[1].ToLower().Contains("inlay"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Inlay;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("weave"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Weave;
                        }
                    }
                    else if (typeSplit.ToLower().Contains("blaster modification"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.BlasterModification;
                        if (typeSplit.Split('(')[1].ToLower().Contains("barrel"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Barrel;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("targeting"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Targeting;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("matrix"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.Matrix;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("energy core"))
                        {
                            enhancedItem.ItemModificationTypeEnum = ItemModificationType.EnergyCore;
                        }
                    }
                    else if (typeSplit.ToLower().Contains("shield"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.Shield;
                        if (typeSplit.Split('(')[1].ToLower().Contains("heavy"))
                        {
                            enhancedItem.EnhancedShieldTypeEnum = EnhancedShieldType.Heavy;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("light"))
                        {
                            enhancedItem.EnhancedShieldTypeEnum = EnhancedShieldType.Light;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("medium"))
                        {
                            enhancedItem.EnhancedShieldTypeEnum = EnhancedShieldType.Medium;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("any"))
                        {
                            enhancedItem.EnhancedShieldTypeEnum = EnhancedShieldType.Any;
                        }
                        else
                        {
                            enhancedItem.EnhancedShieldTypeEnum = EnhancedShieldType.Specific;
                        }
                    }
                    else if (typeSplit.ToLower().Contains("weapon"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.Weapon;
                        if (typeSplit.Split('(')[1].ToLower().Contains("any blaster"))
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.AnyBlaster;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("any vibroweapon"))
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.AnyVibroweapon;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("any lightweapon"))
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.AnyLightweapon;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("lightweapon") && typeSplit.Split('(')[1].ToLower().Contains("property"))
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.AnyLightweaponWithProperty;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("blaster") && typeSplit.Split('(')[1].ToLower().Contains("property"))
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.AnyBlasterWithProperty;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("vibroweapon") && typeSplit.Split('(')[1].ToLower().Contains("property"))
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.AnyVibroweaponWithProperty;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("any") && typeSplit.Split('(')[1].ToLower().Contains("property"))
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.AnyWithProperty;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("any"))
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.Any;
                        }
                        else
                        {
                            enhancedItem.EnhancedWeaponTypeEnum = EnhancedWeaponType.Specific;
                        }
                        
                    }
                    else if (typeSplit.ToLower().Contains("valuable"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.Valuable;
                        if (typeSplit.Split('(')[1].ToLower().Contains("art"))
                        {
                            enhancedItem.ValuableTypeEnum = ValuableType.Art;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("jewel"))
                        {
                            enhancedItem.ValuableTypeEnum = ValuableType.Jewel;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("relic"))
                        {
                            enhancedItem.ValuableTypeEnum = ValuableType.Relic;
                        }
                        else if (typeSplit.Split('(')[1].ToLower().Contains("sculpture"))
                        {
                            enhancedItem.ValuableTypeEnum = ValuableType.Sculpture;
                        }
                        else
                        {
                            enhancedItem.ValuableTypeEnum = ValuableType.Other;
                        }
                    }
                    else if (typeSplit.ToLower().Contains("ship armor"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.ShipArmor;
                    }
                    else if (typeSplit.ToLower().Contains("ship shield"))
                    {
                        enhancedItem.TypeEnum = EnhancedItemType.ShipShield;
                    }
                    else if (typeSplit.ToLower().Contains("ship weapon"))
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
