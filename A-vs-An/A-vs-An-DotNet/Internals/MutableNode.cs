using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AvsAnLib.Internals {
    /// <summary>
    /// A mutable node representing an article prefix-trie during construction.
    /// </summary>
    public class MutableNode {
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


        public static MutableNode DeserializeDenseHex(string rawDict) {
            var mutableRoot = new MutableNode();
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

    }
}