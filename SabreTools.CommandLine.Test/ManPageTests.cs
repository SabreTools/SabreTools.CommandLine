using System;
using System.IO;
using SabreTools.CommandLine.Inputs;
using Xunit;

namespace SabreTools.CommandLine.Test
{
    public class ManPageTests
    {
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

        [Fact]
        public void OutputManPage_Structure_ContainsRequiredSections()
        {
            var set = BuildSampleSet();
            var info = new ManPageInfo("sample") { Version = "sample 1.0", Description = "do sample things" };

            string page = set.OutputManPage(info);

            Assert.Contains(".TH \"SAMPLE\" \"1\"", page);
            Assert.Contains(".SH NAME", page);
            Assert.Contains("sample \\- do sample things", page);
            Assert.Contains(".SH SYNOPSIS", page);
            Assert.Contains(".SH DESCRIPTION", page);
            Assert.Contains(".SH OPTIONS", page);
            Assert.Contains(".TP", page);
        }

        [Fact]
        public void OutputManPage_EscapesHyphensInFlags()
        {
            var set = BuildSampleSet();
            var info = new ManPageInfo("sample");

            string page = set.OutputManPage(info);

            Assert.Contains(".B \\-\\-convert", page);
            Assert.DoesNotContain(".B --convert", page);
        }

        [Fact]
        public void OutputManPage_Verbose_TogglesDetailedText()
        {
            var set = BuildSampleSet();
            var info = new ManPageInfo("sample");

            string terse = set.OutputManPage(info, includeVerbose: false);
            string verbose = set.OutputManPage(info, includeVerbose: true);

            Assert.DoesNotContain("Built\\-in to most of the programs", terse);
            Assert.Contains("Built\\-in to most of the programs", verbose);
        }

        [Fact]
        public void OutputManPage_NestsChildren()
        {
            var set = BuildSampleSet();
            var info = new ManPageInfo("sample");

            string page = set.OutputManPage(info);

            Assert.Contains(".RS", page);
            Assert.Contains(".RE", page);
            Assert.Contains(".B \\-\\-output", page);
        }

        [Fact]
        public void OutputManPage_HonorsCustomOptionsHeading()
        {
            var set = BuildSampleSet();
            var info = new ManPageInfo("sample") { OptionsHeading = "COMMANDS" };

            string page = set.OutputManPage(info);

            Assert.Contains(".SH COMMANDS", page);
            Assert.DoesNotContain(".SH OPTIONS", page);
        }

        [Fact]
        public void OutputManPage_LeavesBlankDateForStamping()
        {
            var set = BuildSampleSet();
            var info = new ManPageInfo("sample") { Version = "sample 1.0" };

            string page = set.OutputManPage(info);

            // Section, then an empty date field, then the version
            Assert.Contains(".TH \"SAMPLE\" \"1\" \"\" \"sample 1.0\"", page);
        }

        [Fact]
        public void OutputManPage_LinesWithinWidth()
        {
            var set = BuildSampleSet();
            var info = new ManPageInfo("sample") { Description = "do sample things" };

            string page = set.OutputManPage(info, includeVerbose: true);

            foreach (string line in page.Split('\n'))
            {
                Assert.True(line.Length <= 78, $"Line exceeds 78 columns: {line}");
            }
        }

        [Fact]
        public void OutputManPage_FileOverload_WritesSameContent()
        {
            var set = BuildSampleSet();
            var info = new ManPageInfo("sample");

            string expected = set.OutputManPage(info);
            string path = Path.Combine(Path.GetTempPath(), $"sabretools-manpage-{Guid.NewGuid():N}.1");
            try
            {
                set.OutputManPage(path, info);
                string actual = File.ReadAllText(path);
                Assert.Equal(expected, actual);
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        [Fact]
        public void ManPageFeature_WritesPageToStandardOutput()
        {
            var set = BuildSampleSet();
            var info = new ManPageInfo("sample") { Version = "sample 1.0" };
            set.Add(new Features.ManPage(info));

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
