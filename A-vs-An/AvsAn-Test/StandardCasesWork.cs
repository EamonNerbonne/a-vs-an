using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvsAnLib;
using ExpressionToCodeLib;
using NUnit.Framework;

namespace AvsAn_Test {
    public class StandardCasesWork {
        [TestCase("an", "unanticipated result")]
        [TestCase("a", "unanimous vote")]
        [TestCase("an", "honest decision")]
        [TestCase("a", "honeysuckle shrub")]
        [TestCase("an", "0800 number")]
        [TestCase("an", "∞ of oregano")]
        [TestCase("a", "NASA scientist")]
        [TestCase("an", "NSA analyst")]
        [TestCase("a", "FIAT car")]
        [TestCase("an", "FAA policy")]
        [TestCase("an", "A")]
        [TestCase("a", "uniformed agent")]
        [TestCase("an", "unissued permit")]
        [TestCase("an", "unilluminating argument")]
        public void DoTest(string article, string word) {
            PAssert.That(() => AvsAn.Query(word).Article == article);
        }

        [TestCase("a", "", "")]
        [TestCase("a", "'", "'")]
        [TestCase("an", "N", "N ")]
        [TestCase("a", "NASA", "NAS")]
        public void CheckOddPrefixes(string article, string word, string prefix) {
            PAssert.That(() => AvsAn.Query(word).Article == article && AvsAn.Query(word).Prefix == prefix);
        }
    }
}
