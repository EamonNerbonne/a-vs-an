using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvsAnTrie {
	class AnnotatedTrie {
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

		const int MinOccurence = 10;
		const int MinDiff = 5;
		const double MinRatio = 0.1;//e.g. 0.45 to 0.55
		public void Simplify() {
			if (children != null) {
				foreach (var child in children.Values)
					child.Simplify();
				foreach (var child_kv in children.ToArray()) {
					var child = child_kv.Value;

					bool childless = !child.HasChildren;
					bool tooRare = child.Occurence < MinOccurence;

					bool sameAnnotation = !child.HasChildren && child.Annotation == Annotation;
					bool tooVague = child.Diff < MinDiff || child.DiffRatio < MinRatio;
					if (childless && (tooRare || tooVague || sameAnnotation))
						children.Remove(child_kv.Key);
				}
				if (!children.Any())
					children = null;
			}
		}

		public string Readable() {
			var sb = new StringBuilder();
			RepresentSelf(sb, "", 0);
			return sb.ToString();
		}
		void RepresentSelf(StringBuilder sb, string prefix, int parentAnnot) {
			if (HasChildren)
				foreach (var child_kv in children.OrderBy(kv=>kv.Key.ToString(),StringComparer.InvariantCultureIgnoreCase))
					child_kv.Value.RepresentSelf(sb, prefix + child_kv.Key, Annotation);
			if (Annotation != parentAnnot) {
				sb.Append(prefix);
				sb.Append(Annotation < 0 ? "[a:" : Annotation > 0 ? "[an:" : "[?:");
				sb.Append(count_an);
				sb.Append(':');
				sb.Append(count_a);
				sb.Append("]\n");
			}
		}
	}
}
