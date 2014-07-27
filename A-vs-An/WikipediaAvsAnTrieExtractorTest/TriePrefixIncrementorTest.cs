using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvsAnLib.Internals;
using WikipediaAvsAnTrieExtractor;
using Xunit;

namespace WikipediaAvsAnTrieExtractorTest {
    public class TriePrefixIncrementorTest {
        [Fact]
        public void BasicIncrementWorks() {
            Node node = new Node();
            IncrementPrefixExtensions.IncrementPrefix(ref node, true, "test", 0);
            Assert.Equal(NodeSerializer.Serialize(node), @"(0:1)t(0:1)te(0:1)tes(0:1)test(0:1)test (0:1)" );
        }

        [Fact]
        public void IncrementWithSharedPrefixWorks() {
            Node node = new Node();
            IncrementPrefixExtensions.IncrementPrefix(ref node, true, "test", 0);
            IncrementPrefixExtensions.IncrementPrefix(ref node, true, "taste", 0);
            Assert.Equal(NodeSerializer.Serialize(node), @"(0:2)t(0:2)ta(0:1)tas(0:1)tast(0:1)taste(0:1)taste (0:1)te(0:1)tes(0:1)test(0:1)test (0:1)");
        }
    }
}
