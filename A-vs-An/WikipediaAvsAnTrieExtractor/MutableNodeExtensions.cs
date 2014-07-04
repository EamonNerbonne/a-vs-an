using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AvsAnLib;

namespace WikipediaAvsAnTrieExtractor {

    public static class MutableNodeExtensions {
        public static int Count(this MutableNode node) {
            int count = 1;
            if (node.Kids != null)
                foreach (var kid in node.Kids.Values)
                    count += Count(kid);
            return count;
        }

        public static string SerializeToReadable(this MutableNode node) {
            var sb = new StringBuilder();
            SerializeToReadableImpl(node, sb, "");
            return sb.ToString();
        }
        static void SerializeToReadableImpl(this MutableNode node, StringBuilder sb, string prefix) {
            if (node.Kids != null)
                foreach (var kidEntry in node.Kids.OrderBy(kv => kv.Key.ToString(CultureInfo.InvariantCulture), StringComparer.InvariantCultureIgnoreCase))
                    kidEntry.Value.SerializeToReadableImpl(sb, prefix + kidEntry.Key);
            sb.Append(prefix);

            sb.Append(node.ratio.anCount < node.ratio.aCount ? "[a:" : node.ratio.anCount > node.ratio.aCount ? "[an:" : "[?:");
            sb.Append(node.ratio.anCount);
            sb.Append(':');
            sb.Append(node.ratio.aCount);
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
                        aCount = int.Parse(m.Groups[3].Value),
                        anCount = int.Parse(m.Groups[2].Value)
                    });
            return mutableRoot;
        }

        public static void IncrementPrefix(this MutableNode node, bool isAn, string word, int level) {
            if (isAn) node.ratio.anCount++;
            else node.ratio.anCount++;

            if (level < 40 && word.Length > level)
                GetChild(node, word[level]).IncrementPrefix(isAn, word, level + 1);
        }

        public static MutableNode GetChild(this MutableNode node, char c) {
            if (node.Kids == null)
                node.Kids = new Dictionary<char, MutableNode>();
            MutableNode kid;
            if (!node.Kids.TryGetValue(c, out kid))
                node.Kids.Add(c, kid = new MutableNode());
            return kid;
        }

        static int Occurence(this MutableNode node) {
            return node.ratio.anCount + node.ratio.aCount;
        }
        static int Diff(this MutableNode node) { return Math.Abs(node.ratio.anCount - node.ratio.aCount); }
        static double DiffRatio(this MutableNode node) {
            return node.Diff() / (double)node.Occurence();
        }
        static int Annotation(this MutableNode node) { return Math.Sign(node.ratio.anCount - node.ratio.aCount); }

        //const int MinOccurence = 19;
        //const int MinDiff = 9;
        //const double MinRatio = 0.1;//e.g. 0.45 to 0.55

        public static MutableNode Simplify(this MutableNode node, int MinOccurence, int MinDiff, double MinRatio) {
            Dictionary<char, MutableNode> simpleKids = null;
            if (node.Kids != null)
                foreach (var kidEntry in node.Kids) {
                    var kid = kidEntry.Value;
                    if (kid.Occurence() < MinOccurence) continue;
                    var simpleKid = kid.Simplify(MinOccurence, MinDiff, MinRatio);
                    if (simpleKid.Kids != null ||
                        simpleKid.Diff() >= MinDiff && simpleKid.DiffRatio() >= MinRatio && kid.Annotation() != node.Annotation()) {

                        simpleKids = simpleKids ?? new Dictionary<char, MutableNode>();
                        simpleKids.Add(kidEntry.Key, simpleKid);
                    }
                }
            return new MutableNode {
                ratio = node.ratio,
                Kids = simpleKids
            };
        }
    }
}
