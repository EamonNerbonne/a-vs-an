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

let rawDictPath = @"E:\avsan.log"

let newLookup =
    File.ReadAllText(rawDictPath, Encoding.UTF8)
    |> ReadableSerializationExtension.DeserializeReadable
    |> Node.CreateFromMutable
    |> (fun n -> n.Simplify(6))

newLookup.SerializeToDenseHex() 
    |> ObjectToCode.PlainObjectToCode
    |> printfn "%s"
