using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AvsAnLib.Internals;

namespace WikipediaAvsAnTrieExtractor {
    public static class ReadableSerializationExtension {
        public static string SerializeToReadable(this MutableNode node) {
            var sb = new StringBuilder();
            SerializeToReadableImpl(node, sb, "");
            return sb.ToString();
        }

        static void SerializeToReadableImpl(this MutableNode node, StringBuilder sb, string prefix) {
            if (node.Kids != null)
                foreach (
                    var kidEntry in
                        node.Kids.OrderBy(kv => kv.Key))
                    kidEntry.Value.SerializeToReadableImpl(sb, prefix + kidEntry.Key);
            sb.Append(prefix);

            sb.Append(node.ratio.anCount < node.ratio.aCount
                ? "[a:"
                : node.ratio.anCount > node.ratio.aCount ? "[an:" : "[?:");
            sb.Append(node.ratio.aCount);
            sb.Append(':');
            sb.Append(node.ratio.anCount);
            sb.Append("]\n");
        }

        public static string SerializeToReadable(this Node node) {
            var sb = new StringBuilder();
            SerializeToReadableImpl(node, sb, "");
            return sb.ToString();
        }

        static void SerializeToReadableImpl(this Node node, StringBuilder sb, string prefix) {
            if (node.SortedKids != null)
                foreach (var kidEntry in node.SortedKids)
                    kidEntry.SerializeToReadableImpl(sb, prefix + kidEntry.c);
            sb.Append(prefix);

            sb.Append(node.ratio.anCount < node.ratio.aCount
                ? "[a:"
                : node.ratio.anCount > node.ratio.aCount ? "[an:" : "[?:");
            sb.Append(node.ratio.aCount);
            sb.Append(':');
            sb.Append(node.ratio.anCount);
            sb.Append("]\n");
        }


        public static MutableNode DeserializeReadable(string readableRepresentation) {
            var mutableRoot = new MutableNode();
            foreach (Match m in Regex.Matches(readableRepresentation,
                @"^(.*?)\[an?:([0-9]*):([0-9]*)\]$", RegexOptions.Multiline))
                mutableRoot.LoadPrefixRatio(
                    m.Groups[1].Value,
                    0,
                    new Ratio {
                        aCount = int.Parse(m.Groups[2].Value),
                        anCount = int.Parse(m.Groups[3].Value)
                    });
            return mutableRoot;
        }

        public static Node DeserializeReadableNode(string readableRepresentation) {
            var mutableRoot = new Node();
            foreach (Match m in Regex.Matches(readableRepresentation,
                @"^(.*?)\[an?:([0-9]*):([0-9]*)\]$", RegexOptions.Multiline))
                mutableRoot.LoadPrefixRatio(
                    m.Groups[1].Value,
                    0,
                    new Ratio {
                        aCount = int.Parse(m.Groups[2].Value),
                        anCount = int.Parse(m.Groups[3].Value)
                    });
            return mutableRoot;


        }
    }
}