using System;
using System.Collections.Generic;
using AvsAnLib.Internals;

namespace WikipediaAvsAnTrieExtractor {
    public static class SimplifyExtension {
        public static MutableNode Simplify(this MutableNode node, int scaleFactor) {
            Dictionary<char, MutableNode> simpleKids = null;
            if (node.Kids != null)
                foreach (var kidEntry in node.Kids) {
                    var kid = kidEntry.Value;
                    var diff = kid.ratio.anCount - kid.ratio.aCount;
                    var occurence = kid.ratio.anCount + kid.ratio.aCount;
                    if (Math.Abs(occurence) < scaleFactor)
                        continue;
                    var simpleKid = kid.Simplify(scaleFactor);
                    if (simpleKid.Kids != null ||
                        diff*(long) diff >= scaleFactor*(long) occurence
                        //&& simpleKid.Annotation() != node.Annotation()
                        ) {
                        simpleKids = simpleKids ?? new Dictionary<char, MutableNode>();
                        simpleKids.Add(kidEntry.Key, simpleKid);
                    }
                }
            return new MutableNode {
                ratio = node.ratio,
                Kids = simpleKids
            };
        }
    }
}