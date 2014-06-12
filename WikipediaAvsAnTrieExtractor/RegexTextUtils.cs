using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using EmnExtensions.Filesystem;

namespace AvsAnTrie {
    public struct Entry {
        public string Word;
        public bool PrecededByAn;
    }

    public class RegexTextUtils {
        readonly Regex followingAn = new Regex(@"\b(?<article>a|an) [\(""'“‘-]?(?<word>\S+)", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
        public IEnumerable<Entry> ExtractWordsPrecededByAOrAn(string text) {
            return
                from Match m in followingAn.Matches(text)
                select new Entry { Word = m.Groups["word"].Value + " ", PrecededByAn = m.Groups["article"].Value.Length == 2 };
        }

        public string StripMarkup(string wikiMarkedUpText) {
            return markupToReplaceRegex.Replace(CutBraces(markupToStripRegex.Replace(wikiMarkedUpText, "")), m => m.Groups["txt"].Value);
        }

        static string CutBraces(string wikiMarkedUpText) {
            int numOpen = 0;
            int nextOpen = wikiMarkedUpText.IndexOf("{{", StringComparison.Ordinal);
            int nextClose = wikiMarkedUpText.IndexOf("}}", StringComparison.Ordinal);
            nextOpen = nextOpen == -1 ? int.MaxValue : nextOpen;
            nextClose = nextClose == -1 ? int.MaxValue : nextClose;
            var sb = new StringBuilder();
            int startAt = 0;
            while (true) {
                if (nextOpen < nextClose) {
                    if (numOpen == 0)
                        sb.Append(wikiMarkedUpText.Substring(startAt, nextOpen - startAt));
                    numOpen++;
                    nextOpen = wikiMarkedUpText.IndexOf("{{", nextOpen + 2, StringComparison.Ordinal);
                    nextOpen = nextOpen == -1 ? int.MaxValue : nextOpen;
                } else if (nextClose < nextOpen) {
                    //if (numOpen == 0) 
                    //throw new Exception("Invalid Wiki Text: unbalanced braces");
                    numOpen = Math.Max(numOpen - 1, 0);
                    if (numOpen == 0)
                        startAt = nextClose + 2;
                    nextClose = wikiMarkedUpText.IndexOf("}}", nextClose + 2, StringComparison.Ordinal);
                    nextClose = nextClose == -1 ? int.MaxValue : nextClose;
                } else if (numOpen == 0) { // nextOpen==nextClose implies both are not present.
                    sb.Append(wikiMarkedUpText.Substring(startAt, wikiMarkedUpText.Length - startAt));
                    break;
                } else {
                    //	throw new Exception("Invalid Wiki Text: unbalanced braces");
                    break;
                }
            }

            return sb.ToString();
        }

        const RegexOptions options = RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.CultureInvariant;

        static readonly string[] markupToStripRegexes = {
            @"(?>'')'*",
            @"(?><)(!--([^-]|-[^-]|--[^>])*-->|([mM][aA][tT][hH]|[rR][eE][fF]|[sS][mM][aA][lL][lL]).*?(/>|</([mM][aA][tT][hH]|[rR][eE][fF]|[sS][mM][aA][lL][lL])>))",
            @"^((?>#)[rR][eE][dD][iI][rR][eE][cC][tT].*$|(?>\*)\**|(?>=)=*)",
            @"(?<=&)(?>[aA])[mM][pP];",
            @"&(?>[nN])([bB][sS][pP]|[dD][aA][sS][hH]);",
            @"=+ *$",
            @"\{\|([^\|]|\|[^\}])*\|\}",
            @"</?[a-zA-Z]+( [^>]*?)?/?>",
        };
        readonly Regex markupToStripRegex = new Regex(string.Join("|", markupToStripRegexes.Select(r => "(" + r + ")").ToArray()), options);

        static readonly string[] markupToReplaceRegexes = {
            @"(?>\[\[([^\[:\|\]]+):)([^\[\]]|\[\[[^\[\]]*\]\])*\]\]",
			@"\[([^ \[\]]+( (?<txt>[^\[\]]*))?|\[((?<txt>:[^\[\]]*)|(?<txt>[^\[\]:\|]*)|[^\[\]:\|]*\|(?<txt>[^\[\]]*))\])\]",
        };
        readonly Regex markupToReplaceRegex = new Regex(string.Join("|", markupToReplaceRegexes.Select(r => "(" + r + ")").ToArray()), options);

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
                where m.Success
                select m.Groups["sentence"].Value;
        }

        static readonly Regex firstLetter = new Regex(@"(?<=^[^\s\w]*)\w", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
        static string Capitalize(string word) {
            return firstLetter.Replace(word, m => m.Value.ToUpperInvariant());
        }

        static HashSet<string> dictionary;
        public static void LoadDictionary(FileInfo fileInfo) {
            var dictElems =
                File.ReadAllLines(fileInfo.FullName)
                    .Select(line => line.Trim())
                    .SelectMany(word => new[] { word, Capitalize(word) });
            dictionary = new HashSet<string>(dictElems);
        }

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
