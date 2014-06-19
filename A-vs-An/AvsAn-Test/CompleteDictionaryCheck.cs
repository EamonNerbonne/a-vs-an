using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using AvsAnLib;
using NUnit.Framework;

namespace AvsAn_Test {
    [UseReporter(typeof(DiffReporter))]
    public class CompleteDictionaryCheck {
        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public void DictionaryClassifications() {
            using (var stream = this.GetType().Assembly.GetManifestResourceStream(this.GetType(), "354984si.ngl"))
            using (var reader = new StreamReader(stream)) {
                var content = reader.ReadToEnd();
                var dictionary = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var mappedDictionary = string.Join("\n", dictionary.Select(word => word + " => " + AvsAn.Query(word).Article));

                Approvals.Verify(mappedDictionary);
            }
        }

        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public void NumberClassifications() {
            var numbers = Enumerable.Range(0, 100000).Select(i => i.ToString());
            var mappedNumbers = string.Join("\n", numbers.Select(word => word + " => " + AvsAn.Query(word).Article));
            Approvals.Verify(mappedNumbers);
        }

        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public void AcronymClassifications() {
            var letters = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (char)i).ToArray();
            var acronyms =
                from len in new[] { 1, 2, 3, 4 }
                from acronym in
                    Enumerable.Repeat(letters, len)
                        .Aggregate(new[] { "" },
                        (prefixes, chars) => (
                        from prefix in prefixes
                        from suffix in chars
                        select prefix + suffix
                        ).ToArray())
                select acronym;

            Enumerable.Range(0, 100000).Select(i => i.ToString());
            var mappedAcronyms = string.Join("\n", acronyms.Select(word => word + " => " + AvsAn.Query(word).Article));
            Approvals.Verify(mappedAcronyms);
        }

    }
}
