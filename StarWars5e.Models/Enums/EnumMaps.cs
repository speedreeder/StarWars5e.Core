using System.Collections.Generic;
using System.Linq;

namespace StarWars5e.Models.Enums
{
    public class EnumMaps
    {
        #region Size Enum

        private static Dictionary<string, Size> sizes = new Dictionary<string, Size>
        {
            {"tiny", Size.Tiny},
            {"small", Size.Small},
            {"medium", Size.Medium},
            {"large", Size.Large},
            {"huge", Size.Huge},
            {"gargantuan", Size.Gargantuan}
        };

        /// <summary>
        /// Retrieve a size by the string representation
        /// </summary>
        /// <param name="input">Value to check</param>
        /// <returns>The size value, or unknown if the value isn't matched</returns>
        public static Size RetrieveSize(string input)
        {
            var result = sizes.TryGetValue(input.ToLower(), out var found);
            return result
                ? found
                : Size.Unknown;
        }

        /// <summary>
        /// Given a size this will return the appropriate string (with casing)
        /// </summary>
        /// <param name="input">a <see cref="Size"/></param>
        /// <returns>String representation</returns>
        public static string ConvertSizeToString(Size input)
        {
            var found = sizes.FirstOrDefault(s => s.Value == input);
            return found.Key;
        }

        #endregion

        #region Monster Types

        private static Dictionary<string, MonsterType> monsterTypes = new Dictionary<string, MonsterType>
        {
            {"beast", MonsterType.Beast},
            {"construct", MonsterType.Construct},
            {"droid", MonsterType.Droid},
            {"force-wielder", MonsterType.ForceWielder},
            {"humanoid", MonsterType.Humanoid},
        };

        /// <summary>
        /// Retrieve a monster type by the string representation
        /// </summary>
        /// <param name="input">Value to check</param>
        /// <returns>The size value, or unknown if the value isn't matched</returns>
        public static MonsterType RetrieveMonsterType(string input)
        {
            var result = monsterTypes.TryGetValue(input.ToLower(), out var found);
            return result
                ? found
                : MonsterType.Unknown;
        }

        /// <summary>
        /// Given a monster type this will return the appropriate string (with casing)
        /// </summary>
        /// <param name="input">a <see cref="Size"/></param>
        /// <returns>String representation</returns>
        public static string ConvertMonsterTypeToString(MonsterType input)
        {
            var found = monsterTypes.FirstOrDefault(s => s.Value == input);
            return found.Key;
        }

        #endregion

        #region Attributes

        private static Dictionary<string, CharacterAttribute> attributes = new Dictionary<string, CharacterAttribute>
        {
            {"Unknown", CharacterAttribute.Unknown},
            {"Strength", CharacterAttribute.Strength},
            {"Dexterity", CharacterAttribute.Dexterity},
            {"Constitution", CharacterAttribute.Constitution},
            {"Intelligence", CharacterAttribute.Intelligence},
            {"Wisdom", CharacterAttribute.Wisdom},
            {"Charisma", CharacterAttribute.Charisma}
        };

        /// <summary>
        /// Retrieve an attribute by the string representation
        /// </summary>
        /// <param name="input">Value to check</param>
        /// <returns>The <see cref="CharacterAttribute"/> value, or unknown if the value isn't matched</returns>
        public static CharacterAttribute RetrieveAttribute(string input)
        {
            var result = attributes.TryGetValue(input.ToLower(), out var found);
            return result
                ? found
                : CharacterAttribute.Unknown;
        }

        /// <summary>
        /// Given a attribute this will return the appropriate string (with casing)
        /// </summary>
        /// <param name="input">an <see cref="CharacterAttribute"/></param>
        /// <returns>String representation</returns>
        public static string ConvertAttributeToString(CharacterAttribute input)
        {
            var found = attributes.FirstOrDefault(s => s.Value == input);
            return found.Key;
        }

        #endregion
    }
}