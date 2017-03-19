using System.Collections.Generic;
using System.Linq;
using AvsAnLib.Internals;

namespace WikipediaAvsAnTrieExtractorTest {
    internal class NodeEqualityComparer : IEqualityComparer<Node> {
        public static readonly NodeEqualityComparer Instance = new NodeEqualityComparer();

        public bool Equals(Node x, Node y) {
            return
                x.c == y.c
                    && x.ratio.Occurence == y.ratio.Occurence
                    && x.ratio.AminAnDiff == y.ratio.AminAnDiff
                    && (x.SortedKids == null) == (y.SortedKids == null)
                    && (x.SortedKids == null ||
                        x.SortedKids.Length == y.SortedKids.Length
                            && x.SortedKids.SequenceEqual(y.SortedKids, Instance)
                        )
                ;
        }

        public int GetHashCode(Node obj) {
            return obj.c;
        }
    }
}