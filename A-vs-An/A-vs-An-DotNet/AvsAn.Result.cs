using System;
using System.Collections.Generic;
using System.Text;

namespace AvsAnLib {
    public partial class AvsAn {
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
