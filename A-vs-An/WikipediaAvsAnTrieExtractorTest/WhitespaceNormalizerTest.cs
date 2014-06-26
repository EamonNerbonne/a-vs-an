using System;
using System.Collections.Generic;
using System.Linq;
using ExpressionToCodeLib;
using WikipediaAvsAnTrieExtractor;
using Xunit;

namespace WikipediaAvsAnTrieExtractorTest
{
    public class WhitespaceNormalizerTest
    {
        [Fact]
        public void WhitespaceNormalizes()
        {
            var data = new Dictionary<string, string>
            {
                {"A  test with two spaces", "A test with two spaces"},
                {"A test with a trailing newline\n", "A test with a trailing newline\n"},
                {"A test1 \n", "A test1\n"},
                {"A test2\n \n", "A test2\n"},
                {"A test3\n  ", "A test3\n"},
                {"A test4\n\n\na line", "A test4\n\na line"},
                {"A test5\t\t same line", "A test5 same line"},
                {"A test6\n\t\n \n  \nnew line", "A test6\n\nnew line"},
                {"A test7\nsame line", "A test7\nsame line"},
                {"\n\t\n \n  \nA test\n\t\n \n  \nXYZ\n\t\n \n  \n", "\n\nA test\n\nXYZ\n"},
                {"\nA test\nXYZ\n", "\nA test\nXYZ\n"},
            };

            foreach (var kvp in data)
            {
                PAssert.That(() => kvp.Value == WhitespaceNormalizer.Normalize(kvp.Key));
            }
        }
    }
}
