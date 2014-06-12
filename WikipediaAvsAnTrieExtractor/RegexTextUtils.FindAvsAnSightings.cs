using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AvsAnTrie {
    public partial class RegexTextUtils {
        public AvsAnSighting[] FindAvsAnSightings(XElement page) {
            string pagetext = StripWikiMarkupMarkup(WikiXmlReader.GetArticleText(page));
            string pageWSnormal = WhitespaceNormalizer.Normalize(pagetext);

            return (
                from sentence in FindEnglishSentences(pageWSnormal)
                where GradeEnglishSentence(sentence) > 2.5
                from entry in ExtractWordsPrecededByAOrAn(sentence)
                select entry).ToArray();
        }
    }
}
