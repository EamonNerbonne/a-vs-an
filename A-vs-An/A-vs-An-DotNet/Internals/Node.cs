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
    }
}