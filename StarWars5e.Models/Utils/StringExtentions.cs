namespace StarWars5e.Models.Utils
{
    public static class StringExtentions
    {
        public static string RemoveHtmlWhitespace(this string input)
        {
            return input.Replace("&emsp;", string.Empty).Replace("&nbsp;", string.Empty);
        }

        public static string RemoveMarkdownCharacters(this string input)
        {
            return input.Replace(">", string.Empty).Replace("*", string.Empty);
        }
    }
}
