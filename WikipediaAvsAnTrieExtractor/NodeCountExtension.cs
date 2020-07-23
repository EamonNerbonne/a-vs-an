using AvsAnLib.Internals;

namespace WikipediaAvsAnTrieExtractor {
    public static class NodeCountExtension {
        public static int Count(this Node node) {
            var count = 1;
            if (node.SortedKids != null) {
                foreach (var kid in node.SortedKids) {
                    count += Count(kid);
                }
            }

            return count;
        }
    }
}
