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
                    var nextKidIdx = node.GetOrAddKidIdx(word[level]);
                    IncrementPrefix(
                        ref node.SortedKids[nextKidIdx],
                        isAn, word, level + 1
                    );
                } else if (word.Length == level) {
                    var nextKidIdx = node.GetOrAddKidIdx(' ');
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

        static void IncAn(this ref Ratio ratio) {
            ratio.Occurrence++;
            ratio.AminAnDiff--;
        }

        static void IncA(this ref Ratio ratio) {
            ratio.Occurrence++;
            ratio.AminAnDiff++;
        }
    }
}
