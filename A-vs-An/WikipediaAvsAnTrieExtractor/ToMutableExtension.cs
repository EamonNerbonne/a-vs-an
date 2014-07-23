using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvsAnLib.Internals;

namespace WikipediaAvsAnTrieExtractor {
    public static class ToMutableExtension {
        public static MutableNode ToMutableNode(this Node node) {
            return new MutableNode {
                ratio = node.ratio,
                Kids = node.SortedKids == null ? null :
                    node.SortedKids.ToDictionary(n => n.c, ToMutableNode)
            };
        }
    }
}
