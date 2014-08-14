using System.Text;

namespace AvsAnLib.Internals {
    public static class NodeDeserializer {
        static int DenseIntParse(string str, ref int cursor) {
            int retval = 0;
            char c = str[cursor++];
            while (c != ';') {
                retval = retval * 36;
                retval += c - (c >= 'a' ? 'a' - 10 : '0');
                c = str[cursor++];
            }
            return retval;
        }

        static Node DeserializeDenseImpl(string rawDict, ref int cursor) {
            var ratio = new Ratio { 
                aCount = DenseIntParse(rawDict, ref cursor), 
                anCount = DenseIntParse(rawDict, ref cursor) };
           
            int kidCount = DenseIntParse(rawDict, ref cursor);
            Node[] kids = null;
            
            if (kidCount != 0) {
                kids = new Node[kidCount];
                for (int i = 0; i < kidCount; i++) {
                    var c = rawDict[cursor++];
                    var nodeWithIdx = DeserializeDenseImpl(rawDict, ref cursor);
                    kids[i] = nodeWithIdx;
                    kids[i].c = c;
                }
            }
            return new Node {
                ratio = ratio,
                SortedKids = kids
            };
        }

        public static Node DeserializeDense(string rawDict) {
            int cursor = 0;
            return DeserializeDenseImpl(rawDict, ref cursor);
        }
    }
}
