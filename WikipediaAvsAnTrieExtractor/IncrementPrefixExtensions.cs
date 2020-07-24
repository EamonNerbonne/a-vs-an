using AvsAnLib.Internals;

namespace WikipediaAvsAnTrieExtractor {
    public static class IncrementPrefixExtensions {
        public static void IncrementPrefix(ref Node node, bool isAn, string word, int level) {
            if (isAn) {
                node.ratio.IncAn();
            } else {
                node.ratio.IncA();
            }

            if (level < 40) {
                if (word.Length > level) {
                    var nextKidIdx = Node.GetOrAddKidIdx(ref node, word[level]);
                    IncrementPrefix(
                        ref node.SortedKids[nextKidIdx],
                        isAn, word, level + 1
                    );
                } else if (word.Length == level) {
                    var nextKidIdx = Node.GetOrAddKidIdx(ref node, ' ');
                    IncrementTerminator(
                        ref node.SortedKids[nextKidIdx],
                        isAn
                    );
                }
            }
        }

        static void IncrementTerminator(ref Node node, bool isAn) {
            if (isAn) {
                node.ratio.IncAn();
            } else {
                node.ratio.IncA();
            }
        }
    }
}
