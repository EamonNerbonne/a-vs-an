using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressionToCodeLib;
using WikipediaAvsAnTrieExtractor;
using Xunit;

namespace WikipediaAvsAnTrieExtractorTest {
    
    public class MarkupStripperTests {
        readonly RegexTextUtils utils = new RegexTextUtils();

        [Fact]
        public void StripsMarkup()
        {
            var data = new Dictionary<string, string>
            {
            };
            foreach (var kvp in data) {
                PAssert.That(() => kvp.Value == utils.StripWikiMarkup(kvp.Key));
            }
        }
    }
}
