using System.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace WikipediaAvsAnTrieExtractor {
    public partial class RegexTextUtils
    {
        private readonly Regex maybeAcronymError = new Regex(
            @"\W[a-zA-Z]{0,4}\.$"
            , options | RegexOptions.RightToLeft);
        readonly HashSet<string> alreadySeen = new HashSet<string>(); 

        public AvsAnSighting[] FindAvsAnSightings(string page)
        {
            var strippedPage = StripWikiMarkup(page);
            var plaintextPage = WhitespaceNormalizer.Normalize(strippedPage);
            var retval=  new List<AvsAnSighting>();
            var sNew = FindEnglishSentences(plaintextPage).ToArray();
            var sOld = FindEnglishSentencesOld(plaintextPage).ToArray();
            if (!sNew.SequenceEqual(sOld)) {
                var onlyNew = sNew.Except(sOld).ToArray();
                var onlyOld = sOld.Except(sNew).ToArray();
                Console.Write('.');
            }


            foreach (var sentence in sNew)
            {
                //var mightBeWrongEnding = maybeAcronymError.Match(sentence);

                //if (mightBeWrongEnding.Success && alreadySeen.Add(mightBeWrongEnding.Value)) {
                //    Console.WriteLine(mightBeWrongEnding.Value);
                //}
                if (GradeEnglishSentence(sentence) > 2.5)
                    foreach (var entry in ExtractWordsPrecededByAOrAn(sentence))
                        retval.Add(entry);
            }
            return retval.ToArray();
            //return (
            //    from sentence in FindEnglishSentences(pageTextContent)
            //    where GradeEnglishSentence(sentence) > 2.5
            //    from entry in ExtractWordsPrecededByAOrAn(sentence)
            //    select entry).ToArray();
        }
    }
}
