using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using AvsAnLib.Internals;
using ExpressionToCodeLib;
// ReSharper disable AccessToModifiedClosure
// ReSharper disable AccessToDisposedClosure

namespace WikipediaAvsAnTrieExtractor {
    static class Program {
        const int PageBlocksQueueSize = 20;
        const int SightingBlocksQueueSize = 200;
        const int PagesPerBlock = 1000;
        static readonly ConcurrentBag<Func<string>> ProgressReporters = new ConcurrentBag<Func<string>>();

        static int Main(string[] args) {
            if (args.Length == 2 && args[0] == "-simplify") {
                Simplify(args[1]);
            } else if (args.Length == 3 && args[0] == "-extract") {
                return Extract(args) ? 0 : 1;
            }

            PrintUsage();
            return 1;
        }

        static void PrintUsage() {
            Console.Error.WriteLine("Usage: AvsAnTrie <wikidumpfile> <outputfile>");
            Console.Error.WriteLine("The dump is available at http://dumps.wikimedia.org/enwiki/latest/");
            Console.Error.WriteLine("The appropriate dump is enwiki-latest-pages-articles.xml.bz2, though some others will work too (in particular, pages-meta-current includes talk pages).");
        }

        static bool Extract(string[] args) {
            var wikiPath = args[1];
            if (!File.Exists(wikiPath)) {
                Console.Error.WriteLine("The wikipedia dump file could not be found at " + args[1]);
                return false;
            }

            var outputFilePath = args[2];
            Task.Factory.StartNew(
                () => {
                    while (true) {
                        Thread.Sleep(1000);
                        Console.WriteLine(string.Join("; ", ProgressReporters.Select(f => f())));
                    }

                    // ReSharper disable once FunctionNeverReturns
                },
                TaskCreationOptions.LongRunning
            );

            CreateAvsAnStatistics(wikiPath, outputFilePath);
            return true;
        }

        static void Simplify(string input) {
            var newLookup = NodeDeserializer.DeserializeDense(File.ReadAllText(input, Encoding.UTF8)).Simplify(5).UnmarkUnsure(3);

            Console.WriteLine("Simplified dense representation on next line:");
            Console.WriteLine(ObjectToCode.PlainObjectToCode(NodeSerializer.SerializeDense(newLookup)));
            Console.WriteLine();
            Console.WriteLine("Simplified no-statistics dense representation on next line:");
            Console.WriteLine(ObjectToCode.PlainObjectToCode(NodeSerializer.SerializeDenseNoStats(newLookup)));
        }

        static void CreateAvsAnStatistics(string wikiPath, string outputFilePath) {
            var wikiPageQueue = LoadWikiPagesAsync(wikiPath);
            var entriesTodo = ExtractAvsAnSightingsAsync(wikiPageQueue);
            var trieBuilder = BuildAvsAnTrie(entriesTodo);
            var result = trieBuilder.Result;
            Console.WriteLine("Raw trie of # nodes" + trieBuilder.Result.Count());
            File.WriteAllText(outputFilePath, NodeSerializer.SerializeDense(result), Encoding.UTF8);
        }

        static BlockingCollection<AvsAnSighting[]> ExtractAvsAnSightingsAsync(BlockingCollection<string[]> wikiPageQueue) {
            var entriesTodo = new BlockingCollection<AvsAnSighting[]>(SightingBlocksQueueSize);
            ProgressReporters.Add(() => "wordQ: " + entriesTodo.Count);

            var sightingExtractionTask = Task.WhenAll(
                Enumerable.Range(0, Environment.ProcessorCount)
                    .Select(
                        i =>
                            Task.Factory.StartNew(
                                () => {
                                    var ms = new RegexTextUtils();
                                    foreach (var pageSet in wikiPageQueue.GetConsumingEnumerable()) {
                                        foreach (var page in pageSet) {
                                            entriesTodo.Add(ms.FindAvsAnSightings(page));
                                        }
                                    }
                                },
                                TaskCreationOptions.LongRunning
                            )
                    )
                    .ToArray()
            );

            sightingExtractionTask.ContinueWith(
                t => {
                    if (t.IsFaulted) {
                        Console.WriteLine(t.Exception);
                    }

                    entriesTodo.CompleteAdding();
                }
            );
            return entriesTodo;
        }

        static Task<Node> BuildAvsAnTrie(BlockingCollection<AvsAnSighting[]> entriesTodo) {
            var wordCount = 0;

            var sw = Stopwatch.StartNew();
            ProgressReporters.Add(() => "words/ms: " + (wordCount / sw.Elapsed.TotalMilliseconds).ToString("f1"));

            var trieBuilder = Task.Factory.StartNew(
                () => {
                    var trie = new Node();
                    foreach (var entries in entriesTodo.GetConsumingEnumerable()) {
                        foreach (var entry in entries) {
                            IncrementPrefixExtensions.IncrementPrefix(ref trie, entry.PrecededByAn, entry.Word, 0);
                            wordCount++;
                        }
                    }

                    return trie;
                },
                TaskCreationOptions.LongRunning
            );
            return trieBuilder;
        }

        static BlockingCollection<string[]> LoadWikiPagesAsync(string wikiPath) {
            var wikiPageQueue = new BlockingCollection<string[]>(PageBlocksQueueSize);

            ProgressReporters.Add(() => "pageQ: " + wikiPageQueue.Count);

            Task.Factory.StartNew(
                () => {
                    var sw = Stopwatch.StartNew();
                    using var stream = File.OpenRead(wikiPath);
                    using var reader = XmlReader.Create(stream);

                    var stopped = false;
                    try {
                        long pageCount = 0;
                        var percentScale = 100.0 / stream.Length;
                        // ReSharper disable once AccessToDisposedClosure
                        ProgressReporters.Add(() => stopped ? "" : (stream.Position * percentScale).ToString("f1") + "%");
                        ProgressReporters.Add(() => stopped ? "" : "MB/s: " + (stream.Position / 1024.0 / 1024.0 / sw.Elapsed.TotalSeconds).ToString("f1"));
                        ProgressReporters.Add(() => stopped ? "" : "pages/ms: " + (pageCount / sw.Elapsed.TotalMilliseconds).ToString("f1"));
                        var pages = new string[1];
                        var i = 0;

                        while (reader.Read()) {
                            if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "page") {
                                if (reader.ReadToDescendant("text") && reader.Read() && reader.Value != null) {
                                    pageCount++;
                                    pages[i] = reader.Value;
                                    i++;
                                    if (i == pages.Length) {
                                        wikiPageQueue.Add(pages);
                                        pages = new string[Math.Min(pages.Length + 1, PagesPerBlock)];
                                        i = 0;
                                    }
                                }
                            }
                        }

                        wikiPageQueue.Add(pages.Take(i).ToArray());
                    } finally {
                        stopped = true;
                    }


                    wikiPageQueue.CompleteAdding();
                },
                TaskCreationOptions.LongRunning
            );
            return wikiPageQueue;
        }
    }
}
