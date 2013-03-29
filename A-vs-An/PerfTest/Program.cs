using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvsAnLib;

namespace PerfTest {
	class Program {
		static void Main(string[] args) {
			var words = File.ReadAllLines(@"..\..\354984si.ngl").Where(w => w != "").ToArray();

			var tr = new Dictionary<string, int> { { "a", 0 }, { "an", 1 } };
			long sum = 0;
			Stopwatch sw = Stopwatch.StartNew();
			for (var k = 0; k < 50; k++) {
				for (var i = 0; i < words.Length; i++)
					sum += AvsAn.Query(words[i]).Article == "an" ? 1 : 0;
				for (var i = words.Length - 1; i >= 0; i--)
					sum += AvsAn.Query(words[i]).Article == "an" ? 1 : 0;
			}
			var duration = sw.Elapsed.TotalMilliseconds;
			Console.WriteLine(sum + " / " + words.Length + " (" + ((double)sum / words.Length / 10) + ") an rate.");
			Console.WriteLine("took " + duration);
		}
	}
}
