using System.Collections.Generic;

namespace AvsAnLib.Internals {
    public static class NodeDeserializer {
        static int DenseIntParse(string str, ref int cursor) {
            var retval = 0;
            var c = str[cursor++];
            while (c != ';') {
                retval = retval * 36;
                retval += c - (c >= 'a' ? 'a' - 10 : '0');
                c = str[cursor++];
            }
            return retval;
        }

        static Node DeserializeDenseImpl(string rawDict, ref int cursor, Ratio parentRatio) {
            var retval = new Node {
                ratio = new Ratio {
                    aCount = DenseIntParse(rawDict, ref cursor),
                    anCount = DenseIntParse(rawDict, ref cursor)
                }
            };
            if (!retval.ratio.isSet) {
                retval.ratio = parentRatio;
            }
            var kidCount = DenseIntParse(rawDict, ref cursor);

            if (kidCount != 0) {
                retval.SortedKids = new Dictionary<char, Node>();
                for (var i = 0; i < kidCount; i++) {
                    var c = rawDict[cursor++];
                    var nodeWithIdx = DeserializeDenseImpl(rawDict, ref cursor, retval.ratio);
                    retval.SortedKids.Add(c, nodeWithIdx);
                }
            }

            return retval;
        }

        public static Node DeserializeDense(string rawDict) {
            var cursor = 0;
            return DeserializeDenseImpl(rawDict, ref cursor, default);
        }
    }
}
