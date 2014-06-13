using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace WikipediaAvsAnTrieExtractor {
    public partial class RegexTextUtils {
        //Note: regexes are NOT static and shared between threads because of... http://stackoverflow.com/questions/7585087/multithreaded-use-of-regex
        //This code is bottlenecked by regexes, so this really matters, here.


        public string StripWikiMarkupMarkup(string wikiMarkedUpText) {
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
    }
}
