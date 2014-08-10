using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvsAnLib;
using ExpressionToCodeLib;
using Xunit.Extensions;

namespace AvsAn_Test {
    public class StandardCasesWork {
        [Theory]
        [InlineData("an", "unanticipated result")]
        [InlineData("a", "unanimous vote")]
        [InlineData("an", "honest decision")]
        [InlineData("a", "honeysuckle shrub")]
        [InlineData("an", "0800 number")]
        //[InlineData("an", "∞ of oregano")]//no longer reliable in latest wikidump!
        [InlineData("a", "NASA scientist")]
        [InlineData("an", "NSA analyst")]
        [InlineData("a", "FIAT car")]
        [InlineData("an", "FAA policy")]
        [InlineData("an", "A")]
        [InlineData("a", "uniformed agent")]
        [InlineData("an", "unissued permit")]
        [InlineData("an", "unilluminating argument")]
        public void DoTest(string article, string word) {
            PAssert.That(() => AvsAn.Query(word).Article == article);
        }

        [Theory]
        [InlineData("a", "", "")]
        [InlineData("a", "'", "'")]
        [InlineData("an", "N", "N ")]
        [InlineData("a", "NASA", "NAS")]
        public void CheckOddPrefixes(string article, string word, string prefix) {
            PAssert.That(() => AvsAn.Query(word).Article == article && AvsAn.Query(word).Prefix == prefix);
        }
    }
}
