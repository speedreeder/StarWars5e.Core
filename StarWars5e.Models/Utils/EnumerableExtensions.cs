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
                    !(s.StartsWith("<") && (Regex.IsMatch(s, @">\s*$") || Regex.IsMatch(s, @"^\s+$"))) && !s.StartsWith("\\"))
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
    }
}
