using System;

namespace AvsAnLib.Internals {
    /// <summary>
    /// A node the article lookup trie. Do not mutate after construction.
    /// </summary>
    public struct Node : IComparable<Node> {
        public char c;
        public Ratio ratio;
        public Node[] SortedKids;
        public int CompareTo(Node other) { return c.CompareTo(other.c); }

        public static Node CreateFromMutable(MutableNode node, char key=' ') {
            Node[] sortedKids = null;
            if (node.Kids != null) {
                sortedKids = new Node[node.Kids.Count];
                int i = 0;
                foreach (var kv in node.Kids)
                    sortedKids[i++] = CreateFromMutable(kv.Value, kv.Key);
                Array.Sort(sortedKids);
            }
            return new Node { c = key, ratio = node.ratio, SortedKids = sortedKids };
        }


    }
}