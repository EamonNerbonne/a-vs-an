using System.Runtime.CompilerServices;
using AvsAnLib.Internals;

namespace AvsAnLib {
    public static partial class AvsAn {
        static readonly Node rootNode = BuiltInDictionary.Root;

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
            var node = rootNode;
            var depth = 0;
            var result = node.ratio;
            while (true) {
                if (depth < word.Length) {
                    var c = word[depth];
                    if (c == '"' || c == '‘' || c == '’' || c == '“' || c == '”' || c == '$' || c == '\'' || c == '-' || c == '(') {
                        depth++;
                    } else {
                        break;
                    }
                } else {
                    return new Result(result, word, depth);
                }
            }

            while (true) {
                if (node.SortedKids == null) {
                    break;
                }

                var c = depth < word.Length ? word[depth] : ' ';
                var candidateIdx = node.SortedKids.Length - 1;
                var start = 0;
                //invariant: only LT nodes before start
                //invariant: only GTE nodes at or past candidateIdx *OR* needle doesn't exist.

                while (candidateIdx != start) {
                    var midpoint = (candidateIdx + start) >> 1;
                    if (node.SortedKids[midpoint].c < c) {
                        start = midpoint + 1;
                    } else {
                        candidateIdx = midpoint;
                    }
                }

                if (node.SortedKids[candidateIdx].c != c) {
                    break;
                }

                node = node.SortedKids[candidateIdx];
                if (node.ratio.isSet) {
                    result = node.ratio;
                }

                depth++;
                if (depth > word.Length) {
                    break;
                }
            }

            return new Result(result, word, depth);
        }
    }
}
