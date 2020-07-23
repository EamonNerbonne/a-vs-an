using WikipediaAvsAnTrieExtractor;
using Xunit;

namespace WikipediaAvsAnTrieExtractorTest {
    public class MarkupStripperUnitTests {
        static readonly RegexTextUtils utils = UtilsInstance.Utils;

        [Fact]
        public void StripsRedirects() {
            Assert.Equal("", utils.StripWikiMarkup(@"#REDIRECT [[Foreign relations of the Czech Republic]]"));
        }

        [Fact]
        public void StripsHtml() {
            Assert.Equal("test", utils.StripWikiMarkup(@"<b>test</b>"));
        }
        [Fact]
        public void StripsMathEntirelyHtml() {
            Assert.Equal("atest", utils.StripWikiMarkup(@"a<math><nested></x></math>test"));
        }
        [Fact]
        public void StripsCommentsEntirelyHtml() {
            Assert.Equal("atest", utils.StripWikiMarkup(@"a<!--<math>x</math>y--z-->test"));
        }

        [Fact]
        public void ReplacesNormalWikiLinks() {
            Assert.Equal("TestLink", utils.StripWikiMarkup(@"[[TestLink]]"));
        }

        [Fact]
        public void ReplacesWikiLinksWithColons() {
            Assert.Equal("WP:Policy", utils.StripWikiMarkup(@"[[WP:Policy]]"));
        }

        [Fact]
        public void ReplacesWikiLinksHavingText() {
            Assert.Equal("Some Text", utils.StripWikiMarkup(@"[[WP:Policy|Some Text]]"));
        }

        [Fact]
        public void StripsLanguageLinks() {
            Assert.Equal("", utils.StripWikiMarkup(@"[[en:English Link]]"));
        }
        [Fact]
        public void StripsCategoryinks() {
            Assert.Equal("", utils.StripWikiMarkup(@"[[Category:Some Category]]"));
        }

        [Fact]
        public void SupportsExternalLinksHavingText() {
            Assert.Equal("an example", utils.StripWikiMarkup(@"[http://example.org an example]"));
        }

        [Fact]
        public void RetainsNonLinkSquareBrackets() {
            Assert.Equal("[Q]", utils.StripWikiMarkup(@"[Q]"));
        }


    }
}
