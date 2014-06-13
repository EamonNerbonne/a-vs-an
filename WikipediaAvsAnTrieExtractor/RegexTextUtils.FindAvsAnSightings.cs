using System.Linq;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace WikipediaAvsAnTrieExtractor {
    public partial class RegexTextUtils {
        public AvsAnSighting[] FindAvsAnSightings(string page)
        {
            var pageTextContent = WhitespaceNormalizer.Normalize(page);
            var retval=  new List<AvsAnSighting>();
            foreach (var sentence in FindEnglishSentences(pageTextContent))
                if(GradeEnglishSentence(sentence) > 2.5)
                    foreach (var entry in ExtractWordsPrecededByAOrAn(sentence))
                        retval.Add(entry);
            return retval.ToArray();
            //return (
            //    from sentence in FindEnglishSentences(pageTextContent)
            //    where GradeEnglishSentence(sentence) > 2.5
            //    from entry in ExtractWordsPrecededByAOrAn(sentence)
            //    select entry).ToArray();
        }
    }
}
