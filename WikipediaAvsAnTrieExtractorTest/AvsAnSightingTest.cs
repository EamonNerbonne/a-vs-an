using System.Linq;
using WikipediaAvsAnTrieExtractor;
using Xunit;

namespace WikipediaAvsAnTrieExtractorTest {
    public class AvsAnSightingTest {
        static readonly RegexTextUtils utils = UtilsInstance.Utils;

        [Fact]
        public void IgnoresAnBeforeD() {
            var sightings = utils.FindAvsAnSightings("Beethoven premiered the opera at the Theater an der Wien, which was an important venue and a daring choice.");
            Assert.Equal(new[] { "an important", "a daring" }, sightings.Select(s => (s.PrecededByAn ? "an " : "a ") + s.Word));
        }
    }
}
