using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AvsAnLib.Internals;

namespace WikipediaAvsAnTrieExtractor {
    public static class ReadableSerializationExtension {

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

            sb.Append('(');
            sb.Append(node.ratio.aCount);
            sb.Append(':');
            sb.Append(node.ratio.anCount);
            sb.Append(")\n");
        }

        public static Node DeserializeReadableNode(string readableRepresentation) {
            var node = new Node();
            foreach (Match m in Regex.Matches(readableRepresentation,
                @"^(.*?)\(([0-9]*):([0-9]*)\)$", RegexOptions.Multiline))
                node.LoadPrefixRatio(
                    m.Groups[1].Value,
                    0,
                    new Ratio {
                        aCount = int.Parse(m.Groups[2].Value),
                        anCount = int.Parse(m.Groups[3].Value)
                    });
            return node;
        }
    }
}