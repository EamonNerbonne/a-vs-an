using System;
using System.Collections.Generic;
using System.Text;

namespace AvsAnLib.Internals {
    public static class WordQuery {
        /// <summary>
        /// Determines the article for a given prefix by looking it up in the prefix trie.  Recursive.
        /// </summary>
        /// <param name="node">The root of the (remaining) prefix-trie to check</param>
        /// <param name="word">The word being checked</param>
        /// <param name="depth">The depth within the word/trie (initially 0).</param>
        public static AvsAn.Result Query(Node node, string word, int depth) {
            Ratio result = node.ratio;
            while (true)
                if (depth >= word.Length) return new AvsAn.Result(result.aCount, result.anCount, word, depth);
                else if (word[depth] == '(' || word[depth] == '\'' || word[depth] == '"') depth++;
                else break;
            while (true) {
                if (node.SortedKids == null) break;
                char c = depth == word.Length ? ' ' : word[depth];
                int start = 0, end = node.SortedKids.Length;
                while (end - start > 1) {
                    int midpoint = end + start >> 1;
                    if (node.SortedKids[midpoint].c <= c)
                        start = midpoint;
                    else
                        end = midpoint;
                }
                if (node.SortedKids[start].c == c) {
                    node = node.SortedKids[start];
                    depth++;
                    if (node.ratio.isSet)
                        result = node.ratio;
                } else
                    break;

                if (depth > word.Length)
                    break;
            }
            return new AvsAn.Result(result.aCount, result.anCount, word, depth);
        }

        public static AvsAn.PlainResult FastQuery(Node node, string word, int depth) {
            Ratio result = node.ratio;
            while (true)
                if (depth >= word.Length)
                    return new AvsAn.PlainResult( result.AminAnDiff >= 0 ? AvsAn.IndefiniteArticle.A : AvsAn.IndefiniteArticle.An, word, depth);
                else if (word[depth] == '(' || word[depth] == '\'' || word[depth] == '"') depth++;
                else break;
            while (true) {
                if (node.SortedKids == null) break;
                char c = depth == word.Length ? ' ' : word[depth];
                int start = 0, end = node.SortedKids.Length;
                while (end - start > 1) {
                    int midpoint = end + start >> 1;
                    if (node.SortedKids[midpoint].c <= c)
                        start = midpoint;
                    else
                        end = midpoint;
                }
                if (node.SortedKids[start].c == c) {
                    node = node.SortedKids[start];
                    depth++;
                    if (node.ratio.isSet)
                        result = node.ratio;
                } else
                    break;

                if (depth > word.Length)
                    break;
            }
            return new AvsAn.PlainResult(result.AminAnDiff >= 0 ? AvsAn.IndefiniteArticle.A : AvsAn.IndefiniteArticle.An, word, depth); 
        }
    }
}
