using System.Collections.Generic;
using System.Linq;
using AvsAnLib.Internals;

namespace WikipediaAvsAnTrieExtractorTest {
    class NodeEqualityComparer : IEqualityComparer<Node> {
        public static readonly NodeEqualityComparer Instance = new NodeEqualityComparer();

        public bool Equals(Node x, Node y)
            => x.c == y.c
                && x.ratio.Occurrence == y.ratio.Occurrence
                && x.ratio.AminAnDiff == y.ratio.AminAnDiff
                && (x.SortedKids == null) == (y.SortedKids == null)
                && (x.SortedKids == null ||
                    x.SortedKids.Length == y.SortedKids.Length
                    && x.SortedKids.SequenceEqual(y.SortedKids, Instance)
                );

        public int GetHashCode(Node obj)
            => obj.c;
    }
}
