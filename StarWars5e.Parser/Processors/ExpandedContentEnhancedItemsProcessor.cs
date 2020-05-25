using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models.EnhancedItems;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Processors.WH;

namespace StarWars5e.Parser.Processors
{
    public class ExpandedContentEnhancedItemsProcessor : BaseProcessor<EnhancedItem>
    {
        public override async Task<List<EnhancedItem>> FindBlocks(List<string> lines)
        {
            var enhancedItemProcessor = new EnhancedItemProcessor(Localization);
            return await enhancedItemProcessor.ParseEnhancedItems(lines, ContentType.ExpandedContent);
        }
    }
}
