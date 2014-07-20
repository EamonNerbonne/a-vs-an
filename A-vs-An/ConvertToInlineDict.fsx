#r @"WikipediaAvsAnTrieExtractor\bin\Release\WikipediaAvsAnTrieExtractor.exe"
#r @"WikipediaAvsAnTrieExtractor\bin\Release\AvsAn.dll"
#r @"WikipediaAvsAnTrieExtractor\bin\Release\ExpressionToCodeLib.dll"

open System.IO
open System
open System.Text
open WikipediaAvsAnTrieExtractor
open AvsAnLib
open AvsAnLib.Internals
open ExpressionToCodeLib

let rawDictPath = @"E:\avsan-old.log"

let newLookup =
    File.ReadAllText(@"E:\avsan-old.log",Encoding.UTF8)
    |> ReadableSerializationExtension.DeserializeReadable
    |> (fun n -> n.Simplify(6))
    |> Node.CreateFromMutable

newLookup.SerializeToDenseHex() 
    |> ObjectToCode.PlainObjectToCode
    |> printfn "%s"
