<Query Kind="Statements">
  <Reference Relative="A-vs-An\AvsAn-Test\bin\Release\ApprovalTests.dll">C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-Test\bin\Release\ApprovalTests.dll</Reference>
  <Reference Relative="A-vs-An\AvsAn-Test\bin\Release\ApprovalUtilities.dll">C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-Test\bin\Release\ApprovalUtilities.dll</Reference>
  <Reference Relative="A-vs-An\AvsAn-Test\bin\Release\AvsAn.dll">C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-Test\bin\Release\AvsAn.dll</Reference>
  <Reference Relative="A-vs-An\AvsAn-Test\bin\Release\AvsAn-Test.dll">C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-Test\bin\Release\AvsAn-Test.dll</Reference>
  <Reference Relative="..\..\emn\programs\EmnExtensions\bin\Release\EmnExtensions.dll">C:\VCS\emn\programs\EmnExtensions\bin\Release\EmnExtensions.dll</Reference>
  <Reference Relative="A-vs-An\AvsAn-Test\bin\Release\ExpressionToCodeLib.dll">C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-Test\bin\Release\ExpressionToCodeLib.dll</Reference>
  <Reference Relative="A-vs-An\WikipediaAvsAnTrieExtractor\bin\Release\WikipediaAvsAnTrieExtractor.exe">C:\VCS\remote\a-vs-an\A-vs-An\WikipediaAvsAnTrieExtractor\bin\Release\WikipediaAvsAnTrieExtractor.exe</Reference>
  <NuGetReference>ExpressionToCodeLib</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>ValueUtils</NuGetReference>
  <Namespace>AvsAn_Test</Namespace>
  <Namespace>AvsAnLib</Namespace>
  <Namespace>AvsAnLib.Internals</Namespace>
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

var rawLookup = NodeSerializer.Deserialize(File.ReadAllText(@"E:\avsan.log",Encoding.UTF8));
var newLookup = rawLookup.Simplify(6);
var oldLookup = BuiltInDictionary.Root;
var dict = Dictionaries.LoadEnglishDictionary();
var badset= new HashSet<string>(@"
contains each either enough enoughs exists ft fth fthm ftncmd ftnerr including includible 
indicate instead instealing insteam it iud iuds dich
abouchement aboudikro aboulia an are if on than un honed onza states
".Split(" \r\n".ToCharArray(),StringSplitOptions.RemoveEmptyEntries));
var badprefixes = @"abou aga amo ulu usur  hong anot hond honi onf lvalue yl herbal ona and unillu
ukiyo unanc ust unissu unidiomatic onei haut
".Split(" \r\n".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
//sf?x?
(from word in dict
 //where !badset.Contains(word)
// where !badprefixes.Any(p=>word.StartsWith(p))
 let classification = WordQuery.Query(oldLookup,word,0)
 let newclassification= WordQuery.Query(newLookup,word,0)
 where classification.Article != newclassification.Article
 let rawClassification = WordQuery.Query(rawLookup, word,0)
 select new {word, 
  old=classification.Article+"|"+classification.Prefix+":"+classification.aCount+"/"+classification.anCount,
  @new=newclassification.Article+"|"+newclassification.Prefix+":"+newclassification.aCount+"/"+newclassification.anCount,
  raw =rawClassification.Article+"|"+rawClassification.Prefix+":"+rawClassification.aCount+"/"+rawClassification.anCount,
  }
 ).Dump();
 
 //an durin (59/46)
 //a exemplifie (23/)