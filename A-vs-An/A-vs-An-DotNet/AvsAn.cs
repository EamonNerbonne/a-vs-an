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
        public static Result Query(string word) {
            return WordQuery.Query(BuiltInDictionary.Root, word, 0);
        }

        // ReSharper disable MemberCanBePrivate.Global
        //part of public nuget api, usages may be in external code.
        public struct Result {
            /// <summary>
            /// How often this prefix was preceded by "a" on wikipedia.
            /// </summary>
            public readonly int aCount;
            /// <summary>
            /// How often this prefix was preceded by "an" on wikipedia.
            /// </summary>
            public readonly int anCount;
            /// <summary>
            /// The prefix of the word on which the determination was based.
            /// </summary>
            public string Prefix {
                get {
                    return Depth > Word.Length
                        ? Word + new string(' ', Depth - Word.Length)
                        : Word.Substring(0, Depth);
                }
            }
            /// <summary>
            /// The tested word.
            /// </summary>
            public readonly string Word;
            /// <summary>
            /// How many letters of the tested word were used in determining the appropriate article.
            /// </summary>
            public readonly int Depth;
            /// <summary>
            /// The article you should use.
            /// </summary>
            public string Article { get { return aCount >= anCount ? "a" : "an"; } }
            public Result(int aCount, int anCount, string word, int depth) {
                this.aCount = aCount;
                this.anCount = anCount;
                Word = word;
                Depth = depth;
            }
        }
        // ReSharper restore MemberCanBePrivate.Global

    }
}
