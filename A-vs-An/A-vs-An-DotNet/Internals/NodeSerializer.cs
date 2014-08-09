using System.Text;

namespace AvsAnLib.Internals {
    public static class NodeSerializer {
        public static Node Deserialize(string rawDict) {
            var mutableRoot = new Node();
            int pos = 0;
            int len = rawDict.Length;
            while (true) {
                while (true) {
                    if (pos >= len)
                        return mutableRoot;
                    if (rawDict[pos] != ' ' && rawDict[pos] != '\n' && rawDict[pos] != '\r')
                        break;
                    pos++;
                }
                int start = pos;
                while (true) {
                    if (pos >= len)
                        return mutableRoot;
                    if (rawDict[pos] == '(')
                        break;
                    pos++;
                }
                int paren0 = pos;
                pos++;
                while (true) {
                    if (pos >= len)
                        return mutableRoot;
                    if (rawDict[pos] == ':')
                        break;
                    pos++;
                }
                int sep = pos;
                while (true) {
                    if (pos >= len)
                        return mutableRoot;
                    if (rawDict[pos] == ')')
                        break;
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
                if (c <= '9')
                    charVal = c - '0';
                else if (c <= 'F')
                    charVal = c - 'A' + 10;
                else
                    charVal = c - 'a' + 10;
                retval += charVal;
                start++;
            }
            return retval;
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
            if (withNewlines)
                sb.Append('\n');
            if (node.SortedKids != null)
                foreach (var kidEntry in node.SortedKids)
                    SerializeImpl(kidEntry, sb, prefix + kidEntry.c, withNewlines);
        }
    }
}