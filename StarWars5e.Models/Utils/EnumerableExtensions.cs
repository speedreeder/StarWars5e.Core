using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StarWars5e.Models.Utils
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<string> CleanListOfStrings(this IEnumerable<string> source)
        {
            var output = source.Where(s => !(s.StartsWith("<") && s.EndsWith(">")) && !s.StartsWith("\\"))
                .Select(s => Regex.Replace(s, "<.*?>", string.Empty).RemoveHtmlWhitespace())
                .ToList();

            var badIndexes = new List<int>();
            for (var i = 0; i < output.Count; i++)
            {
                var currentLine = output[i];
                var nextLine = output.ElementAtOrDefault(i + 1);

                if (nextLine != null && string.IsNullOrWhiteSpace(currentLine) && string.IsNullOrWhiteSpace(nextLine))
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
