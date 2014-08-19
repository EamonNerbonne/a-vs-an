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

var basename = "AvsAn-simple";
var dir = @"C:\VCS\remote\a-vs-an\A-vs-An\AvsAn-JsDemo\";
var src = File.ReadAllText(dir+basename+".js",Encoding.UTF8);
var ms = new Minifier();


var mini1=ms.MinifyJavaScript(src);


src.Length.Dump();
mini1.Length.Dump();
File.WriteAllText(dir+basename+".min.js",mini1,Encoding.UTF8);
var res = EmnExtensions.WinProcessUtil.ExecuteProcessSynchronously(
@"C:\Program Files\7-zip\7z.exe",
@"a DUMMY -tgzip -mfb=258 -mx=9 -mpass=15 -si -so",mini1, new ProcessStartOptions{
 StandardInputEncoding = Encoding.UTF8,
 StandardOutputAndErrorEncoding = Encoding.GetEncoding(1251)
}); 
res.StandardOutputContents.Length.Dump();
res.StandardErrorContents.Dump();

var bytes = Encoding.GetEncoding(1251).GetBytes(res.StandardOutputContents);
var sink = new MemoryStream();
new GZipStream(new MemoryStream(bytes),CompressionMode.Decompress).CopyTo(sink);
var roundtripped = Encoding.UTF8.GetString(sink.ToArray());

(roundtripped == mini1).Dump();