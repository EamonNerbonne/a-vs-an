namespace AvsAnLib.Internals {
    /// <summary>
    /// A node the article lookup trie. Do not mutate after construction.
    /// </summary>
    public struct Node {
        public Node[] SortedKids;
        public Ratio ratio;
        public char c;
    }
}
