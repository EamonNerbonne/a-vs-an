using System.Collections.Generic;

namespace AvsAnLib.Internals {
    /// <summary>
    /// A node the article lookup trie. Do not mutate after construction.
    /// </summary>
    public struct Node {
        public Dictionary<char,Node> SortedKids;
        public Ratio ratio;
    }
}
