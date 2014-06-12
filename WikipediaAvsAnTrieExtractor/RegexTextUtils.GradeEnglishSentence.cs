using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Collections.Generic;

namespace AvsAnTrie {
    public partial class RegexTextUtils {

        readonly Regex words = new Regex(@"(?<=(^|\s)[^\w\s]*)\w+", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
        readonly Regex capwordsAfterFirst = new Regex(@" [^\w\s]*[A-Z]\S*", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);

        public double GradeEnglishSentence(string sentenceCandidate) {
            int capCount = sentenceCandidate.Count(c => c >= 'A' && c <= 'Z');
            int numCount = sentenceCandidate.Count(c => c >= '0' && c <= '9');//CharUnicodeInfo.GetUnicodeCategory(capCount) 
            int wordCharCount = capCount + sentenceCandidate.Count(c => c >= 'a' && c <= 'z');
            var wordMatches = words.Matches(sentenceCandidate);
            int wordCount = wordMatches.Count;

            double pref = 0.0;
            if (dictionary != null) {
                int inDictCount = 0;
                int ignore;
                foreach (Match m in wordMatches) {
                    var word = m.Value;
                    inDictCount +=
                        dictionary.Contains(m.Value) ? 2
                        : int.TryParse(m.Value, out ignore) ? 1 //numbers aren't quite valid words in the dictionary, but they're not nonsense either.
                        : word[0] >= 'A' && word[0] <= 'Z' ? 1 //don't quite expect proper nouns to be in the dictionary.
                        : 0;
                }
                pref = (inDictCount - wordCount) / (double)wordCount;
            }
            int capWordCount = capwordsAfterFirst.Matches(sentenceCandidate).Count;
            int charCount = sentenceCandidate.Length;

            double lineCost = sentenceCandidate.Length == 0 || sentenceCandidate[sentenceCandidate.Length - 1] == '\n' ? -0.4 : 0.0; //if they don't end with punctuation... hmmm.

            double capRate = wordCount == 1 ? 0.5 :
                capWordCount / (double)(wordCount - 1);

            double grade = (wordCharCount - numCount - capCount) / (double)charCount
                           + 0.3 * Math.Min(wordCount - capWordCount, 6)
                           - 0.3 * capRate
                           + pref
                           + lineCost;
            return grade;
        }
    }
}
