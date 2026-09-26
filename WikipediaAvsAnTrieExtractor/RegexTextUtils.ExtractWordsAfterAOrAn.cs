using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace WikipediaAvsAnTrieExtractor {
    public partial class RegexTextUtils {
        //Note: regexes are NOT static and shared because of... http://stackoverflow.com/questions/7585087/multithreaded-use-of-regex
        //This code is bottlenecked by regexes, so this really matters, here.


        //If you change this, keep WordQuery char-skipping in Query in sync!
        readonly Regex followingAn = new Regex(@"(^(?<article>An?)|[\s""()‘’“”](?<article>an?)) [""‘’“”$']*(?<word>[^\s""()‘’“”$-]+(?<!'))", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);
        
        //general notes:
        //words consist of anything BUT spaces and delimiters "()‘’“”-
        //dash is a word delimiter in pronunciation, which is all we care about here
        //$ isn't a delimiter, but its an unpronounced prefix ($3 is "three dollars"), so 
        //we exclude $ from words too.
        
        //Some tricky corner-cases:
        //watch out for dashes *before* "A" because of things like "Triple-A annotation"
        //Be careful of words like Chang'an - ' is not a separator here.
        //Prefer a few false negatives over false positives when it comes to article detection.
        //In particular, some symbolic math and logic expressions use the word "a" followed by things
        //like parentheses and that throws the statistics a little (but enough in rarely occuring 
        //prefixes to matter).  Therefore, don't detect a/an + word when separated by "(".
        //English never uses "an" before a d-sound (not even for the letter: "dee"), yet wikipedia has thousands of such sightings:
        //mostly German ("Frankfurt an der Oder", "Theater an der Wien"), some Gaelic ("Rubha an Dùnain"), and typos.
        IEnumerable<AvsAnSighting> ExtractWordsPrecededByAOrAn(string text) {
            //TODO: ignore uppercase "A" -it's just too hard to get right.
            return
                from Match m in followingAn.Matches(text)
                let word = m.Groups["word"].Value
                let precededByAn = m.Groups["article"].Value.Length == 2
                where !(precededByAn && (word[0] == 'd' || word[0] == 'D'))
                select new AvsAnSighting { Word = word, PrecededByAn = precededByAn };
        }
    }
}
