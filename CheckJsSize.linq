<Query Kind="Statements">
  <Reference Relative="A-vs-An\A-vs-An-DotNet\bin\Release\AvsAn.dll">C:\VCS\remote\a-vs-an\A-vs-An\A-vs-An-DotNet\bin\Release\AvsAn.dll</Reference>
  <Reference Relative="..\..\emn\programs\EmnExtensions\bin\Release\EmnExtensions.dll">C:\VCS\emn\programs\EmnExtensions\bin\Release\EmnExtensions.dll</Reference>
  <NuGetReference>AjaxMin</NuGetReference>
  <NuGetReference>ExpressionToCodeLib</NuGetReference>
  <NuGetReference>FSPowerPack.Core.Community</NuGetReference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>ValueUtils</NuGetReference>
  <NuGetReference>YUICompressor.NET</NuGetReference>
  <Namespace>AvsAnLib</Namespace>
  <Namespace>AvsAnLib.Internals</Namespace>
  <Namespace>EmnExtensions</Namespace>
  <Namespace>EmnExtensions.Algorithms</Namespace>
  <Namespace>EmnExtensions.MathHelpers</Namespace>
  <Namespace>ExpressionToCodeLib</Namespace>
  <Namespace>Microsoft.Ajax.Utilities</Namespace>
  <Namespace>Microsoft.FSharp.Collections</Namespace>
  <Namespace>Microsoft.FSharp.Core</Namespace>
  <Namespace>MoreLinq</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.Dynamic</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Xml.Linq</Namespace>
  <Namespace>ValueUtils</Namespace>
  <Namespace>Yahoo.Yui.Compressor</Namespace>
  <Namespace>System.IO.Compression</Namespace>
</Query>

var basename = "AvsAn";
var dir = @"C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-JsDemo\";
var src = File.ReadAllText(dir+basename+".js",Encoding.UTF8);
var ms = new Minifier();


var mini1=ms.MinifyJavaScript(src);


src.Length.Dump();
mini1.Length.Dump();
File.WriteAllText(dir+basename+".min.js",mini1,new UTF8Encoding(false));
var res = EmnExtensions.WinProcessUtil.ExecuteProcessSynchronously(
@"C:\Program Files\7-zip\7z.exe",
@"a """+dir+basename+".min.js.gz"+@""" -tgzip -mfb=258 -mx=9 -mpass=15 """+dir+basename+".min.js"+@"""",""); 
var file = new FileInfo(dir+basename+".min.js.gz");
file.Length.Dump();
string roundtripped;
using (var stream = file.OpenRead())
	using(var unzip = new GZipStream(stream,CompressionMode.Decompress))
		using(var reader = new StreamReader(unzip,Encoding.UTF8))
			roundtripped = reader.ReadToEnd();

(roundtripped == mini1).Dump();
