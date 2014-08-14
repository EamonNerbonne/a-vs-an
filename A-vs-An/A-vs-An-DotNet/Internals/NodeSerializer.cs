using System.Security.Cryptography;
using System.Text;

namespace AvsAnLib.Internals {
    public static class NodeSerializer {
        public static Node Deserialize(string rawDict) {
            var mutableRoot = new Node();
            int pos = 0;
            int len = rawDict.Length;
            while (true) {
                while (true) {
                    if (pos >= len) {
                        return mutableRoot;
                    }
                    if (rawDict[pos] != ' ' && rawDict[pos] != '\n' && rawDict[pos] != '\r') {
                        break;
                    }
                    pos++;
                }
                int start = pos;
                while (true) {
                    if (pos >= len) {
                        return mutableRoot;
                    }
                    if (rawDict[pos] == '(') {
                        break;
                    }
                    pos++;
                }
                int paren0 = pos;
                pos++;
                while (true) {
                    if (pos >= len) {
                        return mutableRoot;
                    }
                    if (rawDict[pos] == ':') {
                        break;
                    }
                    pos++;
                }
                int sep = pos;
                while (true) {
                    if (pos >= len) {
                        return mutableRoot;
                    }
                    if (rawDict[pos] == ')') {
                        break;
                    }
                    pos++;
                }
                int paren1 = pos;
                mutableRoot.LoadPrefixRatio(
                    rawDict.Substring(start, paren0 - start),
                    0,
                    new Ratio {
                        aCount = parseHex(rawDict, paren0 + 1, sep),
                        anCount = parseHex(rawDict, sep + 1, paren1)
                    });
                pos++;
            }
        }

        static int parseHex(string str, int start, int end) {
            int retval = 0;
            while (start < end) {
                retval = retval << 4;
                char c = str[start];
                int charVal;
                if (c <= '9') {
                    charVal = c - '0';
                } else if (c <= 'F') {
                    charVal = c - 'A' + 10;
                } else {
                    charVal = c - 'a' + 10;
                }
                retval += charVal;
                start++;
            }
            return retval;
        }

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
        static void DenseIntToString(StringBuilder sb, int num) {
            DenseIntDigitToString(sb, num);
            sb.Append(';');
        }

        static void DenseIntDigitToString(StringBuilder sb, int num) {
            if (num > 0) {
                int digit = num % 36;
                DenseIntDigitToString(sb, num / 36);
                sb.Append((char)(digit + (digit < 10 ? '0' : 'a' - 10)));
            }
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


        public static string SerializeDense(Node node) {
            var sb = new StringBuilder();
            SerializeDenseImpl(sb, node);
            return sb.ToString();
        }


        public static void SerializeDenseImpl(StringBuilder sb, Node node) {
            DenseIntToString(sb, node.ratio.aCount);
            DenseIntToString(sb, node.ratio.anCount);
            DenseIntToString(sb, node.SortedKids == null ? 0 : node.SortedKids.Length);
            if (node.SortedKids != null) {
                foreach (var kid in node.SortedKids) {
                    sb.Append(kid.c);
                    SerializeDenseImpl(sb, kid);
                }
            }
        }

        public static string SerializeReadably(Node node) {
            return SerializeImpl(node, true);
        }

        public static string Serialize(Node node) {
            return SerializeImpl(node, false);
        }

        static string SerializeImpl(Node node, bool withNewlines) {
            var sb = new StringBuilder();
            SerializeImpl(node, sb, "", withNewlines);
            return sb.ToString();
        }

        static void SerializeImpl(Node node, StringBuilder sb, string prefix, bool withNewlines) {
            sb.Append(prefix);
            sb.Append('(');
            sb.Append(node.ratio.aCount.ToString("x"));
            sb.Append(':');
            sb.Append(node.ratio.anCount.ToString("x"));
            sb.Append(')');
            if (withNewlines) {
                sb.Append('\n');
            }
            if (node.SortedKids != null) {
                foreach (var kidEntry in node.SortedKids) {
                    SerializeImpl(kidEntry, sb, prefix + kidEntry.c, withNewlines);
                }
            }
        }
    }
}
