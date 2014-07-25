using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikipediaAvsAnTrieExtractor;
using Xunit;

namespace WikipediaAvsAnTrieExtractorTest {
    public class EntityDecoderTest {
        readonly RegexTextUtils utils = new RegexTextUtils();

        [Fact]
        public void AmpLessTextUnchanged() {
            Assert.Equal("This is text.", utils.DecodeEntities("This is text."));
        }

        [Fact]
        public void SpacedAmpUnchanged() {
            Assert.Equal("This is a Q & A.", utils.DecodeEntities("This is a Q & A."));
        }
        [Fact]
        public void NonSemicolonAmpUnchanged() {
            Assert.Equal("This is a Q&A.", utils.DecodeEntities("This is a Q&A."));
        }
        [Fact]
        public void TrivialLtEntityReplaced() {
            Assert.Equal("<", utils.DecodeEntities("&lt;"));
        }
        [Fact]
        public void MessyLtEntityReplaced() {
            Assert.Equal("&<;", utils.DecodeEntities("&&lt;;"));
        }
        [Fact]
        public void UnknownEntityUnchanged() {
            Assert.Equal("&xyz;", utils.DecodeEntities("&xyz;"));
        }
        [Fact]
        public void ReplacesInTheMiddleOfText() {
            Assert.Equal("The html &amp; entity is an &.", utils.DecodeEntities("The html &amp;amp; entity is an &."));
        }
    }
}
