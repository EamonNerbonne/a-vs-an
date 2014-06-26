using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikipediaAvsAnTrieExtractor {
    class AnnotatedTrie {
        const int MinOccurence = 19;
        const int MinDiff = 9;
        const double MinRatio = 0.1;//e.g. 0.45 to 0.55

        int count_an;
        int count_a;
        Dictionary<char, AnnotatedTrie> children;

        AnnotatedTrie GetChild(char c) {
            if (children == null)
                children = new Dictionary<char, AnnotatedTrie>();
            AnnotatedTrie child;
            if (!children.TryGetValue(c, out child))
                children.Add(c, child = new AnnotatedTrie());
            return child;
        }

        public void AddEntry(bool isAn, string word, int level) {
            if (isAn) ++count_an;
            else ++count_a;

            if (level < 40 && word.Length > level)
                GetChild(word[level]).AddEntry(isAn, word, level + 1);
        }
        public int Occurence { get { return count_a + count_an; } }
        public int Annotation { get { return Math.Sign(count_an - count_a); } }
        public int Diff { get { return Math.Abs(count_an - count_a); } }
        public double DiffRatio { get { return Diff / (double)Occurence; } }
        public bool HasChildren { get { return children != null; } }
        public int Count { get { return 1 + (children == null ? 0 : children.Values.Sum(child => child.Count)); } }
        public int CountParallel { get { return 1 + (children == null ? 0 : children.Values.AsParallel().Sum(child => child.Count)); } }

        public AnnotatedTrie Simplify() {
            Dictionary<char, AnnotatedTrie> simpleChildren = null;
            if (HasChildren)
                foreach (var child_kv in children) {
                    var child = child_kv.Value;
                    if (child.Occurence < MinOccurence) continue;
                    var simpleChild = child.Simplify();
                    if (simpleChild.HasChildren ||
                        simpleChild.Diff >= MinDiff && simpleChild.DiffRatio >= MinRatio && child.Annotation != Annotation) {

                        simpleChildren = simpleChildren ?? new Dictionary<char, AnnotatedTrie>();
                        simpleChildren.Add(child_kv.Key, simpleChild);
                    }
                }
            return new AnnotatedTrie {
                count_a = count_a,
                count_an = count_an,
                children = simpleChildren
            };
        }

        public string SerializeToReadable() {
            var sb = new StringBuilder();
            SerializeToReadableImpl(sb, "");
            return sb.ToString();
        }
        void SerializeToReadableImpl(StringBuilder sb, string prefix) {
            if (HasChildren)
                foreach (var child_kv in children.OrderBy(kv => kv.Key.ToString(), StringComparer.InvariantCultureIgnoreCase))
                    child_kv.Value.SerializeToReadableImpl(sb, prefix + child_kv.Key);
            sb.Append(prefix);
            sb.Append(Annotation < 0 ? "[a:" : Annotation > 0 ? "[an:" : "[?:");
            sb.Append(count_an);
            sb.Append(':');
            sb.Append(count_a);
            sb.Append("]\n");
        }
    }
}
