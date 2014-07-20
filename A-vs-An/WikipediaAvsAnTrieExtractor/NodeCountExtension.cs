using AvsAnLib.Internals;

namespace WikipediaAvsAnTrieExtractor {
    public static class NodeCountExtension {
        public static int Count(this MutableNode node) {
            int count = 1;
            if (node.Kids != null)
                foreach (var kid in node.Kids.Values)
                    count += Count(kid);
            return count;
        }

        public static int Count(this Node node) {
            int count = 1;
            if (node.SortedKids != null)
                foreach (var kid in node.SortedKids)
                    count += Count(kid);
            return count;
        }
    }
}