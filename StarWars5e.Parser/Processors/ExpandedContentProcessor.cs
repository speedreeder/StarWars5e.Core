using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Processors
{
    public class ExpandedContentProcessor : BaseProcessor<ChapterRules>
    {
        public override Task<List<ChapterRules>> FindBlocks(List<string> lines)
        {
            var expandedContent = new List<ChapterRules>
            {
                ParseExpandedContent(lines.CleanListOfStrings(false).ToList(), ContentType.ExpandedContent)
            };

            return Task.FromResult(expandedContent);
        }

        public ChapterRules ParseExpandedContent(List<string> lines, ContentType contentType)
        {
            if (lines.Count <= 2)
            {
                return null;
            }

            var expandedContent = new ChapterRules();

            var nameLineIndex = lines.FindIndex(f => f.StartsWith("# "));

            var name = lines[nameLineIndex].Split('#')[1].Trim();

            expandedContent.ChapterName = name;
            expandedContent.ContentMarkdown = string.Join("\r\n", lines.Skip(1).ToList());
            expandedContent.ContentTypeEnum = contentType;

            return expandedContent;
        }
    }
}
