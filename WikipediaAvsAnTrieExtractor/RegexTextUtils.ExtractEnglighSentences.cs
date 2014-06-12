using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace AvsAnTrie {
    public partial class RegexTextUtils {
        const string sentenceRegex = @"
            (?<=[.?!]\s+|^)
                    (?<sentence>
                        [\(""]?
                        (?=[A-Z])
                        (
                            [ ]
                                (
                                    [Ss]t
                                    |Mrs?
                                    |[dD]r
                                    |ed
                                    |c
                                    |v(s|ol)?
                                    |[nN]o(?=\s+[0-9])
                                    |et[ ]al
                                )\.
                            |\(\w+\.
                            |([A-Z]\.)+[ ]
                            |\.
                                (
                                    [\w\d]
                                    |[ ]
                                        (
                                            \w\.([ ]\w\.)*
                                            |[a-z]
                                        )
                                )
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
