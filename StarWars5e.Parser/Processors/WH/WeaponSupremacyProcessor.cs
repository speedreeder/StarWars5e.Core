using StarWars5e.Models.CustomizationOptions;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarWars5e.Parser.Processors.WH
{
    public class WeaponSupremacyProcessor : BaseProcessor<WeaponSupremacy>
    {
        public override Task<List<WeaponSupremacy>> FindBlocks(List<string> lines, ContentType contentType)
        {
            var weaponSupremacies = new List<WeaponSupremacy>();
            lines = lines.CleanListOfStrings().ToList();

            var weaponSupremaciesStart = lines.FindIndex(f => f.StartsWith("## Weapon Supremacy"));
            var weaponSupremaciesTempEndIndex = lines.FindIndex(weaponSupremaciesStart + 1, f => f.StartsWith("## "));
            var weaponSupremaciesEndIndex = weaponSupremaciesTempEndIndex != -1 ? weaponSupremaciesTempEndIndex : lines.Count;

            for (var i = weaponSupremaciesStart; i < weaponSupremaciesEndIndex; i++)
            {
                if (!lines[i].StartsWith("#### ")) continue;

                var weaponSupremacyStartIndex = i;
                var weaponSupremacyEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("#### "));

                var weaponSupremacyLines = lines.Skip(weaponSupremacyStartIndex).Take(weaponSupremaciesEndIndex - weaponSupremacyStartIndex);
                if (weaponSupremacyEndIndex != -1 && weaponSupremacyEndIndex < weaponSupremaciesEndIndex)
                {
                    weaponSupremacyLines = lines.Skip(weaponSupremacyStartIndex).Take(weaponSupremacyEndIndex - weaponSupremacyStartIndex);
                }

                var weaponSupremacy = ParseWeaponSupremacy(weaponSupremacyLines.ToList(), contentType);
                weaponSupremacies.Add(weaponSupremacy);
            }

            return Task.FromResult(weaponSupremacies);
        }

        public WeaponSupremacy ParseWeaponSupremacy(List<string> weaponSupremacyLines, ContentType contentType)
        {
            var name = weaponSupremacyLines[0].Split("####")[1].Trim();
            try
            {
                var weaponSupremacy = new WeaponSupremacy
                {
                    ContentTypeEnum = contentType,
                    Name = name,
                    PartitionKey = contentType.ToString(),
                    RowKey = name,
                    Description = string.Join("\r\n", weaponSupremacyLines.Skip(1).ToList())
                };

                return weaponSupremacy;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing weapon supremacy {name}", e);
            }
        }
    }
}
