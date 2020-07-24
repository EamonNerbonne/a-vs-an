using AvsAnLib.Internals;

namespace AvsAnLib {
    public static partial class AvsAn {
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
            var length = word.Length;
            ref readonly var node = ref BuiltInDictionary.Root();
            var depth = 0;
            var result = node.ratio;
            while (true) {
                if (length == depth) {
                    return new Result(result, word, depth);
                }

                var c = word[depth];

                if (c == '"' || c == '‘' || c == '’' || c == '“' || c == '”' || c == '$' || c == '\'' || c == '-' || c == '(') {
                    depth++;
                    continue;
                }

                break;
            }

            while (true) {
                var c = depth < length ? word[depth] : ' ';
                var lastIdx = node.SortedKids.Length - 1;
                var firstIdx = 0;
                //invariant: only LT nodes before start
                //invariant: only GTE nodes at or past candidateIdx *OR* needle doesn't exist.

                while (13 < lastIdx - firstIdx) {
                    var midpoint = (lastIdx + firstIdx) >> 1;
                    if (node.SortedKids[midpoint].c < c) {
                        firstIdx = midpoint + 1;
                    } else {
                        lastIdx = midpoint;
                    }
                }

                while (true) {
                    if (node.SortedKids[firstIdx].c != c) {
                        firstIdx++;
                        if (firstIdx > lastIdx) {
                            return new Result(result, word, depth);
                        }
                    } else {
                        node = ref node.SortedKids[firstIdx];

                        if (node.ratio.isSet) {
                            result = node.ratio;
                        }

                        depth++;
                        if (depth > length || node.SortedKids == null) {
                            return new Result(result, word, depth);
                        }

                        break;
                    }
                }
            }
        }
    }
}
