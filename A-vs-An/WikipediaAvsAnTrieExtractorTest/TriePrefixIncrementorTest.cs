using System;
using System.Collections.Generic;
using System.Linq;
using AvsAnLib.Internals;
using WikipediaAvsAnTrieExtractor;
using Xunit;

namespace WikipediaAvsAnTrieExtractorTest {
    public class TriePrefixIncrementorTest {
        [Fact]
        public void BasicIncrementWorks() {
            var node = new Node();
            IncrementPrefixExtensions.IncrementPrefix(ref node, true, "test", 0);
            Assert.Equal(@"(0:1)t(0:1)te(0:1)tes(0:1)test(0:1)test (0:1)" , NodeSerializer.Serialize(node));
        }

        [Fact]
        public void IncrementWithSharedPrefixWorks() {
            var node = new Node();
            IncrementPrefixExtensions.IncrementPrefix(ref node, true, "test", 0);
            IncrementPrefixExtensions.IncrementPrefix(ref node, true, "taste", 0);
            Assert.Equal(@"(0:2)t(0:2)ta(0:1)tas(0:1)tast(0:1)taste(0:1)taste (0:1)te(0:1)tes(0:1)test(0:1)test (0:1)", NodeSerializer.Serialize(node));
        }
    }
}
