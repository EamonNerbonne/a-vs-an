using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvsAnLib.Internals;

namespace WikipediaAvsAnTrieExtractor {
    public static class IncrementPrefixExtensions {
        public static void IncrementPrefix(this MutableNode node, bool isAn, string word, int level) {
            if (isAn) node.ratio.anCount++;
            else node.ratio.aCount++;

            if (level < 40)
                if (word.Length > level)
                    GetChild(node, word[level]).IncrementPrefix(isAn, word, level + 1);
                else if (word.Length == level)
                    GetChild(node, ' ').IncrementTerminator(isAn);
        }

        static void IncrementTerminator(this MutableNode node, bool isAn) {
            if (isAn) node.ratio.anCount++;
            else node.ratio.aCount++;
        }

        static MutableNode GetChild(this MutableNode node, char c) {
            if (node.Kids == null)
                node.Kids = new Dictionary<char, MutableNode>();
            MutableNode kid;
            if (!node.Kids.TryGetValue(c, out kid))
                node.Kids.Add(c, kid = new MutableNode());
            return kid;
        }
    }
}