using System;
using System.Collections.Generic;

namespace AvsAnLib {
    class MutableNode {
        //static readonly Node[] EmptyNodeArr = new Node[0];
        internal Ratio ratio;
        internal Dictionary<char, MutableNode> Kids;
        
        public void LoadPrefixRatio(string prefix, int depth, Ratio prefixRatio) {
            if (prefix.Length == depth) {
                ratio = prefixRatio;
            } else {
                MutableNode kid;
                if (Kids == null)
                    Kids = new Dictionary<char, MutableNode>();
                if (!Kids.TryGetValue(prefix[depth], out kid))
                    Kids[prefix[depth]] = kid = new MutableNode();
                kid.LoadPrefixRatio(prefix, depth + 1, prefixRatio);
            }
        }
        public Node Finish(char key) {
            Node[] sortedKids = null;
            if (Kids != null) {
                sortedKids = new Node[Kids.Count];
                int i = 0;
                foreach (var kv in Kids)
                    sortedKids[i++] = kv.Value.Finish(kv.Key);
                Array.Sort(sortedKids);
            }
            return new Node { c = key, ratio = ratio, SortedKids = sortedKids };
        }
    }
}