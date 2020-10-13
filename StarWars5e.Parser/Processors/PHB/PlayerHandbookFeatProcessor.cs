using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StarWars5e.Models;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;
using StarWars5e.Parser.Localization;
using Attribute = StarWars5e.Models.Enums.Attribute;

namespace StarWars5e.Parser.Processors.PHB
{
    public class PlayerHandbookFeatProcessor : BaseProcessor<Feat>
    {
        public PlayerHandbookFeatProcessor(ILocalization localization)
        {
            Localization = localization;
        }
        public override Task<List<Feat>> FindBlocks(List<string> lines)
        {
            var feats = new List<Feat>();
            lines = lines.CleanListOfStrings().ToList();
            var featsStart = lines.FindIndex(f => f.Equals($"## {Localization.Feats}"));
            var featsLines = lines.Skip(featsStart + 1).ToList();

            for (var i = 0; i < featsLines.Count; i++)
            {
                if (!featsLines[i].StartsWith("### ")) continue;

                var featStartIndex = i;
                var featEndIndex = featsLines.FindIndex(i + 1, f => f.StartsWith("##"));

                var featLines = featsLines.Skip(featStartIndex);
                if (featEndIndex != -1)
                {
                    featLines = featsLines.Skip(featStartIndex).Take(featEndIndex - featStartIndex);
                }

                var feat = ParseFeat(featLines.ToList(), ContentType.Core);
                feats.Add(feat);
            }

            return Task.FromResult(feats);
        }

        public Feat ParseFeat(List<string> featLines, ContentType contentType)
        {
            var name = featLines[0].Split("###")[1].Trim();
            try
            {
                var feat = new Feat
                {
                    ContentTypeEnum = contentType,
                    Name = name,
                    PartitionKey = contentType.ToString(),
                    RowKey = name
                };

                var prerequisiteIndex =
                    featLines.FindIndex(f =>
                        f.StartsWith($"*{Localization.Prerequisite}") || f.StartsWith($"{Localization.Prerequisite}") ||
                        f.StartsWith($"_**{Localization.Prerequisite}") || f.StartsWith($"_*{Localization.Prerequisite}"));

                if (prerequisiteIndex != -1)
                {
                    feat.Prerequisite = featLines[prerequisiteIndex].Split(':')[1].Replace("*", string.Empty).Replace("_", string.Empty)
                        .RemoveHtmlWhitespace().Trim();
                    feat.Text = string.Join("\r\n", featLines.Skip(prerequisiteIndex + 1).ToList());
                }
                else
                {
                    feat.Text = string.Join("\r\n", featLines.Skip(1).ToList());
                }

                var attributesChoices = Enum.GetNames(typeof(Attribute)).ToList();
                attributesChoices.Add(Localization.ability);
                var attributeIncreaseIndex = featLines.FindIndex(f =>
                    Regex.IsMatch(f, Localization.PHBAttributeIncreaseIndexPattern(attributesChoices), RegexOptions.IgnoreCase));

                if (attributeIncreaseIndex != -1)
                {
                    var attributeIncreaseLine = featLines[attributeIncreaseIndex];
                    feat.AttributesIncreased = new List<string>();
                    if (attributeIncreaseLine.Contains(Attribute.Strength.ToString()))
                    {
                        feat.AttributesIncreased.Add(Attribute.Strength.ToString());
                    }
                    if (attributeIncreaseLine.Contains(Attribute.Dexterity.ToString()))
                    {
                        feat.AttributesIncreased.Add(Attribute.Dexterity.ToString());
                    }
                    if (attributeIncreaseLine.Contains(Attribute.Constitution.ToString()))
                    {
                        feat.AttributesIncreased.Add(Attribute.Constitution.ToString());
                    }
                    if (attributeIncreaseLine.Contains(Attribute.Intelligence.ToString()))
                    {
                        feat.AttributesIncreased.Add(Attribute.Intelligence.ToString());
                    }
                    if (attributeIncreaseLine.Contains(Attribute.Wisdom.ToString()))
                    {
                        feat.AttributesIncreased.Add(Attribute.Wisdom.ToString());
                    }
                    if (attributeIncreaseLine.Contains(Attribute.Charisma.ToString()))
                    {
                        feat.AttributesIncreased.Add(Attribute.Charisma.ToString());
                    }
                    if (attributeIncreaseLine.Contains(Localization.PHBFeatYourChoice))
                    {
                        feat.AttributesIncreased.Add(Localization.Any);
                    }
                }

                return feat;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing feat {name}", e);
            }
        }
    }
}
