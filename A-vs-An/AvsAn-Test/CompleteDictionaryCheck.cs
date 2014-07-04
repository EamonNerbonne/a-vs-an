using System;
using System.Collections.Generic;
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
            var dictionary = Dictionaries.LoadEnglishDictionary();
            var mappedDictionary = string.Join("\n", dictionary.Select(word => word + " => " + AvsAn.Query(word).Article));
            Approvals.Verify(mappedDictionary);
        }

        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public void NumberClassifications() {
            var numbers = Dictionaries.SmallNumberStrings();
            var mappedNumbers = string.Join("\n", numbers.Select(word => word + " => " + AvsAn.Query(word).Article));
            Approvals.Verify(mappedNumbers);
        }

        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public void AcronymClassifications() {
            var mappedAcronyms = string.Join("\n", Dictionaries.AcronymsWithUpto4Letters().Select(word => word + " => " + AvsAn.Query(word).Article));
            Approvals.Verify(mappedAcronyms);
        }
    }
}
