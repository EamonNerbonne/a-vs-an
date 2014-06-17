using System.Linq;
using System;
using System.Collections.Generic;

namespace WikipediaAvsAnTrieExtractor {
    public partial class RegexTextUtils
    {
        public AvsAnSighting[] FindAvsAnSightings(string page)
        {
            var strippedPage = StripWikiMarkup(page);
            var plaintextPage = WhitespaceNormalizer.Normalize(strippedPage);
            var retval=  new List<AvsAnSighting>();

            foreach (var sentence in FindEnglishSentences(plaintextPage))
            {
                if (GradeEnglishSentence(sentence) > 2.5)
                    foreach (var entry in ExtractWordsPrecededByAOrAn(sentence))
                        retval.Add(entry);
            }
            return retval.ToArray();
        }
    }
}
