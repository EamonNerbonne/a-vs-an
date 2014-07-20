<Query Kind="Statements">
  <Reference Relative="A-vs-An\AvsAn-Test\bin\Release\ApprovalTests.dll">C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-Test\bin\Release\ApprovalTests.dll</Reference>
  <Reference Relative="A-vs-An\AvsAn-Test\bin\Release\ApprovalUtilities.dll">C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-Test\bin\Release\ApprovalUtilities.dll</Reference>
  <Reference Relative="A-vs-An\AvsAn-Test\bin\Release\AvsAn.dll">C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-Test\bin\Release\AvsAn.dll</Reference>
  <Reference Relative="A-vs-An\AvsAn-Test\bin\Release\AvsAn-Test.dll">C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-Test\bin\Release\AvsAn-Test.dll</Reference>
  <Reference Relative="..\..\emn\programs\EmnExtensions\bin\Release\EmnExtensions.dll">C:\VCS\emn\programs\EmnExtensions\bin\Release\EmnExtensions.dll</Reference>
  <Reference Relative="A-vs-An\AvsAn-Test\bin\Release\ExpressionToCodeLib.dll">C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-Test\bin\Release\ExpressionToCodeLib.dll</Reference>
  <Reference Relative="A-vs-An\AvsAn-Test\bin\Release\nunit.framework.dll">C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-Test\bin\Release\nunit.framework.dll</Reference>
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

var node = Node.CreateFromMutable(ReadableSerializationExtension.DeserializeReadable(File.ReadAllText(@"E:\avsan-old.log",Encoding.UTF8)));
node.Count().Dump();
node.Simplify(6).Count().Dump();
BuiltInDictionary.Root.Count().Dump();

node.SerializeToDenseHex().Length.Dump();
BuiltInDictionary.Root.SerializeToDenseHex().Length.Dump();
