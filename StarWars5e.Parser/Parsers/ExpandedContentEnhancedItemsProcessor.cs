using System.Collections.Generic;
using System.Threading.Tasks;
using StarWars5e.Models.EnhancedItems;
using StarWars5e.Models.Enums;
using StarWars5e.Parser.Parsers.WH;

namespace StarWars5e.Parser.Parsers
{
    public class ExpandedContentEnhancedItemsProcessor : BaseProcessor<EnhancedItem>
    {
        private readonly EnhancedItemProcessor _enhancedItemProcessor;
        public ExpandedContentEnhancedItemsProcessor()
        {
            _enhancedItemProcessor = new EnhancedItemProcessor();
        }

        public override async Task<List<EnhancedItem>> FindBlocks(List<string> lines)
        {
            return await _enhancedItemProcessor.ParseEnhancedItems(lines, ContentType.ExpandedContent);
        }
    }
}
