a-vs-an
=======
Find the english language indeterminate article ("a" or "an") for a word. Based on real usage patterns extracted from the wikipedia text dump; can therefore even deal with tricky edge cases such as acronyms (FIAT vs. FAA, NASA vs. NSA) and odd symbols.

The implementations (C# and Javascript) in this project determine whether "a" or "an" should precede a word.  They are efficient and accurate (using the method described in [this stackoverflow response](http://stackoverflow.com/questions/1288291/how-can-i-correctly-prefix-a-word-with-a-and-an/1288473#1288473)).

You can try the javascript implementation of this library online: [A-vs-An](http://home.nerbonne.org/A-vs-An/).

The dataset used is based on the [wikipedia-article-text dump](http://en.wikipedia.org/wiki/Wikipedia:Database_download#English-language_Wikipedia) of april 2010.  Some additional preprocessing was done to remove as much wiki-markup as possible and extract only things vaguely resembling sentences using regular expressions. If the word following 'a' or 'an' started with a quote or parenthesis, the initial quote or parenthesis was ignored. The resulting prefix-list with the code to query it is less than 10KB in size; excluding the actual counts would reduce the size still further.

The implementations are efficient: on a single thread of a 2.5GHz Q9300 a benchmark classifying all words of an english dictionary achieves about 15 million words a second; that's just 166 clock cycles per word. The javascript implementations were benchmarked on chrome 26, firefox 22, IE 10, and opera 12, and are about 5-10 times slower, at approximately 2 million classifications per second. 
