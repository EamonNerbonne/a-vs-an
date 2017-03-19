using System.Text.RegularExpressions;
using System;
using System.Text;

namespace WikipediaAvsAnTrieExtractor
{
    public partial class RegexTextUtils {
        //Note: regexes are NOT static and shared between threads because of... http://stackoverflow.com/questions/7585087/multithreaded-use-of-regex
        //This code is bottlenecked by regexes, so this really matters, here.

        public string StripWikiMarkup(string wikiMarkedUpText) {
            var markupWithQuotesNotEmphasis = markupToReplaceWithQuotes.Replace(wikiMarkedUpText, "\"");
            var withoutNonTrivialMarkup = markupToStripRegex.Replace(markupWithQuotesNotEmphasis, "");
            var withoutBraces = CutBraces(withoutNonTrivialMarkup);
            var plainTextMarkupReplacedByContent =
                markupToReplaceRegex.Replace(withoutBraces, m => m.Groups["txt"].Value);
            return DecodeEntities(plainTextMarkupReplacedByContent);
        }

        public string DecodeEntities(string text) {
            var sb = new StringBuilder();
            var pos = 0;
            while (true) {
                var nextAmp = text.IndexOf('&', pos);
                if (nextAmp < 0) {
                    sb.Append(text, pos, text.Length - pos);
                    break;
                } else {
                    var end = nextAmp + 1;
                    while (end < text.Length &&
                        (text[end] >= 'a' && text[end] <= 'z' ||
                            text[end] >= 'A' && text[end] <= 'Z' ||
                            text[end] >= '0' && text[end] <= '9'))
                        end++;
                    if (end == text.Length) {
                        sb.Append(text, pos, end - pos);
                        break;
                    } else if (text[end] == ';' && end - nextAmp < 10) {
                        var key = text.Substring(nextAmp + 1, end - (nextAmp + 1));
                        if (HtmlEntities.EntityLookup.TryGetValue(key, out char c))
                        {
                            sb.Append(text, pos, nextAmp - pos);
                            sb.Append(c);
                        }
                        else
                        {
                            sb.Append(text, pos, end + 1 - pos);
                        }
                        pos = end + 1;
                    } else {
                        sb.Append(text, pos, end - pos);
                        pos = end;
                    }
                }
            }
            return sb.ToString();
        }

        static string CutBraces(string wikiMarkedUpText) {
            var numOpen = 0;
            var nextOpen = wikiMarkedUpText.IndexOf("{{", StringComparison.Ordinal);
            var nextClose = wikiMarkedUpText.IndexOf("}}", StringComparison.Ordinal);
            nextOpen = nextOpen == -1 ? int.MaxValue : nextOpen;
            nextClose = nextClose == -1 ? int.MaxValue : nextClose;
            var sb = new StringBuilder();
            var startAt = 0;
            while (true) {
                if (nextOpen < nextClose) {
                    if (numOpen == 0) {
                        sb.Append(wikiMarkedUpText.Substring(startAt, nextOpen - startAt));
                        sb.Append(" __ ");
                        //we replace templated content by "something" so that later interpretation
                        //doesn't mistakenly think the word right before and right after the template
                        //are actually adjacent in the text.
                    }
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

        readonly Regex markupToReplaceWithQuotes = new Regex(@"
    ''+ #For a vs. an: important that emphasis isn't around article
    |</?code>
", options);
        readonly Regex markupToStripRegex = new Regex(@"
(?>
'''*
|<(
  !--.*?-->
  |(
    [mM][aA][tT][hH]
    |[rR][eE][fF]
    |[sS][mM][aA][lL][lL]
    ).*?(
      />
      |</(
        [mM][aA][tT][hH]
        |[rR][eE][fF]
        |[sS][mM][aA][lL][lL]
        )>
      )
  |/?[a-z0-9]+(?<=[</](
h1|h2|h3|h4|h5|h6|p|br|hr|comment|abbr|b|bdi|blockquote|cite|code|data|del|dfn|em|i|ins|kbd|mark|pre|rb|rp|rt|ruby|s|samp|small|strong|sub|sup|time|u|var|wbr|dl|dt|dd|ol|ul|li|div|span|table|td|tr|th|caption|thead|tfoot|tbody|elements|big|center|font|strike|tt|noinclude|nowiki))( [^>]*)?>
  )
|=+[ ]*\r?$
|&[nN]([bB][sS][pP]|[dD][aA][sS][hH]);
|\{\|([^\|]|\|[^\}])*\|\}
|(?<=&)[aA][mM][pP];
|^(\#[rR][eE][dD][iI][rR][eE][cC][tT][^\n]*|\*+|=+)
)
", options);

        readonly Regex markupToReplaceRegex = new Regex(@"
  \[
    (
      [a-z][a-z][a-z]+://[^ \[\]]+
        ([ ](?<txt>[^\[\]]*))
      |\[(
        (?!Category:|[a-z][a-z][a-z-]*:)(?<txt>[^\[\]\|]*)
        |[^\[\]\|]*\|(?<txt>[^\[\]]*)
        |([^\[\|\]]+)
        ([^\[\]]|\[\[[^\[\]]*\]\])*   #support one level of nesting.
        )
        \]
    )
  \]
", options);
    }
}
