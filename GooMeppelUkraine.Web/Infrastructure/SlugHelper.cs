using System.Text;
using System.Text.RegularExpressions;

namespace GooMeppelUkraine.Web.Infrastructure
{
    public static class SlugHelper
    {
        private static readonly Dictionary<char, string> UaMap = new()
        {
            ['а'] = "a",
            ['б'] = "b",
            ['в'] = "v",
            ['г'] = "h",
            ['ґ'] = "g",
            ['д'] = "d",
            ['е'] = "e",
            ['є'] = "ie",
            ['ж'] = "zh",
            ['з'] = "z",
            ['и'] = "y",
            ['і'] = "i",
            ['ї'] = "yi",
            ['й'] = "i",
            ['к'] = "k",
            ['л'] = "l",
            ['м'] = "m",
            ['н'] = "n",
            ['о'] = "o",
            ['п'] = "p",
            ['р'] = "r",
            ['с'] = "s",
            ['т'] = "t",
            ['у'] = "u",
            ['ф'] = "f",
            ['х'] = "kh",
            ['ц'] = "ts",
            ['ч'] = "ch",
            ['ш'] = "sh",
            ['щ'] = "shch",
            ['ю'] = "yu",
            ['я'] = "ya",
            ['ь'] = ""
        };

        public static string Generate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "article";

            var s = input.Trim().ToLowerInvariant();

            // translit UA
            var sb = new StringBuilder(s.Length);
            foreach (var ch in s)
            {
                if (UaMap.TryGetValue(ch, out var repl))
                    sb.Append(repl);
                else
                    sb.Append(ch);
            }

            s = sb.ToString();

            s = Regex.Replace(s, @"[\s_]+", "-");
            s = Regex.Replace(s, @"[^a-z0-9\-]", "");
            s = Regex.Replace(s, @"-+", "-").Trim('-');

            return string.IsNullOrWhiteSpace(s) ? "article" : s;
        }
    }
}
