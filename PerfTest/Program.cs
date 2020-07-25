using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;
using AvsAnLib;
using AvsAnDemo;

namespace PerfTest
{
    static class Program
    {
        static void Main() {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            var benchdict = Dictionaries.LoadEnglishDictionary();
            var borkedWords = benchdict.Select(w => new string(w.Reverse().ToArray())).ToArray();
            for (var i = 0; i < benchdict.Length; i++) {
                //deterministically shuffle to avoid over-friendliness to the branch-predictor
                var otherI = (i * 13379L + 42) % benchdict.Length;
                var tmp = benchdict[i];
                benchdict[i] = benchdict[otherI];
                benchdict[otherI] = tmp;
            }

            long sum = 0;
            var sw = Stopwatch.StartNew();
            var _ = AvsAn.Query("example").Article;
            var init = sw.Elapsed;
            Console.WriteLine("initialization took " + init.TotalMilliseconds);
            sw.Restart();
            const int iters = 500;
            for (var k = 0; k < iters; k++) {
                foreach (var word in benchdict) {
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
            Console.WriteLine(sum + " / " + benchdict.Length + " (" + (double)sum / benchdict.Length / iters / 2 + ") an rate.");
            var microseconds = duration / (benchdict.Length + borkedWords.Length) / iters * 1000.0;
            Console.WriteLine(microseconds * 1000.0 + " nanoseconds per lookup");

            Console.WriteLine(clockrateMHz * microseconds + " cycles per lookup @ " + clockrateMHz + "MHz");

            Console.WriteLine("took " + duration + "ms");
        }
    }
}
