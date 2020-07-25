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
            var node = Dictionary;
            var depth = 0;
            while (true) {
                if (word.Length == depth) {
                    return new Result(node.ratio, word, depth);
                }

                var c = word[depth];
                var needsCheck = '‘' <= c && c <= '”' || '"' <= c && c <= '-';
                if (needsCheck && (c == '"' || c == '$' || c == '\'' || c == '(' || c == '-' || c == '‘' || c == '’' || c == '“' || c == '”')) {
                    depth++;
                    continue;
                }
                break;
            }

            while (true) {
                var c = depth < word.Length ? word[depth] : ' ';
                if (node.SortedKids.TryGetValue(c, out var kid)) {
                    node = kid;
                    depth++;
                    if (depth <= word.Length && node.SortedKids != null) {
                        continue;
                    }
                }

                return new Result(node.ratio, word, depth);
            }
        }
    }
}
