namespace AvsAnLib.Internals {
    public static class NodeMutation {
        public static void LoadPrefixRatio(this ref Node node, string prefix, int depth, Ratio prefixRatio) {
            if (prefix.Length == depth) {
                node.ratio = prefixRatio;
            } else {
                var kidC = prefix[depth];
                var idx = GetOrAddKidIdx(ref node, kidC);
                LoadPrefixRatio(ref node.SortedKids[idx], prefix, depth + 1, prefixRatio);
            }
        }

        public static int GetOrAddKidIdx(this ref Node node, char kidC) {
            if (node.SortedKids == null) {
                node.SortedKids = new[] { new Node { c = kidC, } }; //expensive, so many arrays.
                return 0;
            }

            var idx = IdxAfterLastLtNode(node.SortedKids, kidC);

            if (idx == node.SortedKids.Length || node.SortedKids[idx].c != kidC) {
                InsertBefore(ref node, idx, kidC);
            }

            return idx;
        }

        static int IdxAfterLastLtNode(Node[] sortedKids, char needle) {
            int start = 0, end = sortedKids.Length;
            //invariant: only LT nodes before start
            //invariant: only GTE nodes at or past end

            while (end != start) {
                var midpoint = end + start >> 1;
                // start <= midpoint < end
                if (sortedKids[midpoint].c < needle) {
                    start = midpoint + 1; //i.e.  midpoint < start1 so start0 < start1
                } else {
                    end = midpoint; //i.e end1 = midpoint so end1 < end0
                }
            }

            return end;
        }

        static void InsertBefore(ref Node node, int idx, char newC) {
            var newArr = new Node[node.SortedKids.Length + 1];
            var i = 0;
            for (; i < idx; i++) {
                newArr[i] = node.SortedKids[i];
            }

            newArr[idx].c = newC;
            for (; i < node.SortedKids.Length; i++) {
                newArr[i + 1] = node.SortedKids[i];
            }

            node.SortedKids = newArr;
        }
    }
}
