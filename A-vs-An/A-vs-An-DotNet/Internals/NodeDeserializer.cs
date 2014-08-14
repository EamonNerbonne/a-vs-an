using System.Text;

namespace AvsAnLib.Internals {
    public static class NodeDeserializer {
        static ParseResult<T> Result<T>(int cursor, T value) {
            return new ParseResult<T> { Cursor = cursor, Value = value };
        }

        struct ParseResult<T> {
            public int Cursor;
            public T Value;
        }

        static ParseResult<int> DenseIntParse(string str, int cursor) {
            int retval = 0;
            char c = str[cursor++];
            while (c != ';') {
                retval = retval * 36;
                retval += c - (c >= 'a' ? 'a' - 10 : '0');
                c = str[cursor++];
            }
            return Result(cursor, retval);
        }

        static ParseResult<Node> DeserializeDenseImpl(string rawDict, int cursor) {
            var aCountResult = DenseIntParse(rawDict, cursor);
            var anCountResult = DenseIntParse(rawDict, aCountResult.Cursor);
            var kidCountResult = DenseIntParse(rawDict, anCountResult.Cursor);
            int kidCount = kidCountResult.Value;
            var ratio = new Ratio { aCount = aCountResult.Value, anCount = anCountResult.Value };
            cursor = kidCountResult.Cursor;
            Node[] kids = null;
            if (kidCount != 0) {
                kids = new Node[kidCount];
                for (int i = 0; i < kidCount; i++) {
                    var c = rawDict[cursor];
                    var nodeWithIdx = DeserializeDenseImpl(rawDict, cursor + 1);
                    kids[i] = nodeWithIdx.Value;
                    kids[i].c = c;
                    cursor = nodeWithIdx.Cursor;
                }
            }
            return Result(cursor,
                new Node {
                    ratio = ratio,
                    SortedKids = kids
                });
        }

        public static Node DeserializeDense(string rawDict) {
            return DeserializeDenseImpl(rawDict, 0).Value;
        }
    }
}
