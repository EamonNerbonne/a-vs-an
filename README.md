a-vs-an
=======
Find the english language indeterminate article ("a" or "an") for a word. Based on real usage patterns extracted from the wikipedia text dump; can therefore even deal with tricky edge cases such as acronyms (FIAT vs. FAA, NASA vs. NSA) and odd symbols.

The implementations (C# and Javascript) in this project determine whether "a" or "an" should precede a word.  They are efficient and accurate (using the method described in [this stackoverflow response](http://stackoverflow.com/questions/1288291/how-can-i-correctly-prefix-a-word-with-a-and-an/1288473#1288473)).

You can try the javascript implementation of this library online: [A-vs-An](http://home.nerbonne.org/A-vs-An/).

The dataset used is based on the [wikipedia-article-text dump](http://en.wikipedia.org/wiki/Wikipedia:Database_download#English-language_Wikipedia) of july 2014.  Some additional preprocessing was done to remove as much wiki-markup as possible and extract only things vaguely resembling sentences using regular expressions. If the word following 'a' or 'an' started with a quote or parenthesis, the initial quote or parenthesis was ignored. The resulting prefix-list with the code to query it is less than 10KB in size; excluding the actual counts would reduce the size still further.

The implementations are efficient: on a single thread of a 3.6GHz i7-4770k a benchmark classifying all words of [an english dictionary](http://wixml.net/moby.html) achieves about 37 million words a second; that's just 100 clock cycles per word. The javascript implementations were benchmarked on chrome 35, firefox 32.0a1 (2014-05-22), IE 11, and opera (12 and 21), and are all about 7-10 times slower, at approximately 4-5 million classifications per second. 

Contributing
---
Contributions welcome!  Feel free to make a suggestion, create a pull request with improvements. Contributed code should be apache 2 licensed, as a-vs-an is.

Thanks in particular to @lukespice for adding .net core support!

