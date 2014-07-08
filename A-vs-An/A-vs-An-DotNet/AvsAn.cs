//by Eamon Nerbonne (from http://home.nerbonne.org/A-vs-An), Apache 2.0 license.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using AvsAnLib.Internals;

namespace AvsAnLib {
    public static partial class AvsAn {
        /// <summary>
        /// Determines whether an english word should be preceded by the indefinite article "a" or "an".
        /// 
        /// By Eamon Nerbonne; feedback can be reported to https://github.com/EamonNerbonne/a-vs-an/
        /// </summary>
        /// <param name="word">The word to test.  AvsAn assumes this is a complete word; in some cases word-prefixes may result in a differing classification that complete words.  If you wish to classify an incomplete word (a prefix), append a non-word, non-space character such as the underscore "_" as a placeholder for further letters.</param>
        /// <returns>A classification result indicating "a" or "an" with some wikipedia-derived statistics.</returns>
        public static Result Query(string word) { return WordQuery.Query(root, word, 0); }


        static readonly Node root;

        static AvsAn() {
            var mutableRoot = new MutableNode();
            foreach (Match m in Regex.Matches(BuiltInDictionary.dict, @"([^\[]*)\[([0-9a-f]*):([0-9a-f]*)\]", RegexOptions.CultureInvariant))
                mutableRoot.LoadPrefixRatio(
                    m.Groups[1].Value,
                    0,
                    new Ratio {
                        aCount = parseHex(m.Groups[2].Value),
                        anCount = parseHex(m.Groups[3].Value)
                    });
            root = mutableRoot.Finish(default(char));
        }
        static int parseHex(string str) {
            return str == "" ? 0 : int.Parse(str, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
        }
    }
}
