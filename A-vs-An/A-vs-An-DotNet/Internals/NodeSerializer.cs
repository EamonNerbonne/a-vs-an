using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace AvsAnLib.Internals {
    public static class NodeSerializer {
        public static Node Deserialize(string rawDict) {
            var mutableRoot = new Node();
            foreach (
                Match m in Regex.Matches(rawDict, @"([^(\n]*)\(([0-9a-f]*):([0-9a-f]*)\)", RegexOptions.CultureInvariant)
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
            return str == "" ? 0 : Int32.Parse(str, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
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