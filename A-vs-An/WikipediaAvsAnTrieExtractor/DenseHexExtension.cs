using System.Text;
using AvsAnLib.Internals;

namespace WikipediaAvsAnTrieExtractor {
    public static class DenseHexExtension {
        public static string SerializeToDenseHex(this Node node) {
            var sb = new StringBuilder();
            SerializeToDenseHex(node, sb, "");
            return sb.ToString();
        }

        static void SerializeToDenseHex(this Node node, StringBuilder sb, string prefix) {
            sb.Append(prefix);
            sb.Append('(');
            sb.Append(node.ratio.aCount.ToString("x"));
            sb.Append(':');
            sb.Append(node.ratio.anCount.ToString("x"));
            sb.Append(')');
            if (node.SortedKids != null)
                foreach (var kidEntry in node.SortedKids)
                    kidEntry.SerializeToDenseHex(sb, prefix + kidEntry.c);
        }
    }
}