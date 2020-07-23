using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using AvsAnLib;
using AvsAnDemo;

namespace PerfTest
{
    internal static class Program
    {
        static void Main()
        {
            var words = Dictionaries.LoadEnglishDictionary();
            var borkedWords = words.Select(w => new string(w.Reverse().ToArray())).ToArray();
            for (var i = 0; i < words.Length; i++) {
                //deterministically shuffle to avoid over-friendliness to the branch-predictor
                var otherI = (i * 13379L + 42) % words.Length;
                var tmp = words[i];
                words[i] = words[otherI];
                words[otherI] = tmp;
            }

            long sum = 0;
            var sw = Stopwatch.StartNew();
            var _ = AvsAn.Query("example").Article;
            var init = sw.Elapsed;
            Console.WriteLine("initialization took " + init.TotalMilliseconds);
            sw.Restart();
            const int iters = 100;
            for (var k = 0; k < iters; k++) {
                foreach (var word in words) {
                    sum += AvsAn.Query(word).Article == "an" ? 1 : 0;
                }

                foreach (var word in borkedWords) {
                    sum += AvsAn.Query(word).Article == "an" ? 1 : 0;
                }
            }
            var duration = sw.Elapsed.TotalMilliseconds;
            var clockrateMHz =
                new ManagementObjectSearcher(new ObjectQuery("SELECT CurrentClockSpeed FROM Win32_Processor")).Get()
                    .Cast<ManagementObject>()
                    .Select(mo => Convert.ToDouble(mo["CurrentClockSpeed"]))
                    .Max();
            Console.WriteLine(sum + " / " + words.Length + " (" + (double)sum / words.Length / iters / 2 + ") an rate.");
            var microseconds = duration / (words.Length + borkedWords.Length) / iters * 1000.0;
            Console.WriteLine(microseconds * 1000.0 + " nanoseconds per lookup");

            Console.WriteLine(clockrateMHz * microseconds + " cycles per lookup @ " + clockrateMHz + "MHz");

            Console.WriteLine("took " + duration + "ms");
        }
    }
}
