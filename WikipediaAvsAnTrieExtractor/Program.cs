using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using System.Xml.Linq;
using System.Xml;
using System.Diagnostics;

namespace AvsAnTrie {
    static class Program {
        static int Main(string[] args) {
            if (args.Length != 2) {
                Console.Error.WriteLine("Usage: AvsAnTrie <wikidumpfile> <outputfile>\nThe dump is available at http://dumps.wikimedia.org/enwiki/latest/");
                return 1;
            }
            var wikiPath = args[0];
            if (!File.Exists(wikiPath)) {
                Console.Error.WriteLine("The wikipedia dump file could not be found at " + args[0]);
                return 1;
            }
            if (File.Exists(args[1])) {
                Console.Error.WriteLine("The output file " + args[1] + "already exists; delete it or pick another location.");
                return 1;
            }
            var outputFilePath = args[1];

            CreateAvsAnStatistics(wikiPath, outputFilePath);
            return 0;
        }

        static void CreateAvsAnStatistics(string wikiPath, string outputFilePath) {
            var wikiPageQueue = LoadWikiPagesAsync(wikiPath);
            var entriesTodo = ExtractAvsAnSightingsAsync(wikiPageQueue);
            var trieBuilder = BuildAvsAnTrie(entriesTodo, () => wikiPageQueue.Count);
            AnnotatedTrie result = trieBuilder.Result;
            Console.WriteLine("Before simplification: trie of # nodes" + trieBuilder.Result.CountParallel);
            result.Simplify();
            File.AppendAllText(outputFilePath, result.Readable());
            Console.WriteLine("After simplification: trie of # nodes" + trieBuilder.Result.CountParallel);
        }

        static BlockingCollection<AvsAnSighting[]> ExtractAvsAnSightingsAsync(BlockingCollection<XElement> wikiPageQueue) {
            var entriesTodo = new BlockingCollection<AvsAnSighting[]>(3000);

            var sightingExtractionTask = Task.WhenAll(
                Enumerable.Range(0, 32).Select(i =>
                    Task.Factory.StartNew(() => {
                        var ms = new RegexTextUtils();
                        foreach (var page in wikiPageQueue.GetConsumingEnumerable())
                            entriesTodo.Add(ms.FindAvsAnSightings(page));
                    }, TaskCreationOptions.LongRunning)
                    ).ToArray()
                );

            sightingExtractionTask.ContinueWith(_ => entriesTodo.CompleteAdding());
            return entriesTodo;
        }

        static Task<AnnotatedTrie> BuildAvsAnTrie(BlockingCollection<AvsAnSighting[]> entriesTodo, Func<int> wikiPageQueueLength) {
            int wordCount = 0;
            Task.Factory.StartNew(() => {
                Stopwatch sw = Stopwatch.StartNew();
                while (!entriesTodo.IsCompleted) {
                    Thread.Sleep(5000);
                    Console.WriteLine("Entrycache: " + entriesTodo.Count + "; pagecache: " + wikiPageQueueLength() +
                                      "; wordcount: " + wordCount + "; words/sec: " +
                                      (wordCount / sw.Elapsed.TotalSeconds).ToString("f1"));
                }
            }, TaskCreationOptions.LongRunning);

            var trieBuilder = Task.Factory.StartNew(() => {
                var trie = new AnnotatedTrie();
                foreach (var entries in entriesTodo.GetConsumingEnumerable())
                    foreach (var entry in entries) {
                        trie.AddEntry(entry.PrecededByAn, entry.Word, 0);
                        wordCount++;
                    }
                return trie;
            }, TaskCreationOptions.LongRunning);
            return trieBuilder;
        }

        static BlockingCollection<XElement> LoadWikiPagesAsync(string wikiPath) {
            var pagesTodo = new BlockingCollection<XElement>(3000);

            Task.Factory.StartNew(() => {
                using (var stream = File.OpenRead(wikiPath))
                using (var reader = XmlReader.Create(stream))
                    while (reader.Read())
                        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "page")
                            pagesTodo.Add((XElement)XNode.ReadFrom(reader));
                pagesTodo.CompleteAdding();
            }, TaskCreationOptions.LongRunning);
            return pagesTodo;
        }
    }
}
