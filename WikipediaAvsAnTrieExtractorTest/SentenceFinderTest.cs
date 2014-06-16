using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikipediaAvsAnTrieExtractor;
using Xunit;

namespace WikipediaAvsAnTrieExtractorTest {
    public class SentenceFinderTest {
        readonly RegexTextUtils utils = new RegexTextUtils();

        [Fact]
        public void FindsTrivialSentences() {
            Assert.Equal(new[] { "Mary had a little Lamb." },
                utils.FindEnglishSentences("Mary had a little Lamb."));
            Assert.Equal(new[] { "Hello World!" },
                utils.FindEnglishSentences("Hello World!"));
            Assert.Equal(new[] { "Are you sure this works?" },
                utils.FindEnglishSentences("Are you sure this works?"));
        }
        [Fact]
        public void DoesNotFindNonsense() {
            Assert.Empty(utils.FindEnglishSentences("asdfasdf asdf "));
            Assert.Empty(utils.FindEnglishSentences(""));
            Assert.Empty(utils.FindEnglishSentences("\n\n\n\ntest\n\n\n"));
        }

        [Fact]
        public void FindsBothRealisticSentences() {
            Assert.Equal(new[] { 
                "The Etruscans brought the Greek alphabet to their civilization in the Italian Peninsula and left the letter unchanged.", 
                "The Romans later adopted the Etruscan alphabet to write the Latin language, and the resulting letter was preserved in the modern Latin alphabet used to write many languages, including English."
            }, utils.FindEnglishSentences(
@"The Etruscans brought the Greek alphabet to their civilization in the Italian Peninsula and left the letter unchanged. The Romans later adopted the Etruscan alphabet to write the Latin language, and the resulting letter was preserved in the modern Latin alphabet used to write many languages, including English."
));
            Assert.Equal(new[] {
                @"Each frame was drawn on paper; which invariably required backgrounds and characters to be redrawn and animated.",
                @"Among McCay's most noted films are Little Nemo (1911), Gertie the Dinosaur (1914) and The Sinking of the Lusitania (1918)."
            },
    utils.FindEnglishSentences(
@"Each frame was drawn on paper; which invariably required backgrounds and characters to be redrawn and animated. Among McCay's most noted films are Little Nemo (1911), Gertie the Dinosaur (1914) and The Sinking of the Lusitania (1918)."
));

        }

        [Fact]
        public void SplitSentencesWithSimpleQuotes() {
            Assert.Equal(new[] { 
                        @"""A"" is the third common used letter in English, and the second most common in Spanish and French.",
                        @"In one study, on average, about 3.68% of letters used in English tend to be ‹a›s, while the number is 6.22% in Spanish and 3.95% in French.",
                        @"""A"" is often used to denote something or someone of a better or more prestigious quality or status: A-, A or A+, the best grade that can be assigned by teachers for students' schoolwork; A grade for clean restaurants; A-List celebrities, etc.",
                        @"Such associations can have a motivating effect as exposure to the letter A has been found to improve performance, when compared with other letters."
                        }, utils.FindEnglishSentences(
@"""A"" is the third common used letter in English, and the second most common in Spanish and French. In one study, on average, about 3.68% of letters used in English tend to be ‹a›s, while the number is 6.22% in Spanish and 3.95% in French.

""A"" is often used to denote something or someone of a better or more prestigious quality or status: A-, A or A+, the best grade that can be assigned by teachers for students' schoolwork; A grade for clean restaurants; A-List celebrities, etc. Such associations can have a motivating effect as exposure to the letter A has been found to improve performance, when compared with other letters."
));
        }

        [Fact]
        public void AllowsInitials() {
            Assert.Equal(new[] { "J. Stuart Blackton was possibly the first American filmmaker to use the techniques of stop-motion and hand-drawn animation." }, utils.FindEnglishSentences(
@"J. Stuart Blackton was possibly the first American filmmaker to use the techniques of stop-motion and hand-drawn animation."
));
        }

        [Fact]
        public void SupportsDottedAcronyms() {
            Assert.Equal(new[] {
                @"Following World War II, Alabama experienced significant recovery as the economy of the state transitioned from agriculture to diversified interests in heavy manufacturing, mineral extraction, education, and technology, as well as the establishment or expansion of multiple military installations, primarily those of the U.S. Army and U.S. Air Force.",
            @"The state has heavily invested in aerospace, education, health care, and banking, and various heavy industries including automobile manufacturing, mineral extraction, steel production and fabrication."
            }, utils.FindEnglishSentences(
@"Following World War II, Alabama experienced significant recovery as the economy of the state transitioned from agriculture to diversified interests in heavy manufacturing, mineral extraction, education, and technology, as well as the establishment or expansion of multiple military installations, primarily those of the U.S. Army and U.S. Air Force. The state has heavily invested in aerospace, education, health care, and banking, and various heavy industries including automobile manufacturing, mineral extraction, steel production and fabrication."
));
        }

        [Fact]
        public void Supports_LtGovAcronyms() {
            Assert.Equal(new[] {
                @"Only one Republican Lt. Governor has been elected since Reconstruction, Steve Windom.",
            @"Windom served as Lt. Governor under Democratic Gov. Don Siegelman."
            }, utils.FindEnglishSentences(
@"Only one Republican Lt. Governor has been elected since Reconstruction, Steve Windom. Windom served as Lt. Governor under Democratic Gov. Don Siegelman."
));
        }

        [Fact]
        public void Supports_DrAcronyms() {
            Assert.Equal(new[] {
                @"Mamet also appeared as a guest on Episode 312 of the animated Comedy Central program Dr. Katz, Professional Therapist.",
            @"The episode, ""New Phone System,"" originally aired on March 2, 1997."
            }, utils.FindEnglishSentences(
@"Mamet also appeared as a guest on Episode 312 of the animated Comedy Central program Dr. Katz, Professional Therapist. The episode, ""New Phone System,"" originally aired on March 2, 1997."
));
        }

        [Fact]
        public void Supports_EG()
        {
            Assert.Equal(new[] {
                @"People (e.g. Eamon) are weird.",
            }, utils.FindEnglishSentences(
@"People (e.g. Eamon) are weird."
));
        }
        [Fact]
        public void Supports_IE() {
            Assert.Equal(new[] {
                @"People (i.e. Eamon) are weird.",
            }, utils.FindEnglishSentences(
@"People (i.e. Eamon) are weird."
));
        }

        //Mamet also appeared as a guest on Episode 312 of the animated Comedy Central program Dr. Katz, Professional Therapist. The episode, "New Phone System," originally aired on March 2, 1997.
    }
}
