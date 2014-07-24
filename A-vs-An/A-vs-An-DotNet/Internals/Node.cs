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


        public void LoadPrefixRatio(string prefix, int depth, Ratio prefixRatio) {
            if (prefix.Length == depth) {
                ratio = prefixRatio;
            } else {
                int idx;
                char kidC = prefix[depth];
                if (SortedKids == null) {
                    SortedKids = new[] { new Node { c = kidC, } };
                    idx = 0;
                } else {
                    var nearestIdx = FindNearestNode(kidC);
                    idx = nearestIdx;
                    if (SortedKids[nearestIdx].c != kidC) {
                        if (SortedKids[nearestIdx].c < kidC) {
                            idx = nearestIdx + 1;
                        }
                        InsertBefore(idx, kidC);
                    }
                }
                SortedKids[idx].LoadPrefixRatio(prefix, depth + 1, prefixRatio);
            }
        }

        int FindNearestNode(char c) {
            int start = 0, end = SortedKids.Length;
            while (end - start > 1) {
                int midpoint = end + start >> 1;
                if (SortedKids[midpoint].c <= c) {
                    start = midpoint;
                } else {
                    end = midpoint;
                }
            }
            return start;
        }

        void InsertBefore(int idx, char c) {
            var newArr = new Node[SortedKids.Length + 1];
            int i = 0;
            for (; i < idx; i++) {
                newArr[i] = SortedKids[i];
            }
            newArr[idx].c = c;
            for (; i < SortedKids.Length; i++) {
                newArr[i + 1] = SortedKids[i];
            }
            SortedKids = newArr;
        }
    }
}