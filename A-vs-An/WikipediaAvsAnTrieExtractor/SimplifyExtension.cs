using System;
using System.Collections.Generic;
using AvsAnLib.Internals;

namespace WikipediaAvsAnTrieExtractor {
    public static class SimplifyExtension {
        public static Node Simplify(this Node node, int scaleFactor) {
            if (node.SortedKids == null)
                return new Node {
                    c = node.c,
                    ratio = node.ratio,
                };

            Node[] simpleKids = null;
            int kidCount = 0;
            foreach (var kidEntry in node.SortedKids) {
                var kid = kidEntry;
                var diff = kid.ratio.anCount - kid.ratio.aCount;
                var occurence = kid.ratio.anCount + kid.ratio.aCount;
                if (Math.Abs(occurence) < scaleFactor)
                    continue;
                var simpleKid = kid.Simplify(scaleFactor);
                if (simpleKid.SortedKids != null ||
                    diff * (long)diff >= scaleFactor * (long)occurence
                    && Annotation(simpleKid) != Annotation(node)
                    ) {
                    if (simpleKids == null)
                        simpleKids = new Node[node.SortedKids.Length];
                    simpleKids[kidCount++] = simpleKid;
                }
            }
            if (simpleKids != null)
                Array.Resize(ref simpleKids, kidCount);

            return new Node {
                c = node.c,
                ratio = node.ratio,
                SortedKids = simpleKids
            };
        }

        static bool Annotation(Node node) {
            return node.ratio.anCount > node.ratio.aCount;
        }
    }
}