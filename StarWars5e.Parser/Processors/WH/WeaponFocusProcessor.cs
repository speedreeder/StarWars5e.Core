using StarWars5e.Models.CustomizationOptions;
using StarWars5e.Models.Enums;
using StarWars5e.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarWars5e.Parser.Processors.WH
{
    public class WeaponFocusProcessor : BaseProcessor<WeaponFocus>
    {
        public override Task<List<WeaponFocus>> FindBlocks(List<string> lines, ContentType contentType)
        {
            var weaponFocuses = new List<WeaponFocus>();
            lines = lines.CleanListOfStrings().ToList();

            var weaponFocusesStart = lines.FindIndex(f => f.StartsWith("## Weapon Focus"));
            var weaponFocusesTempEndIndex = lines.FindIndex(weaponFocusesStart + 1, f => f.StartsWith("## "));
            var weaponFocusesEndIndex = weaponFocusesTempEndIndex != -1 ? weaponFocusesTempEndIndex : lines.Count;

            for (var i = weaponFocusesStart; i < weaponFocusesEndIndex; i++)
            {
                if (!lines[i].StartsWith("#### ")) continue;

                var weaponFocusStartIndex = i;
                var weaponFocusEndIndex = lines.FindIndex(i + 1, f => f.StartsWith("#### "));

                var weaponFocusLines = lines.Skip(weaponFocusStartIndex).Take(weaponFocusesEndIndex - weaponFocusStartIndex);
                if (weaponFocusEndIndex != -1 && weaponFocusEndIndex < weaponFocusesEndIndex)
                {
                    weaponFocusLines = lines.Skip(weaponFocusStartIndex).Take(weaponFocusEndIndex - weaponFocusStartIndex);
                }

                var weaponFocus = ParseWeaponFocus(weaponFocusLines.ToList(), contentType);
                weaponFocuses.Add(weaponFocus);
            }

            return Task.FromResult(weaponFocuses);
        }

        public WeaponFocus ParseWeaponFocus(List<string> weaponFocusLines, ContentType contentType)
        {
            var name = weaponFocusLines[0].Split("####")[1].Trim();
            try
            {
                var weaponFocus = new WeaponFocus
                {
                    ContentTypeEnum = contentType,
                    Name = name,
                    PartitionKey = contentType.ToString(),
                    RowKey = name,
                    Description = string.Join("\r\n", weaponFocusLines.Skip(1).ToList())
                };

                return weaponFocus;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed while parsing weapon focus {name}", e);
            }
        }
    }
}
