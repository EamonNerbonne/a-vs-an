using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using ApprovalTests.Approvers;
using ApprovalTests.Core;
using ApprovalTests.Reporters;
using ApprovalTests.Writers;
using AvsAnDemo;
using AvsAnLib;
using Xunit;

namespace AvsAn_Test
{
    [UseReporter(typeof(DiffReporter))]
    public class CompleteDictionaryCheck
    {
        class SaneNamer : IApprovalNamer
        {
            public string SourcePath { get; set; }
            public string Name { get; set; }
        }

        // ReSharper disable once UnusedParameter.Local
        static void MyApprove(string text, object IGNORE_PAST_THIS = null, [CallerFilePath] string filepath = null, [CallerMemberName] string membername = null)
        {
            var writer = WriterFactory.CreateTextWriter(text);
            var filename = Path.GetFileNameWithoutExtension(filepath);
            var filedir = Path.GetDirectoryName(filepath);
            var namer = new SaneNamer { Name = filename + "." + membername, SourcePath = filedir };
            var reporter = new DiffReporter();
            Approver.Verify(new FileApprover(writer, namer, true), reporter);
        }

        [Fact]
        public void AcronymClassifications()
        {
            var mappedAcronyms = string.Join("\n", Dictionaries.AcronymsWithUpto4Letters().Select(word => word + " => " + AvsAn.Query(word).Article));
            MyApprove(mappedAcronyms);
        }

        [Fact]
        public void DictionaryClassifications()
        {
            var dictionary = Dictionaries.LoadEnglishDictionary();
            var mappedDictionary = string.Join("\n", dictionary.Select(word => word + " => " + AvsAn.Query(word).Article));
            MyApprove(mappedDictionary);
        }

        [Fact]
        public void NumberClassifications()
        {
            var numbers = Dictionaries.SmallNumberStrings();
            var mappedNumbers = string.Join("\n", numbers.Select(word => word + " => " + AvsAn.Query(word).Article));
            MyApprove(mappedNumbers);
        }
    }
}
