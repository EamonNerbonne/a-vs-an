using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace WikipediaAvsAnTrieExtractor {
    public partial class RegexTextUtils {
        const string sentenceRegex = @"
            (?<=[.?!]\s+|^)
                (?<sentence>
                    [\(""]?
                    (?=[A-Z])
                    (
                        (
                            [Ss]t
                            |Mrs?
                            |[dD]r
                            |Mt
                            |ed
                            |c
                            |v(s|ol)?
                            |[nN]o(?=\s+[0-9])
                            |et[ ]al
                            |[A-Z](\.[A-Z])*
                            |\(\w+
                        )\.[ ]
                        |\.[\w\d]
                        |[^\.\n\?!]
                    )+
                    [.?!\n]
                    [)""]?
                )
            (?=\s|$)";
        readonly Regex sentenceFinderRegex = new Regex(sentenceRegex, options | RegexOptions.IgnorePatternWhitespace);

        public IEnumerable<string> FindEnglishSentences(string text) {
            return
                from Match m in sentenceFinderRegex.Matches(text)
                select m.Groups["sentence"].Value;
        }
    }
}
