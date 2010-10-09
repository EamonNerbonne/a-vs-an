using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using System.Xml.Linq;
using System.Xml;
using System.Reflection;
using WikiParser;

namespace AvsAnTrie {
	class Program {
		const string dictFileName = "english.ngl";
		static void Main(string[] args) {
			FileInfo exeFile = new FileInfo(Assembly.GetEntryAssembly().Location);
			var dir = exeFile.Directory;
			while (dir != null && !dir.GetFiles(dictFileName).Any())
				dir = dir.Parent;
			RegexTextUtils.LoadDictionary(dir.GetFiles(dictFileName).First());

			var wikiPath = new FileInfo(@"D:\EamonLargeDocs\wikipedia\enwiki-latest-pages-articles.xml");
			BlockingCollection<Entry[]> entriesTodo = new BlockingCollection<Entry[]>(10000);
			BlockingCollection<XElement> pagesTodo = new BlockingCollection<XElement>(10000);
			int wordCount = 0;
			Task.Factory.StartNew(() => {
				while (!entriesTodo.IsCompleted) {
					Thread.Sleep(5000);
					Console.WriteLine("Entrycache: " + entriesTodo.Count + "; pagecache: " + pagesTodo.Count + "; wordcount: " + wordCount);
				}
			});

			var pageReader = Task.Factory.StartNew(() => {
				using (var stream = wikiPath.OpenRead())
				using (var reader = XmlReader.Create(stream))
					while (reader.Read())
						if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "page")
							pagesTodo.Add((XElement)XElement.ReadFrom(reader));
				pagesTodo.CompleteAdding();
			});

			var trieBuilder = Task.Factory.StartNew(() => {
				AnnotatedTrie trie = new AnnotatedTrie();
				foreach (var entries in entriesTodo.GetConsumingEnumerable())
					foreach (var entry in entries) {
						trie.AddEntry(entry.PrecededByAn, entry.Word, 0);
						wordCount++;
					}
				return trie;
			});

			Task.WaitAll(
				Enumerable.Range(0, 32).Select(i =>
					Task.Factory.StartNew(() => {
						ExtractEntriesFromPages(pagesTodo, entriesTodo);
					})
				).ToArray()
			);

			//Parallel.ForEach(pagesTodo.GetConsumingEnumerable(), new ParallelOptions { MaxDegreeOfParallelism = 128 }, () => new WikiMarkupStripper(), (page, state, ms) => {
			//    ProcessPage(entriesTodo, page, ms);
			//    return ms;
			//}, ms => { });

			entriesTodo.CompleteAdding();
			AnnotatedTrie result = trieBuilder.Result;
			Console.WriteLine("Before simplification: trie of # nodes" + trieBuilder.Result.CountParallel);
			result.Simplify();
			File.AppendAllText(@"D:\test.log", result.Readable());
			Console.WriteLine("After simplification: trie of # nodes" + trieBuilder.Result.CountParallel);
		}

		static void ExtractEntriesFromPages(BlockingCollection<XElement> pagesTodo, BlockingCollection<Entry[]> entriesTodo) {
			RegexTextUtils ms = new RegexTextUtils();
			foreach (var page in pagesTodo.GetConsumingEnumerable())
				ProcessPage(entriesTodo, page, ms);
		}

		static void ProcessPage(BlockingCollection<Entry[]> entriesTodo, XElement page, RegexTextUtils ms) {
			string pagetext = ms.StripMarkup(WikiXmlReader.GetArticleText(page));
			string pageWSnormal = WhitespaceNormalizer.Normalize(pagetext);

			Entry[] entries = (
				from sentence in ms.FindEnglishSentences(pageWSnormal)
				where ms.GradeEnglishSentence(sentence) > 2.2
				from entry in ms.ExtractWordsPrecededByAOrAn(sentence)
				select entry).ToArray();
			entriesTodo.Add(entries);
		}
	}
}
