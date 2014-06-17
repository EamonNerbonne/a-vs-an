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
(
    ((?<=[(""\s])|^)(
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
    )\.
    |[^\.\n\?!]
    |\.([\w\d]|(?=[)""]*[ ]+[a-z]))
)+
[.?!\n]
[)""]*
(?=\s|$)
";
        readonly Regex sentenceFinderRegex = new Regex(sentenceRegex, options | RegexOptions.IgnorePatternWhitespace);
        //readonly Regex oldSentenceFinderRegex = new Regex(@"(?<=[\.\?!]\s+|^)((?<sentence>(\(|" + "\"" + @")?[A-Z]( ([Ss]t|Mrs?|dr|ed|c|v(s|ol)?|[nN]o(?=\s+[0-9])|et al)\.|\(\w+\.|[A-Z]\. |\.([\w\d]| (\w\.( \w\.)*|[a-z]))|[^\.\n\?!])+[\.\?!\n](\)|" + "\"" + @")?))(?=\s|$)", options);
        
//        readonly Regex oldSentenceFinderRegex = new Regex(@"
//            (?<=[.?!]\s+|^)
//                (?<sentence>
//                    [\(""]?
//                    (?=[A-Z])
//                    (
//                        (\s|^)(
//                            \(\w+
//                            |c
//                            |[dD]r
//                            |e(t[ ]al|d|\.g)
//                            |Gov
//                            |i\.e
//							|Lt
//                            |M(rs?|t)
//                            |[nN]o(?=\s+[0-9])
//                            |[Ss]t
//                            |[vV](s|ol)?
//                            |[A-Z](\.[A-Z])*
//                        )\.
//                        |\.[\w\d]
//                        |[^\.\n\?!]
//                    )+
//                    [.?!\n]
//                    [)""]*
//                )
//            (?=\s|$)", options | RegexOptions.IgnorePatternWhitespace);

        //public IEnumerable<string> FindEnglishSentencesOld(string text) {
        //    foreach (Match m in oldSentenceFinderRegex.Matches(text))
        //        yield return m.Groups["sentence"].Value;
        //}

        public IEnumerable<string> FindEnglishSentences(string text)
        {
            foreach (Match m in sentenceFinderRegex.Matches(text))
                yield return m.Value;
        }
    }
}
