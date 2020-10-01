using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Equipment;

namespace StarWars5e.Parser.Processors.PHB
{
    public class PlayerHandbookEquipmentProcessor: BaseProcessor<Equipment>
    {
        public override async Task<List<Equipment>> FindBlocks(List<string> lines)
        {
            var equipmentList = new List<Equipment>();

            var expandedContentEquipmentProcessor = new ExpandedContentEquipmentProcessor(Localization);
            equipmentList.AddRange(await expandedContentEquipmentProcessor.ParseWeapons(lines,
                Localization.PHBSimpleBlastersTableStart, true, 1, ContentType.Core));
            equipmentList.AddRange(await expandedContentEquipmentProcessor.ParseWeapons(lines,
                Localization.PHBMartialBlastersTableStart, true, 1, ContentType.Core));
            equipmentList.AddRange(await expandedContentEquipmentProcessor.ParseWeapons(lines,
                Localization.PHBVibroweaponsTableStart, false, 1, ContentType.Core));
            equipmentList.AddRange(await expandedContentEquipmentProcessor.ParseWeapons(lines,
                Localization.PHBLightweaponsTableStart, false, 1, ContentType.Core));

            equipmentList.AddRange(await expandedContentEquipmentProcessor.ParseArmor(lines,
                Localization.PHBArmorAndShieldsTableStart, ContentType.Core));

            equipmentList.AddRange(await expandedContentEquipmentProcessor.ParseOtherEquipment(lines,
                Localization.PHBArtisansImplementsTableStart, true, 1, ContentType.Core));
            equipmentList.AddRange(await expandedContentEquipmentProcessor.ParseOtherEquipment(lines,
                Localization.PHBAmmunitionTableStart, true, 2, ContentType.Core));
            equipmentList.AddRange(await expandedContentEquipmentProcessor.ParseOtherEquipment(lines,
                Localization.PHBMedicalTableStart, true, 2, ContentType.Core));

            return equipmentList;
        }
    }
}
