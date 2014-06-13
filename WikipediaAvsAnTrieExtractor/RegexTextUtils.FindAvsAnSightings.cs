using System.Linq;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace WikipediaAvsAnTrieExtractor {
    public partial class RegexTextUtils {
        public AvsAnSighting[] FindAvsAnSightings(XElement page)
        {
            var pageTextContent = GetPagePlainTextContent(page);
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

        string GetPagePlainTextContent(XElement page)
        {
//Don't use an explicit namespace.  It's changed several times in the past, and it's never been necessary to update the query.
            //static readonly XNamespace ns = XNamespace.Get("http://www.mediawiki.org/xml/export-0.8/");

            var textEl =
                page.Elements()
                    .Where(el => el.Name.LocalName == "revision")
                    .SelectMany(el => el.Elements())
                    .FirstOrDefault(el => el.Name.LocalName == "text");
            string pagetext = StripWikiMarkupMarkup(textEl == null ? null : textEl.Value);

            string pageWSnormal = WhitespaceNormalizer.Normalize(pagetext);
            return pageWSnormal;
        }
    }
}
