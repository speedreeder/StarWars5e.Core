using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StarWars5e.Models.Utils
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<string> CleanListOfStrings(this IEnumerable<string> source, bool removeHtmlWhitespace = true)
        {
            var cleanListOfStrings = source.ToList();
            if (!cleanListOfStrings.Any()) return cleanListOfStrings;
            var output = cleanListOfStrings.Where(s =>
                    !(s.StartsWith("<") && (Regex.IsMatch(s, @">\s*$") && !s.StartsWith("<span")
                    || Regex.IsMatch(s, @"^\s+$"))) && !s.StartsWith("\\") && !Regex.IsMatch(s, @"^\s*\."))
                .Select(s =>
                    removeHtmlWhitespace
                        ? Regex.Replace(s, "<.*?>", string.Empty).RemoveHtmlWhitespace()
                        : Regex.Replace(s, "<.*?>", string.Empty))
                .Select(s =>
                {
                    if (Regex.IsMatch(s, @"\|\s*:--\s*\|"))
                    {
                        s = Regex.Replace(s, @"\s", "");
                    }
                    return s;
                })
                .ToList();

            var badIndexes = new List<int>();
            for (var i = 0; i < output.Count; i++)
            {
                var currentLine = output[i];
                var nextLine = output.ElementAtOrDefault(i + 1);

                if (nextLine != null && string.IsNullOrWhiteSpace(currentLine) && string.IsNullOrWhiteSpace(nextLine) 
                    || currentLine != null && Regex.IsMatch(currentLine, @"^>\s*$"))
                {
                    badIndexes.Add(i);
                }
            }

            var numRemoved = 0;
            foreach (var badIndex in badIndexes)
            {
                output.RemoveAt(badIndex - numRemoved);
                numRemoved++;
            }

            if (string.IsNullOrWhiteSpace(output.Last()))
            {
                output.RemoveAt(output.Count - 1);
            }

            return output;
        }

        public static IEnumerable<string> RemoveEmptyLines(this IEnumerable<string> source)
        {
            return source.Where(x => !string.IsNullOrEmpty(x));
        }

        public static T SafeAccess<T>(this T[] tArray, int index)
            where T : class
        {
            if((tArray.Length - 1) < index || index < 0)
            {
                return null;
            }

            return tArray[index];
        }
    }
}
