using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;

namespace StarWars5e.Parser.Parsers
{
    public class ExpandedContentVariantRulesProcessor : BaseProcessor<ChapterRules>
    {
        public override Task<List<ChapterRules>> FindBlocks(List<string> lines)
        {
            var variantRules = new List<ChapterRules>();

            for (var i = 0; i < lines.Count; i++)
            {
                if (!lines[i].StartsWith("# ")) continue;

                var variantEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("# "));
                var variantLines = lines.Skip(i).CleanListOfStrings().ToList();
                if (variantEndIndex != -1)
                {
                    variantLines = lines.Skip(i).Take(variantEndIndex - i).CleanListOfStrings().ToList();
                }

                variantRules.Add(ParseVariantRules(variantLines, ContentType.ExpandedContent));
            }

            return Task.FromResult(variantRules);
        }

        public static ChapterRules ParseVariantRules(List<string> variantLines, ContentType contentType)
        {
            var variantRule = new ChapterRules();
            var name = variantLines[0].Split('#')[1].Trim();
            try
            {
                if (name == "Force Alignment")
                {
                    variantLines[2] = variantLines[2].Insert(0, "T");
                }
                if (name == "Destiny")
                {
                    variantLines[2] = variantLines[2].Insert(0, "D");
                }
                if (name == "Starship Destiny")
                {
                    variantLines[2] = variantLines[2].Insert(0, "S");
                }
                if (name == "Weapon Sundering")
                {
                    variantLines[2] = variantLines[2].Insert(0, "Y");
                }

                variantRule.ChapterName = name;
                variantRule.ContentMarkdown = string.Join("\r\n", variantLines.Skip(1).ToList());
                return variantRule;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing variant rule {name}", e);
            }
        }
    }
}
