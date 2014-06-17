using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Collections.Generic;

namespace WikipediaAvsAnTrieExtractor {
    public partial class RegexTextUtils {
        //Note: regexes are NOT static and shared because of... http://stackoverflow.com/questions/7585087/multithreaded-use-of-regex
        //This code is bottlenecked by regexes, so this really matters, here.

        readonly Regex followingAn = new Regex(@"\b(?<article>a|an) [\(""'“‘-]?(?<word>\S+)", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
        public IEnumerable<AvsAnSighting> ExtractWordsPrecededByAOrAn(string text) {
            return
                from Match m in followingAn.Matches(text)
                select new AvsAnSighting { Word = m.Groups["word"].Value + " ", PrecededByAn = m.Groups["article"].Value.Length == 2 };
        }
    }
}
