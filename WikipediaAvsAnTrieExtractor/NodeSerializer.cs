using System.Linq;
using System.Text;
using AvsAnLib.Internals;

namespace WikipediaAvsAnTrieExtractor {
    public static class NodeSerializer {
        public static Node Deserialize(string rawDict) {
            var mutableRoot = new Node();
            var pos = 0;
            var len = rawDict.Length;
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
                var start = pos;
                while (true) {
                    if (pos >= len) {
                        return mutableRoot;
                    }
                    if (rawDict[pos] == '(') {
                        break;
                    }
                    pos++;
                }
                var paren0 = pos;
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
                var sep = pos;
                while (true) {
                    if (pos >= len) {
                        return mutableRoot;
                    }
                    if (rawDict[pos] == ')') {
                        break;
                    }
                    pos++;
                }
                var paren1 = pos;
                mutableRoot.LoadPrefixRatio(rawDict.Substring(start, paren0 - start),
                    0,
                    new Ratio {
                        aCount = parseHex(rawDict, paren0 + 1, sep),
                        anCount = parseHex(rawDict, sep + 1, paren1)
                    });
                pos++;
            }
        }

        static int parseHex(string str, int start, int end) {
            var retval = 0;
            while (start < end) {
                retval = retval << 4;
                var c = str[start];
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

        static void DenseIntToString(StringBuilder sb, int num) {
            DenseIntDigitToString(sb, num);
            sb.Append(';');
        }

        static void DenseIntDigitToString(StringBuilder sb, int num) {
            if (num > 0) {
                var digit = num % 36;
                DenseIntDigitToString(sb, num / 36);
                sb.Append((char)(digit + (digit < 10 ? '0' : 'a' - 10)));
            }
        }

        public static string SerializeDense(Node node) {
            var sb = new StringBuilder();
            SerializeDenseImpl(sb, node);
            return sb.ToString();
        }

        static void SerializeDenseImpl(StringBuilder sb, Node node) {
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

        public static string SerializeDenseNoStats(Node node) {
            var sb = new StringBuilder();
            SerializeDenseNoStatsImpl(sb, node, IsAn(node, false));
            return sb.ToString();
        }

        //The no-stats format can't represent unset ("unsure") ratios, so like AvsAn.Query they inherit the nearest ancestor's article;
        //kids that can't change the resulting article are omitted.
        static void SerializeDenseNoStatsImpl(StringBuilder sb, Node node, bool isAn) {
            var kids = node.SortedKids?.Where(kid => AffectsArticle(kid, isAn)).ToArray() ?? new Node[0];
            DenseIntDigitToString(sb, kids.Length);
            sb.Append(isAn ? ';' : '.');
            foreach (var kid in kids) {
                sb.Append(kid.c);
                SerializeDenseNoStatsImpl(sb, kid, IsAn(kid, isAn));
            }
        }

        static bool IsAn(Node node, bool inheritedIsAn)
            => node.ratio.isSet ? node.ratio.AminAnDiff < 0 : inheritedIsAn;

        static bool AffectsArticle(Node node, bool inheritedIsAn) {
            var isAn = IsAn(node, inheritedIsAn);
            return isAn != inheritedIsAn || node.SortedKids != null && node.SortedKids.Any(kid => AffectsArticle(kid, isAn));
        }

        // ReSharper disable once UnusedMember.Global
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
