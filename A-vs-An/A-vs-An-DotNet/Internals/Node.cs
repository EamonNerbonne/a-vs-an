using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AvsAnLib.Internals {
    /// <summary>
    /// A node the article lookup trie. Do not mutate after construction.
    /// </summary>
    public struct Node : IComparable<Node> {
        public char c;
        public Ratio ratio;
        public Node[] SortedKids;
        public int CompareTo(Node other) { return c.CompareTo(other.c); }

        public static Node DeserializeDenseHex(string rawDict) {
            var mutableRoot = new Node();
            foreach (
                Match m in Regex.Matches(rawDict, @"([^\[]*)\[([0-9a-f]*):([0-9a-f]*)\]", RegexOptions.CultureInvariant)
                )
                mutableRoot.LoadPrefixRatio(
                    m.Groups[1].Value,
                    0,
                    new Ratio {
                        aCount = parseHex(m.Groups[2].Value),
                        anCount = parseHex(m.Groups[3].Value)
                    });
            return mutableRoot;
        }
        static int parseHex(string str) {
            return str == "" ? 0 : int.Parse(str, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
        }

        public void LoadPrefixRatio(string prefix, int depth, Ratio prefixRatio) {
            if (prefix.Length == depth) {
                ratio = prefixRatio;
            } else {
                char kidC = prefix[depth];
                var idx = GetOrAddKidIdx(kidC);
                SortedKids[idx].LoadPrefixRatio(prefix, depth + 1, prefixRatio);
            }
        }

        public int GetOrAddKidIdx(char kidC) {
            if (SortedKids == null) {
                SortedKids = new[] { new Node { c = kidC, } };
                return 0;
            } else {
                int idx = IdxAfterLastLtNode(kidC);

                if (idx == SortedKids.Length || SortedKids[idx].c != kidC)
                    InsertBefore(idx, kidC);
                return idx;
            }
        }

        int IdxAfterLastLtNode(char needle) {
            int start = 0, end = SortedKids.Length;
            //invariant: only LT nodes before start
            //invariant: only GTE nodes at or past end

            while (end != start) {
                int midpoint = end + start >> 1;
                // start <= midpoint < end
                if (SortedKids[midpoint].c < needle) {
                    start = midpoint + 1;//i.e.  midpoint < start1 so start0 < start1
                } else {
                    end = midpoint;//i.e end1 = midpoint so end1 < end0
                }
            }
            return end;
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