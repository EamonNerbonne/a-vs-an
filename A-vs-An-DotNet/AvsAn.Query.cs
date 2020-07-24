using AvsAnLib.Internals;

namespace AvsAnLib {
    public static partial class AvsAn {
        static readonly Node Dictionary = BuiltInDictionary.Dictionary;

        /// <summary>
        /// Determines whether an english word should be preceded by the indefinite article "a" or "an".
        /// By Eamon Nerbonne; feedback can be reported to https://github.com/EamonNerbonne/a-vs-an/
        /// </summary>
        /// <param name="word">
        /// The word to test.  AvsAn assumes this is a complete word; in some cases word-prefixes may result in
        /// a differing classification that complete words.  If you wish to classify an incomplete word (a prefix), append a
        /// non-word, non-space character such as the underscore "_" as a placeholder for further letters.
        /// </param>
        /// <returns>A classification result indicating "a" or "an" with some wikipedia-derived statistics.</returns>
        public static Result Query(string word) {
            var kids = Dictionary.SortedKids;
            var result = Dictionary.ratio;
            var depth = 0;
            while (true) {
                if (word.Length == depth) {
                    return new Result(result, word, depth);
                }

                var c = word[depth];
                var needsCheck = '‘' <= c && c <= '”' || '"' <= c && c <= '-';
                if (needsCheck && (c == '"'|| c == '$' || c == '\'' || c == '(' || c == '-' || c == '‘' || c == '’' || c == '“' || c == '”' )) {
                    depth++;
                    continue;
                }

                break;
            }

            while (true) {
                var c = depth < word.Length ? word[depth] : ' ';

                var firstIdx = 0;
                var lastIdx = kids.Length - 1;

                //We start with a binary search while the search space is "large" (14 elements or more)
                //invariant: only LT nodes before firstIdx
                //invariant: only GTE nodes at or past lastIdx *OR* needle doesn't exist.

                while (13 <= lastIdx - firstIdx) {
                    var midpoint = (lastIdx + firstIdx) >> 1;
                    if (kids[midpoint].c < c) {
                        firstIdx = midpoint + 1;
                    } else {
                        lastIdx = midpoint;
                    }
                }

                //With fewer than 14 elements, do a plain iterative scan.
                while (true) {
                    if (kids[firstIdx].c != c) {
                        firstIdx++;
                        if (firstIdx > lastIdx) {
                            return new Result(result, word, depth);
                        }
                    } else {
                        if (kids[firstIdx].ratio.isSet) {
                            result = kids[firstIdx].ratio;
                        }

                        kids = kids[firstIdx].SortedKids;

                        depth++;
                        if (depth > word.Length || kids == null) {
                            return new Result(result, word, depth);
                        }

                        break;
                    }
                }
            }
        }
    }
}
