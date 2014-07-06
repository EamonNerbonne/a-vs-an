<Query Kind="Statements">
  <Reference Relative="A-vs-An\AvsAn-Test\bin\Release\ApprovalTests.dll">C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-Test\bin\Release\ApprovalTests.dll</Reference>
  <Reference Relative="A-vs-An\AvsAn-Test\bin\Release\ApprovalUtilities.dll">C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-Test\bin\Release\ApprovalUtilities.dll</Reference>
  <Reference Relative="A-vs-An\AvsAn-Test\bin\Release\AvsAn.dll">C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-Test\bin\Release\AvsAn.dll</Reference>
  <Reference Relative="A-vs-An\AvsAn-Test\bin\Release\AvsAn-Test.dll">C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-Test\bin\Release\AvsAn-Test.dll</Reference>
  <Reference Relative="..\..\emn\programs\EmnExtensions\bin\Release\EmnExtensions.dll">C:\VCS\emn\programs\EmnExtensions\bin\Release\EmnExtensions.dll</Reference>
  <Reference Relative="A-vs-An\AvsAn-Test\bin\Release\ExpressionToCodeLib.dll">C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-Test\bin\Release\ExpressionToCodeLib.dll</Reference>
  <Reference Relative="A-vs-An\AvsAn-Test\bin\Release\nunit.framework.dll">C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-Test\bin\Release\nunit.framework.dll</Reference>
  <Reference Relative="A-vs-An\WikipediaAvsAnTrieExtractor\bin\Release\WikipediaAvsAnTrieExtractor.exe">C:\VCS\remote\a-vs-an\A-vs-An\WikipediaAvsAnTrieExtractor\bin\Release\WikipediaAvsAnTrieExtractor.exe</Reference>
  <NuGetReference>AvsAn</NuGetReference>
  <NuGetReference>ExpressionToCodeLib</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>ValueUtils</NuGetReference>
  <Namespace>AvsAn_Test</Namespace>
  <Namespace>AvsAnLib</Namespace>
  <Namespace>EmnExtensions</Namespace>
  <Namespace>EmnExtensions.Algorithms</Namespace>
  <Namespace>EmnExtensions.MathHelpers</Namespace>
  <Namespace>ExpressionToCodeLib</Namespace>
  <Namespace>MoreLinq</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.Dynamic</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Xml.Linq</Namespace>
  <Namespace>ValueUtils</Namespace>
  <Namespace>WikipediaAvsAnTrieExtractor</Namespace>
</Query>

var node = MutableNodeExtensions.DeserializeReadable(File.ReadAllText(@"E:\avsan.log",Encoding.UTF8));
var newLookup = node.Simplify(8).Finish(' ');
var dict = Dictionaries.LoadEnglishDictionary();
var badset= new HashSet<string>(@"
contains each either enough enoughs exists ft fth fthm ftncmd ftnerr including includible 
indicate instead instealing insteam it iud iuds 
abouchement aboudikro aboulia an are if on than un
".Split(" \r\n".ToCharArray(),StringSplitOptions.RemoveEmptyEntries));
var badprefixes = @"
abou
aga
amo
ulu
usur 
hong
anot
hond
herbal
ona
unillu
ust
unissu
onei
haut
".Split(" \r\n".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);

(from word in dict
 where !badset.Contains(word)
 where !badprefixes.Any(p=>word.StartsWith(p))
 let classification = AvsAn.Query(word)
 let newclassification= AvsAn.Query(newLookup,word,0)
 where classification.Article != newclassification.Article
 where classification.aCount+classification.anCount > 40
 select new {word, 
  old=classification.Article+"|"+classification.Prefix+":"+classification.aCount+"/"+classification.anCount,
  newC=newclassification.Article+"|"+newclassification.Prefix+":"+newclassification.aCount+"/"+newclassification.anCount}
 ).Dump();
 
 //an durin (59/46)
 //a exemplifie (23/)
 