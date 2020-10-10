using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Equipment;

namespace StarWars5e.Parser.Processors.WH
{
    public class WretchedHivesEquipmentProcessor: BaseProcessor<Equipment>
    {
        public override async Task<List<Equipment>> FindBlocks(List<string> lines)
        {
            var equipmentList = new List<Equipment>();

            var expandedContentEquipmentProcessor = new ExpandedContentEquipmentProcessor(Localization);
            equipmentList.AddRange(await expandedContentEquipmentProcessor.ParseWeapons(lines, Localization.WHBlastersStartLine, false, 1, ContentType.Core));
            equipmentList.AddRange(await expandedContentEquipmentProcessor.ParseWeapons(lines, Localization.WHVibroweaponsStartLine, false, 1, ContentType.Core));
            equipmentList.AddRange(await expandedContentEquipmentProcessor.ParseWeapons(lines, Localization.WHLightweaponsStartLine, false, 1, ContentType.Core));

            equipmentList.AddRange(await expandedContentEquipmentProcessor.ParseArmor(lines, Localization.WHArmorAndShieldsStartLine, ContentType.Core));

            equipmentList.AddRange(await expandedContentEquipmentProcessor.ParseOtherEquipment(lines, Localization.WHSpecialistsKitStartLine, true, 1, ContentType.Core));
            
            equipmentList.AddRange(await expandedContentEquipmentProcessor.ParseOtherEquipment(lines, Localization.WHAmmunitionStartLine, true, 5, ContentType.Core));
            equipmentList.AddRange(await expandedContentEquipmentProcessor.ParseOtherEquipment(lines, Localization.WHExplosivesStartLine, true, 4, ContentType.Core));
            equipmentList.AddRange(await expandedContentEquipmentProcessor.ParseOtherEquipment(lines, Localization.WHAlcoholicBeveragesStartLine, true, 1, ContentType.Core));
            equipmentList.AddRange(await expandedContentEquipmentProcessor.ParseOtherEquipment(lines, Localization.WHArtisansImplementsStartLine, true, 1, ContentType.Core));

            return equipmentList;
        }
    }
}
