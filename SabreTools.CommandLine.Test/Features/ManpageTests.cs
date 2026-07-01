using System;
using System.IO;
using SabreTools.CommandLine.Features;
using SabreTools.CommandLine.Inputs;
using Xunit;

namespace SabreTools.CommandLine.Test.Features
{
    public class ManpageTests
    {
        [Fact]
        public void ManpageFeature_WritesPageToStandardOutput()
        {
            var set = BuildSampleSet();
            var info = new ManpageInfo("sample") { Version = "sample 1.0" };
            set.Add(new Manpage(info));

            var original = Console.Out;
            var writer = new StringWriter();
            string output;
            try
            {
                Console.SetOut(writer);
                bool result = set.ProcessArgs(["man"]);
                Assert.True(result);
            }
            finally
            {
                Console.SetOut(original);
            }

            output = writer.ToString();
            Assert.Contains(".TH \"SAMPLE\" \"1\"", output);
            Assert.Contains(".SH NAME", output);
            Assert.Contains(".B \\-\\-convert", output);
        }

        /// <summary>
        /// Build a representative command set with nested inputs and detail text
        /// </summary>
        private static CommandSet BuildSampleSet()
        {
            var set = new CommandSet("Sample header line");

            var help = new MockFeature("Help", ["?", "h", "help"], "Show this help",
                "Built-in to most of the programs is a basic help text.");
            set.Add(help);

            var convert = new MockFeature("Convert", "--convert", "Convert input files",
                "Converts the provided input files into the desired output format.");
            convert.Add(new StringInput("output", "--output", "Set the output path"));
            convert.Add(new FlagInput("force", "--force", "Overwrite existing files"));
            set.Add(convert);

            return set;
        }

        /// <summary>
        /// Mock Feature implementation for testing
        /// </summary>
        private class MockFeature : Feature
        {
            public MockFeature(string name, string flag, string description, string? detailed = null)
                : base(name, flag, description, detailed)
            {
            }

            public MockFeature(string name, string[] flags, string description, string? detailed = null)
                : base(name, flags, description, detailed)
            {
            }

            /// <inheritdoc/>
            public override bool Execute() => true;

            /// <inheritdoc/>
            public override bool VerifyInputs() => true;
        }
    }
}
