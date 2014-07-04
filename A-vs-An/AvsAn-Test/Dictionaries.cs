using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvsAn_Test {
    public static class Dictionaries {
        public static IEnumerable<string> AcronymsWithUpto4Letters() {
            var letters = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (char)i).ToArray();

            return new[] { 1, 2, 3, 4 }
                .SelectMany(len =>
                    Enumerable.Repeat(letters, len)
                        .Aggregate(new[] { "" },
                            (prefixes, chars) => (
                                from prefix in prefixes
                                from suffix in chars
                                select prefix + suffix
                                ).ToArray()
                        )
                );
        }

        public static IEnumerable<string> SmallNumberStrings() {
            return Enumerable.Range(0, 100000).Select(i => i.ToString(CultureInfo.InvariantCulture));
        }

        public static string[] LoadEnglishDictionary() {
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var stream = typeof(CompleteDictionaryCheck).Assembly.GetManifestResourceStream(typeof(CompleteDictionaryCheck), "354984si.ngl"))
            using (var reader = new StreamReader(stream)) {
                return reader
                    .ReadToEnd()
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}
