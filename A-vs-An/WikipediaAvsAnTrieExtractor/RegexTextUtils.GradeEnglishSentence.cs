using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;
using System;
using System.Collections.Generic;

namespace WikipediaAvsAnTrieExtractor {
    public partial class RegexTextUtils {
        private static readonly bool[] isSeparatorChar =
            Enumerable.Range(0, 65536)
                .Select(c => CharUnicodeInfo.GetUnicodeCategory((char)c))
                .Select(cat =>
                    cat == UnicodeCategory.Control ||
                    cat >= UnicodeCategory.SpaceSeparator && cat <= UnicodeCategory.ParagraphSeparator
                    || cat >= UnicodeCategory.ConnectorPunctuation && cat <= UnicodeCategory.OtherPunctuation
                ).ToArray();

        public double GradeEnglishSentence(string sentenceCandidate) {
            int charCount = sentenceCandidate.Length;
            int capCount = 0, numCount = 0, wordCharCount = 0;
            int inDictScore = 0;
            int capWordCount = 0, wordCount = 0, spaceCount = 0;
            bool seenWord = false;
            bool inWord = false;
            int wordStart = 0;
            foreach (var c in sentenceCandidate) {
                if (c >= 'A' && c <= 'Z')
                    capCount++;
                else if (c >= '0' && c <= '9')
                    numCount++;
                else if (c >= 'a' && c <= 'z')
                    wordCharCount++;
                else if (c == ' ')
                    spaceCount++;
            }

            for (int i = 0; i <= sentenceCandidate.Length; i++) {
                var shouldBeInWord = i < sentenceCandidate.Length
                    && (!isSeparatorChar[sentenceCandidate[i]]
                    || inWord && sentenceCandidate[i] == '\'');
                if (inWord == shouldBeInWord) continue;
                if (!inWord) {
                    inWord = true;
                    wordStart = i;
                } else {
                    inWord = false;
                    var word = sentenceCandidate.Substring(wordStart, i - wordStart);
                    int ignore;
                    inDictScore +=
                        dictionary.Contains(word) ? 2
                        : int.TryParse(word, out ignore) ? 1
                        //numbers aren't quite valid words in the dictionary, but they're not nonsense either.
                        : word[0] >= 'A' && word[0] <= 'Z' ? 1 //don't quite expect proper nouns to be in the dictionary.
                        : 0;
                    wordCount++;
                    if (seenWord) {
                        if (word[0] >= 'A' && word[0] <= 'Z')
                            capWordCount++;
                    } else
                        seenWord = true;
                }
            }
            wordCharCount += capCount;

            double pref = (inDictScore - wordCount) / (double)wordCount;

            double lineCost = sentenceCandidate.Length == 0 || sentenceCandidate[sentenceCandidate.Length - 1] == '\n' ? -0.4 : 0.0; //if they don't end with punctuation... hmmm.

            double capRate = wordCount == 1 ? 0.5 :
                capWordCount / (double)(wordCount - 1);

            double grade = (
                (wordCharCount - numCount - capCount) / (double)charCount
                           + 0.3 * Math.Min(wordCount - capWordCount, 6)
                           - 0.3 * capRate
                           + pref
                           + lineCost
                           + 0.75 * Math.Tanh(0.5 * (spaceCount - 3.0))
                           ) / 3.0;
            return grade;
        }
    }
}
