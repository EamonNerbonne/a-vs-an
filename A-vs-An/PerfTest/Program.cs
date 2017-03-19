using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using AvsAnLib;
using AvsAn_Test;

namespace PerfTest {
    static class Program {
        static void Main(string[] args) {
            var words = Dictionaries.LoadEnglishDictionary();
            
            long sum = 0;
            Stopwatch sw = Stopwatch.StartNew();
            var _ = AvsAn.Query("example").Article;
            var init = sw.Elapsed;
            Console.WriteLine("initialization took " + init.TotalMilliseconds);
            sw.Restart();
            const int iters = 400;
            for (var k = 0; k < iters; k++) {
                for (var i = 0; i < words.Length; i++)
                    sum += AvsAn.Query(words[i]).Article == "an" ? 1 : 0;
                for (var i = words.Length - 1; i >= 0; i--)
                    sum += AvsAn.Query(words[i]).Article == "an" ? 1 : 0;
            }
            var duration = sw.Elapsed.TotalMilliseconds;
            var clockrateMHz =
                new ManagementObjectSearcher(new ObjectQuery("SELECT CurrentClockSpeed FROM Win32_Processor")).Get()
                .Cast<ManagementObject>().Select(mo => Convert.ToDouble(mo["CurrentClockSpeed"])).Max();
            Console.WriteLine(sum + " / " + words.Length + " (" + ((double)sum / words.Length / iters / 2) + ") an rate.");
            var microseconds = duration / words.Length / iters / 2 * 1000.0;
            Console.WriteLine(microseconds * 1000.0 + " nanoseconds per lookup");

            Console.WriteLine(clockrateMHz * microseconds + " cycles per lookup @ " + clockrateMHz + "MHz");

            Console.WriteLine("took " + duration + "ms");
        }
    }
}
