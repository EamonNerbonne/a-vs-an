using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Collections.Generic;

namespace WikipediaAvsAnTrieExtractor {
    public partial class RegexTextUtils {
        const string sentenceRegex = @"
(?<=[.?!]\s+|^)
[\(""]?
(?=[A-Z])
(?>
(
    [^\.\n\?!]
    |\.(
      [\w\d]
      |(?=[)""]*[ ]+[a-z])
      |(?<=
        (^|[(""\s])
        (
        \(\w+
        |c
        |[dD]r
        |e(t[ ]al|d|\.g)
        |Gov
        |i\.e
        |Lt
        |M(rs?|t)
        |[nN]o(?=\s+[0-9])
        |[Ss]t
        |[vV](s|ol)?
        |[A-Z](\.[A-Z])*
        )
        \.
        )
      )
)+
)
[.?!\n]
[)""]*
(?=\s|$)
";
        readonly Regex sentenceFinderRegex = new Regex(sentenceRegex, options);

        public IEnumerable<string> FindEnglishSentences(string text) {
            foreach (Match m in sentenceFinderRegex.Matches(text))
                yield return m.Value;
        }
    }
}
