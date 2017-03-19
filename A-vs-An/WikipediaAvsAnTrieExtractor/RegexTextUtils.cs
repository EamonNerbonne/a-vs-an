using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;

namespace WikipediaAvsAnTrieExtractor {
    public partial class RegexTextUtils {
        //Note: regexes are NOT static and shared between threads because of... http://stackoverflow.com/questions/7585087/multithreaded-use-of-regex
        //This code is bottlenecked by regexes, so this really matters, here.
        const RegexOptions options = RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace;

        static readonly Regex firstLetter = new Regex(@"(?<=^[^\s\w]*)\w", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
        static string Capitalize(string word) {
            return firstLetter.Replace(word, m => m.Value.ToUpperInvariant());
        }

        static readonly HashSet<string> dictionary;

        static IEnumerable<string> ReadWordsFromDictionary(TextReader reader) {
            for (var line = reader.ReadLine(); line != null; line = reader.ReadLine()) {
                var word = line.Trim();
                yield return word;
                yield return Capitalize(word);
            }
        }

        static RegexTextUtils() {
            var myType = typeof(RegexTextUtils);
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var dictStream = myType.Assembly.GetManifestResourceStream(myType.Namespace + ".english.ngl"))
            using (var reader = new StreamReader(dictStream))
                dictionary = new HashSet<string>(ReadWordsFromDictionary(reader));
        }
    }
}
